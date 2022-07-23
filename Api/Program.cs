using Application.Models;
using Microsoft.EntityFrameworkCore;
using Application.Repository;
using Application.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddDbContext<MyContext>(opt => opt.UseInMemoryDatabase("MyDB"));
builder.Services.AddDbContext<MyContext>(opt => opt.UseSqlite("Data Source=./Database/MyDB.db",  b => b.MigrationsAssembly("Api")));

builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();

builder.Services.AddScoped<IInvoiceRepository,InvoiceRepository>();

builder.Services.AddScoped<IInvoiceItemRepository,InvoiceItemRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
