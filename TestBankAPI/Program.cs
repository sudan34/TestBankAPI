using Microsoft.EntityFrameworkCore;
using TestBankAPI.DAL;
using TestBankAPI.Services.Implementations;
using TestBankAPI.Services.Interface;
using TestBankAPI.Utils;

var builder = WebApplication.CreateBuilder(args);
var settings = new AppSettings();
var section = builder.Configuration.GetSection("AppSettings");
section.Bind(settings);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton(settings);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    x =>
    {
        x.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Test Bank API Doc",
            Version = "v2",
            Description = "This is a Test Bank API"
            //Contact = new Microsoft.OpenApi.Models.OpenApiContact
            //{
            //    Name = "Madhusudan Bhattarai",
            //    Email = "summerpalpali@gmail.com",
            //    Url = new Uri("https://google.com")
            //}
        });

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        var prefix = string.IsNullOrEmpty(x.RoutePrefix) ? "." : "..";
        x.SwaggerEndpoint($"{prefix}/swagger/v2/swagger.json", "Bank API Doc");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
