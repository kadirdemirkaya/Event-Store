using EventSourching.API;
using EventSourching.Application;
using EventSourching.Persistence;
using EventStore.ClientAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.ApiServiceRegistration(builder.Configuration);

builder.Services.PersistenceServiceRegistration();

builder.Services.ApplicationServiceRegistration(builder.Services.BuildServiceProvider());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



