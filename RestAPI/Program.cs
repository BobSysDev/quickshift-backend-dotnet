using Entities;
using GrpcClient;
using InMemoryRepositories;
using RepositoryContracts;
using RepositoryProxies;
using RestAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IShiftRepository, ShiftRepositoryProxy>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepositoryProxy>();
builder.Services.AddSingleton<IShiftSwitchRepository, ShiftSwitchRepositoryProxy>();
builder.Services.AddSingleton<IAnnouncementRepository, AnnouncementRepositoryProxy>();



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