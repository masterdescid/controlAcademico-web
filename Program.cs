using controlAcademico_web.Services;

var builder = WebApplication.CreateBuilder(args);

// Registro de IHttpContextAccessor para que est� disponible en la aplicaci�n
builder.Services.AddHttpContextAccessor();

// Registro de servicios personalizados
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IMaestroService, MaestroService>();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();

// Configuraci�n de controladores
builder.Services.AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Habilitar sesiones
builder.Services.AddDistributedMemoryCache(); // Para almacenar la sesi�n en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // Solo accesible por el servidor
    options.Cookie.IsEssential = true; // Necesario para que la sesi�n funcione
});

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

// Middleware de autenticaci�n y autorizaci�n
app.UseAuthorization();

// Habilitar sesiones
app.UseSession();

// Configuraci�n de las rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
