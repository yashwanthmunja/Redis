using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Redis.IRepository;
using Redis;
using Redis.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 builder.Services.AddScoped<ICacheService, CacheService>();
 builder.Services.AddDbContext<StudentdbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
   


    var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
