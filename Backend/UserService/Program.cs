using EfcDatabase.DAOImplementation;
using Microsoft.EntityFrameworkCore;
using UserService.Authorization;
using UserService.Context;
using UserService.DAOImplementation;
using UserService.IDAO;
using UserService.IServices;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IClockDAO, ClockDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IMessageDao, MessageDAO>();
builder.Services.AddScoped<IToDoDAO, ToDoDAO>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowBlazorOrigin",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

        });

});


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