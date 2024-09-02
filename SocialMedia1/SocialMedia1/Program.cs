using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia1.Data;
using SocialMedia1.Hubs;
using SocialMedia1.Services;
using SocialMedia1.Services.Common.MailSender;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// var apiKey = builder.Configuration.GetValue<string>("SendGrid");

//builder.Services.AddTransient<IEmailSender>(x => new SendGridEmailSender("apiKey")); // change with your SendGrid API key
builder.Services.AddTransient<IEmailSender>(x => new SmtpEmailSender("<{smpt>}", "*******")); 

builder.Services
    .Scan(builder => builder
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(a => a.AssignableTo(typeof(IService)))
    .AsMatchingInterface()
    .WithTransientLifetime());

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

    db.Database.Migrate();
}

app.Run();
