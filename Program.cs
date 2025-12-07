using Scalar.AspNetCore;

// Creates Web Application Builder
var builder = WebApplication.CreateBuilder(args);

// Adds services to container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Validation services
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Adds Problem Details services
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<Restaurant.Api.Infrastructure.GlobalExceptionHandler>();

// Adds OpenAPI/Swagger services
builder.Services.AddOpenApi();


var app = builder.Build();

// Seeds the database
await DbSeeder.SeedAsync(app);

app.UseExceptionHandler();

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

// GET /menu -> Returns the list of menu items
menuGroup.MapGet("/", Restaurant.Api.Features.Menu.GetMenu.HandleAsync);
// GET /menu/{id} -> Returns specific menu item
menuGroup.MapGet("/{id:int}", Restaurant.Api.Features.Menu.GetMenu.GetByIdAsync);
// POST /menu -> Creates a new menu item (Requires Body)
menuGroup.MapPost("/", Restaurant.Api.Features.Menu.CreateMenuItem.HandleAsync);

// PUT /menu/{id} -> Updates an existing menu item (Requires Body)
menuGroup.MapPut("/{id:int}", Restaurant.Api.Features.Menu.UpdateMenuItem.HandleAsync);
// DELETE /menu/{id} -> Deletes a menu item
menuGroup.MapDelete("/{id:int}", Restaurant.Api.Features.Menu.DeleteMenuItem.HandleAsync);

// Order Endpoints
var orderGroup = app.MapGroup("/orders").WithTags("Orders");

// GET /orders -> Returns the list
orderGroup.MapGet("/", Restaurant.Api.Features.Orders.GetOrders.GetListAsync);

// POST /orders -> Creates a new order (Requires Body)
orderGroup.MapPost("/", Restaurant.Api.Features.Orders.CreateOrder.HandleAsync);

// GET /orders/{id} -> Returns specific order
orderGroup.MapGet("/{id:int}", Restaurant.Api.Features.Orders.GetOrders.GetByIdAsync);

// Report Endpoints
var reportGroup = app.MapGroup("/reports").WithTags("Reports");

// GET /reports/daily?date=YYYY-MM-DD -> Returns daily report for specified date
reportGroup.MapGet("/daily", Restaurant.Api.Features.Reports.DailyReport.HandleAsync);

// Runs the application
app.Run();
