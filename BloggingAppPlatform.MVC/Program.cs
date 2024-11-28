using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Business.Dependency.Autofac;
using Core.DependencyResolve;
using Core.Extensions;
using Core.Helpers.IoC;
using Core.Helpers.Security.Encryption;
using Core.Helpers.Security.JWT;
using Entities.Mappings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// Register AutoMapper
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Set Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<AutofacBusinessModule>();
});

// Configure session
builder.Services.AddDistributedMemoryCache(); // Required for session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the cookie HttpOnly
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// Token configuration
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

// Add cookie authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Auth/Login"; // Path to redirect when not authenticated
    options.LogoutPath = "/Auth/Logout"; // Path to redirect on logout
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expiration
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        RequireExpirationTime = true,
        ValidateLifetime = true // Validate token expiration
    };
});



// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrModerator", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Moderator"))));
});


// Dependency injection for core services
ServiceTool.Create(builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>());
builder.Services.AddDependencyResolvers(new[] { new CoreModule() });

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>(); // Adjust the namespace and profile name
}, Assembly.GetExecutingAssembly(), typeof(MappingProfile).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware before authentication
app.UseSession();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

// Map controller routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "userProfile",
    pattern: "UserProfile/{userId:int}",
    defaults: new { controller = "Profile", action = "GetProfile" }
);

// Login and Logout routes
app.MapControllerRoute(
    name: "login",
    pattern: "Auth/Login",
    defaults: new { controller = "Auth", action = "Login" }
);

app.MapControllerRoute(
    name: "logout",
    pattern: "Auth/Logout",
    defaults: new { controller = "Auth", action = "Logout" }
);

app.Run();
