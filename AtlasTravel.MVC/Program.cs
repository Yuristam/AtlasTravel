using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");

builder.Services.AddScoped<IUsersRepository>(provider => new UsersRepository(connectionString));
builder.Services.AddScoped<IAdminRepository>(provider => new AdminRepository(connectionString));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MyAuthCookie"; // AtlasAuthCookie
        options.LoginPath = "/Auth/SignIn"; // путь при попытке доступа без авторизации
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}*/

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.Run();
