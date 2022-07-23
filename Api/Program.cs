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

builder.Services.AddSpaStaticFiles (configuration => {
    configuration.RootPath = "ClientApp/dist";
});

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

app.UseSpa (spa => {
    // To learn more about options for serving an Angular SPA from ASP.NET Core,
    // see https://go.microsoft.com/fwlink/?linkid=864501

    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment ()) {
        // spa.UseAngularCliServer (npmScript: "start"); // IE 11
        // spa.UseAngularCliServer (npmScript: "start-es6");
        // spa.Options.StartupTimeout = TimeSpan.FromSeconds (120); // Increase the timeout if angular app is taking longer to startup

        spa.UseProxyToSpaDevelopmentServer ("http://localhost:4200"); // Use this instead to use the angular cli server
    }
});


app.Run();
