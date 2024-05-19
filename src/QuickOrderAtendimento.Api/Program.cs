using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using QuickOrderAtendimento.Api.Configurations;
using QuickOrderAtendimento.Api.Controllers;
using QuickOrderAtendimento.Infra.Gateway.Core;
using QuickOrderAtendimento.Infra.MQ;
using QuickOrderAtendimento.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<DatabaseMongoDBSettings>(
    builder.Configuration.GetSection("DatabaseMongoDBSettings")
);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMqSettings")
);

builder.Services.AddSingleton<IMongoDatabase>(options =>
{
    DatabaseMongoDBSettings settings = new DatabaseMongoDBSettings();
    string mongo = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_MONGODB");

    if (!string.IsNullOrEmpty(mongo))
    {
        settings.DatabaseName = "quickorderdb-atendimento";
        settings.ConnectionString = mongo;
    }
    else
        settings = builder.Configuration.GetSection("DatabaseMongoDBSettings").Get<DatabaseMongoDBSettings>();

    var client = new MongoClient(settings.ConnectionString);

    return client.GetDatabase(settings.DatabaseName);
});

//===================================================================================================

builder.Services.AddDependencyInjectionConfiguration();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithOrigins("http://localhost:8090");
                      });
});


builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuickOrderAtendimento.Api", Version = "v1" });
});

builder.Services.ConfigureHealthChecks(builder.Configuration.GetSection("DatabaseMongoDBSettings").Get<DatabaseMongoDBSettings>());

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseReDoc(c =>
{
    c.DocumentTitle = "QuickOrder Atendimento API Documentation";
    c.SpecUrl = "/swagger/v1/swagger.json";
});

app.RegisterAtendimentoController();
app.RegisterHealthController();

app.ConfigureHealthCheckEndpoints();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.Run();
