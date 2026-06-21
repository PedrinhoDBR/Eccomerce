using Ecommerce.Application.Contracts;
using Ecommerce.Application.Services;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Security;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(_ => new LiteDatabase("meubanco.db"));
builder.Services.AddSingleton<IProductRepository, LiteDbProductRepository>();
builder.Services.AddSingleton<IOrderRepository, LiteDbOrderRepository>();
builder.Services.AddSingleton<AdminAuthService>();
builder.Services.AddScoped<AdminAuthorizationFilter>();
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<CheckoutService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "http://127.0.0.1:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularClient");
app.MapControllers();

app.Run();
