using Application.DAO;
using EfcDatabase.Context;
using Microsoft.EntityFrameworkCore;
using Services.IServices;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClockContext>();
builder.Services.AddScoped<IClockDAO, ClockDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IToDoDAO, ToDoDAO>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IClockService, ClockService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();