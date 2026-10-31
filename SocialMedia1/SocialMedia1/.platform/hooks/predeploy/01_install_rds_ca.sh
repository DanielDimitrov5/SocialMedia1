#!/bin/bash
set -euo pipefail

workdir="$(mktemp -d)"
trap 'rm -rf "$workdir"' EXIT

curl --fail --silent --show-error --location \
  https://truststore.pki.rds.amazonaws.com/eu-north-1/eu-north-1-bundle.pem \
  -o "$workdir/bundle.pem"

# Separate the bundle into individual certificates.
awk -v dir="$workdir" '
/-----BEGIN CERTIFICATE-----/ {
    n++
    file = sprintf("%s/cert-%03d.pem", dir, n)
}
file != "" { print > file }
/-----END CERTIFICATE-----/ {
    close(file)
    file = ""
}
' "$workdir/bundle.pem"

installed=0

for cert in "$workdir"/cert-*.pem; do
    subject="$(openssl x509 -in "$cert" -noout -subject -nameopt RFC2253)"
    issuer="$(openssl x509 -in "$cert" -noout -issuer -nameopt RFC2253)"

    subject="${subject#subject=}"
    issuer="${issuer#issuer=}"

    if [[ "$subject" == "$issuer" ]] &&
       [[ "$subject" == *"Amazon RDS eu-north-1 Root CA RSA2048 G1"* ]]; then
        openssl verify -CAfile "$cert" "$cert"
        install -m 0644 "$cert" \
          /etc/pki/ca-trust/source/anchors/rds-eu-north-1-rsa2048-g1.crt
        installed=1
        break
    fi
done

if [[ "$installed" -ne 1 ]]; then
    echo "ERROR: Expected RDS RSA2048 G1 root CA was not found." >&2
    exit 1
fi

update-ca-trust extract
echo "RDS root CA installed successfully."