using Confluent.Kafka;
using Gvz.Laboratory.ManufacturerService;
using Gvz.Laboratory.ManufacturerService.Abstractions;
using Gvz.Laboratory.ManufacturerService.Kafka;
using Gvz.Laboratory.ManufacturerService.Middleware;
using Gvz.Laboratory.ManufacturerService.Repositories;
using Gvz.Laboratory.ManufacturerService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GvzLaboratoryManufacturerServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

var config = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(config).Build());
builder.Services.AddScoped<IManufacturerKafkaProducer, ManufacturerKafkaProducer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();