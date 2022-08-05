using Application.Models;
using Microsoft.EntityFrameworkCore;
using Application.Repository;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using CleanArchitecture.Api;
using Masstransit.Test.Components.Contracts.Browse;
using Masstransit.Test.Components.Models.Exceptions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyContext>(opt => opt.UseInMemoryDatabase("MyDB"));
// builder.Services.AddDbContext<MyContext>(opt => opt.UseSqlite("Data Source=./Database/MyDB.db",  b => b.MigrationsAssembly("CleanArchitecture.Api")));

builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();

builder.Services.AddScoped<IInvoiceRepository,InvoiceRepository>();

builder.Services.AddScoped<IInvoiceItemRepository,InvoiceItemRepository>();
// rabbitMQ ��ھާ@�A��
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(cfg =>
{

    cfg.AddRequestClient<BrowseCurrentData>();
    cfg.AddConsumer<BrowseCurrentDataConsumer>();

    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("/test", async (IRequestClient<BrowseCurrentData> client,[FromBody] BrowseCurrentData query) =>
{
    var (success,failed) = await client.GetResponse<BrowseCurrentDataSuccess,BadRequestViewModel>(query);
        if(success.IsCompletedSuccessfully){
            return Results.Accepted(null,success.Result.Message);
        }
        if(failed.Result.Message.Status == 400){
            return Results.BadRequest($"{failed.Result.Message.Title}: {failed.Result.Message.Message}");
        }
        else{
            
        }
        return Results.BadRequest($"{failed.Result.Message.Title}: {failed.Result.Message.Message}");
});

app.MapControllers();

app.Run();
