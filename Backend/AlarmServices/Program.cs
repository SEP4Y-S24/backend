using AlarmServices.Context;
using AlarmServices.DAOImplementation;
using AlarmServices.IDAO;
using AlarmServices.IService;
using AlarmServices.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClockContext>();
builder.Services.AddScoped<IClockDAO, ClockDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<IMessageDAO, MessageDAO>();
builder.Services.AddScoped<IAlarmDAO, AlarmDAO>();
builder.Services.AddScoped<IAlarmService, AlarmService>();

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