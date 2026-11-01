# __generated__ by Terraform
# Please review these resources and move them into your main configuration files.

# __generated__ by Terraform
resource "aws_db_instance" "socialmedia" {
  allocated_storage                     = 20
  allow_major_version_upgrade           = null
  apply_immediately                     = null
  auto_minor_version_upgrade            = false
  availability_zone                     = "eu-north-1c"
  backup_retention_period               = 7
  backup_target                         = "region"
  backup_window                         = "01:50-02:20"
  ca_cert_identifier                    = "rds-ca-rsa2048-g1"
  character_set_name                    = "SQL_Latin1_General_CP1_CI_AS"
  copy_tags_to_snapshot                 = true
  custom_iam_instance_profile           = null
  customer_owned_ip_enabled             = false
  database_insights_mode                = "standard"
  db_subnet_group_name                  = "default-vpc-0a3e3d4724446633a"
  dedicated_log_volume                  = false
  delete_automated_backups              = true
  deletion_protection                   = false
  domain                                = null
  domain_auth_secret_arn                = null
  domain_iam_role_name                  = null
  domain_ou                             = null
  enabled_cloudwatch_logs_exports       = []
  engine                                = "sqlserver-ex"
  engine_version                        = "15.00.4490.9.v1"
  final_snapshot_identifier             = null
  iam_database_authentication_enabled   = false
  identifier                            = "socialmedia-sql-dev"
  instance_class                        = "db.t3.micro"
  iops                                  = 0
  kms_key_id                            = "arn:aws:kms:eu-north-1:661255175203:key/5d6cd55c-0894-47e0-9fc9-dd3ddf4c3808"
  license_model                         = "license-included"
  maintenance_window                    = "sat:05:26-sat:05:56"
  manage_master_user_password           = null
  max_allocated_storage                 = 1000
  monitoring_interval                   = 60
  monitoring_role_arn                   = "arn:aws:iam::661255175203:role/rds-monitoring-role"
  multi_az                              = false
  multi_tenant                          = false
  network_type                          = "IPV4"
  option_group_name                     = "default:sqlserver-ex-15-00"
  parameter_group_name                  = "default.sqlserver-ex-15.0"
  password                              = null # sensitive
  password_wo                           = null # sensitive
  password_wo_version                   = null
  performance_insights_enabled          = true
  performance_insights_kms_key_id       = "arn:aws:kms:eu-north-1:661255175203:key/5d6cd55c-0894-47e0-9fc9-dd3ddf4c3808"
  performance_insights_retention_period = 7
  port                                  = 1433
  publicly_accessible                   = true
  region                                = "eu-north-1"
  replicate_source_db                   = null
  skip_final_snapshot                   = true
  storage_encrypted                     = true
  storage_throughput                    = 0
  storage_type                          = "gp2"
  tags                                  = {}
  tags_all                              = {}
  upgrade_storage_config                = null
  username                              = "admin"
  vpc_security_group_ids                = ["sg-02cc3061c1e085f0f"]
  warning_event_categories              = null
  lifecycle {
    prevent_destroy = true
  }
}

# __generated__ by Terraform
resource "aws_elastic_beanstalk_environment" "socialmedia" {
  application            = "socialmedia1"
  description            = "Environment created from the EB CLI using \"eb create\""
  name                   = "socialmedia-dev"
  platform_arn           = "arn:aws:elasticbeanstalk:eu-north-1::platform/.NET 10 running on 64bit Amazon Linux 2023/3.12.0"
  poll_interval          = null
  region                 = "eu-north-1"
  tags                   = {}
  tags_all               = {}
  template_name          = null
  tier                   = "WebServer"
  version_label          = "Sample Application-2"
  wait_for_ready_timeout = null
}
