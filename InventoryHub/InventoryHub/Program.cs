using InventoryHub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add framework services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Register application services
builder.Services.AddScoped<IInventoryService, InventoryService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<InventoryHub.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();