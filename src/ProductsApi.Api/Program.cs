using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApiVersioning();

var app = builder.Build();

// Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();