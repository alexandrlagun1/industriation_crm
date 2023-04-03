using industriation_crm.Server.Interfaces;
using industriation_crm.Server.Middleware;
using industriation_crm.Server.Models;
using industriation_crm.Server.Services;
using industriation_crm.Server.SignalRNotification;
using industriation_crm.Shared.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>
    (options =>
    {
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))/*.LogTo(Console.WriteLine, LogLevel.Information)*/;
        //options.ConfigureWarnings(warnings =>warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    });

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<ICategory, CategoryManager>();
builder.Services.AddScoped<IContact, ContactManager>();
builder.Services.AddScoped<IProgressClientDiscount, ProgressClientDiscountManager>();
builder.Services.AddScoped<IUser, UserManager>();
builder.Services.AddScoped<IClient, ClientManager>();
builder.Services.AddScoped<IRoles, RolesManager>();
builder.Services.AddScoped<IProduct, ProductManager>();
builder.Services.AddScoped<IOrder, OrderManager>();
builder.Services.AddScoped<IProductToOrder, ProductToOrderManager>();
builder.Services.AddScoped<ISupplierOrder, SupplierOrderManager>();
builder.Services.AddScoped<IDeliveryType, DeliveryTypeManager>();
builder.Services.AddScoped<IDeliveryPeriodType, DeliveryPeriodTypeManager>();
builder.Services.AddScoped<IOrderHistory, OrderHistoryManager>();
builder.Services.AddScoped<IUserNotifications, UserNotificationsManager>();
builder.Services.AddScoped<ITask, TaskManager>();
builder.Services.AddScoped<ICallHistory, CallHistoryManager>();
builder.Services.AddScoped<IOrderStatus, OrderStatusManager>();
builder.Services.AddScoped<IPayStatus, PayStatusManager>();
builder.Services.AddScoped<IOrderCheck, OrderCheckManager>();

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRazorPages();

builder.Services.AddLocalization();
//Авторизация
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsSpecs",
    builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(options => true)
            .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//app.UseRequestLocalization(new RequestLocalizationOptions()
//    .AddSupportedCultures(new[] { "ru-RU" })
//    .AddSupportedUICultures(new[] { "ru-RU" }));

//Авторизация
app.UseCors("CorsSpecs");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapHub<StatusNotificationHub>("/statushub");
app.MapFallbackToFile("index.html");

app.Run();
