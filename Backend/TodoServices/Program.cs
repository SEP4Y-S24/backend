using ClockServices.Context;
using Microsoft.EntityFrameworkCore;
using TodoServices.DAOImplementation;
using TodoServices.IDAO;
using TodoServices.IServices;
using TodoServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<ClockContext>(options=>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDbContext<ToDoContext>();
builder.Services.AddScoped<IClockDao, ClockDAO>();
builder.Services.AddScoped<IUserDao, UserDAO>();
builder.Services.AddScoped<ITodoDao, TodoDAO>();
builder.Services.AddScoped<IMessageDao, MessageDAO>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITagDao, TagDao>();

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