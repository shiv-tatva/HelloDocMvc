using Microsoft.EntityFrameworkCore;
using HelloDocMVC.Models;
//using HelloDocMVC.DataContext;
using DAL_Data_Access_Layer_.DataContext;
using BLL_Business_Logic_Layer_.Interface;
using BLL_Business_Logic_Layer_.Services;
using HelloDocMvc.CustomeModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPatientRequest, PatientRequest>();
builder.Services.AddScoped<ICreateAccount, CreateAccount>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
