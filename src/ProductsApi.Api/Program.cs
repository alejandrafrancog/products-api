using System.Text.Json.Serialization;
using ProductsApi.Api.Endpoints;
using ProductsApi.Api.ExceptionHandling;
using ProductsApi.Application;
using ProductsApi.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Composition Root: Application (mediator, handlers, validadores) + Infrastructure (EF Core + PostgreSQL)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapProductsEndpoints();

app.Run();
