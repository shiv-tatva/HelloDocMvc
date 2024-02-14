using Microsoft.EntityFrameworkCore;
using HelloDocMVC.Models;
//using HelloDocMVC.DataContext;
using DAL_Data_Access_Layer_.DataContext;
using BLL_Business_Logic_Layer_.Interface;
using BLL_Business_Logic_Layer_.Services;
using HelloDocMVC.CustomeModel;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPatientRequest, PatientRequest>();
builder.Services.AddScoped<ICreateAccount, CreateAccount>();
builder.Services.AddScoped<IConcierge, ConciergeReq>();
builder.Services.AddScoped<IFamilyFriend, FamilyFriend>();
builder.Services.AddScoped<IBusiness, BusinessService>();
builder.Services.AddScoped<IPatientDash, PatientDash>();

builder.Services.AddSession();//For Session


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();//For Session

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
