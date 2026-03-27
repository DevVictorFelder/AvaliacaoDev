using AvaliacaoDev.Application.Interfaces;
using AvaliacaoDev.Application.Services;
using AvaliacaoDev.Domain.Interfaces;
using AvaliacaoDev.Infrastructure.Data;
using AvaliacaoDev.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using AvaliacaoDev.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados em memória para simplificação
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("PedidosDb"));

// Configuração da injeção de dependência (DI)
// Repositories (AvaliacaoDev.Infrastructure)
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Services (AvaliacaoDev.Application)
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Configuração de serviços da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger com documentação básica
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "API de Gerenciamento de Pedidos.", 
        Version = "v1",
        Description = "API RESTful para gerenciamento de pedidos."
    });
});

var app = builder.Build();

// Configuração de middleware de logging para debug
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERRO: {ex.Message}");
        Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
        throw;
    }
});

// Configuração do middleware Swagger para documentação da API
app.UseSwagger();
app.UseSwaggerUI();

// Mapeamento dos controllers
app.MapControllers();

// Inicialização da aplicação
app.Run();