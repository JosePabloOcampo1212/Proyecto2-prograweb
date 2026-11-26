using GestionProyectos.Web.Filtros;
using GestionProyectos.Web.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new SesionRequeridaAttribute());
    opciones.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opciones =>
{
    opciones.IdleTimeout = TimeSpan.FromMinutes(60);
    opciones.Cookie.HttpOnly = true;
    opciones.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();
builder.Services
    .AddHttpClient<ApiClient>(cliente => cliente.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!))
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddScoped<AuthApi>();
builder.Services.AddScoped<UsuariosApi>();
builder.Services.AddScoped<DashboardApi>();
builder.Services.AddScoped<CatalogosApi>();

var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
