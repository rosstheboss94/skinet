using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(MapperProfiles));
builder.Services.AddCors(o =>
{
    o.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

StoreContextSeed.Seed(app);

app.Run();
