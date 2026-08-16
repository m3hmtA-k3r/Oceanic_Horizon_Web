
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Oceanic_Horizon_Travel.Services.BannerServices;
using Oceanic_Horizon_Travel.Services.CategoryServices;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using Oceanic_Horizon_Travel.Services.FileServices;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings))
    );

builder.Services.AddSingleton<IDatabaseSettings>(servideProvider =>
{
    return servideProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddScoped<IMemberServices, MemberServices>();
builder.Services.AddScoped<IBannerServices, BannerServices>();
builder.Services.AddScoped<IDestinationServices, DestinationServices>();
builder.Services.AddScoped<ITourServices, TourServices>();
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => 
    {
        opt.LoginPath = "/Auth/Login";
        opt.LogoutPath = "/Auth/Logout";
        opt.AccessDeniedPath = "/Auth/AccessDenied";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });


// Desteklenen diller — sıra önemli, ilki varsayılan
var supportedCultures = new[] { "tr", "en", "pt" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("tr")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});



// Çeviri dosyalarının Resources klasöründe olduğunu söylüyoruz
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews(options =>
{
    // Nullable açık olduğu için ASP.NET her non-nullable string'e otomatik [Required] etkiliyor bu yüzden bunu yaptım.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.AddViewLocalization()
.AddDataAnnotationsLocalization();


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

app.UseRequestLocalization();
app.UseRouting();

app.UseAuthentication(); // Burası Login için yapılıyor 
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
