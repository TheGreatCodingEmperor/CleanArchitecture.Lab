using Application.Models;
using Microsoft.EntityFrameworkCore;
using Application.Manager;
using Application.Manager.Interface;
using Application.Repository;
using Application.Repository.Interface;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddDbContext<MyContext>(opt => opt.UseInMemoryDatabase("MyDB"));
builder.Services.AddDbContext<MyContext>(opt => opt.UseSqlite("Data Source=./Database/MyDB.db",  b => b.MigrationsAssembly("Api")));

builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();
builder.Services.AddScoped<ICustomerManager,CustomerManager>();

builder.Services.AddScoped<IInvoiceRepository,InvoiceRepository>();
builder.Services.AddScoped<IInvoiceManager,InvoiceManager>();

builder.Services.AddScoped<IInvoiceItemRepository,InvoiceItemRepository>();
builder.Services.AddScoped<IInvoiceItemManager,InvoiceItemManager>();

builder.Services.AddControllers();

// blazor
// blazor httpclient
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


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

// blazor
app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

// blazor
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
// app.MapControllers();

app.Run();
