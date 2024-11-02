using fiap_grupo58_fase1.Configurations;
using fiap_grupo58_fase1.Infrastructures.Data;
using fiap_grupo58_fase1.Infrastructures.Excpetion;
using fiap_grupo58_fase1.Interfaces.Dapper;
using fiap_grupo58_fase1.Interfaces.Repositories;
using fiap_grupo58_fase1.Interfaces.Services;
using fiap_grupo58_fase1.Repositories;
using fiap_grupo58_fase1.Services;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços e repositórios
builder.Services.AddTransient<IDbConnection>((sp) =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IContatosRepository, ContatosRepository>();
builder.Services.AddScoped<IContatosService, ContatosService>();
builder.Services.AddScoped<IDapperWrapper, DapperWrapper>();

// Configuração do Entity Framework DbContext
builder.Services.AddDbContext<ContatoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23))));

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
