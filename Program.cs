using Scalar.AspNetCore;

// Creates Web Application Builder
var builder = WebApplication.CreateBuilder(args);

// Adds services to container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Validation services
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Adds OpenAPI/Swagger services
builder.Services.AddOpenApi();


var app = builder.Build();

// Seeds the database
await DbSeeder.SeedAsync(app);

// Configures the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enables OpenAPI/Swagger in development
    app.MapOpenApi();
    // Enables Scalar API reference in development
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

// Menu Endpoints
var menuGroup = app.MapGroup("/menu").WithTags("Menu");

menuGroup.MapGet("/", Restaurant.Api.Features.Menu.GetMenu.HandleAsync);
menuGroup.MapPost("/", Restaurant.Api.Features.Menu.CreateMenuItem.HandleAsync);

menuGroup.MapPut("/{id:int}", Restaurant.Api.Features.Menu.UpdateMenuItem.HandleAsync);
menuGroup.MapDelete("/{id:int}", Restaurant.Api.Features.Menu.DeleteMenuItem.HandleAsync);

// Runs the application
app.Run();
