using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using HelloDocMVC.Models;
//using HelloDocMVC.DataContext;
using DAL_Data_Access_Layer_.DataContext;
using BLL_Business_Logic_Layer_.Interface;
using BLL_Business_Logic_Layer_.Services;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;
using NuGet.Protocol;
using System.Text;
using BusinessLogic.Interfaces;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPatientRequest, PatientRequest>();
builder.Services.AddScoped<ICreateAccount, CreateAccount>();
builder.Services.AddScoped<IConcierge, ConciergeReq>();
builder.Services.AddScoped<IFamilyFriend, FamilyFriend>();
builder.Services.AddScoped<IBusiness, BusinessService>();
builder.Services.AddScoped<IPatientDash, PatientDash>(); 
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAdminDash, AdminDash>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IProviderDash, ProviderDash>();

builder.Services.AddSession();//For Session


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/adminDashboard"))
    {
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        context.Response.Headers.Add("Pragma", "no-cache");
        context.Response.Headers.Add("Expires", "0");
    }

    if (context.Request.Path.StartsWithSegments("/patientDashboard"))
    {
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        context.Response.Headers.Add("Pragma", "no-cache");
        context.Response.Headers.Add("Expires", "0");
    }

    await next.Invoke();
});


app.UseSession();//For Session

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRotativa();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
