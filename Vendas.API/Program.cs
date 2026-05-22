using Vendas.API.Endpoints.Pedidos;
using Vendas.Infrastructure.Extensions;
using Vendas.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddFakeIntegration();

var app = builder.Build();

app.MapPedidosEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope()) // Em produção, esta abordagem de criar o BD ao iniciar a aplicação não é recomendada. O ideal é usar migrações e um processo de implantação adequado.
    {
        var db = scope.ServiceProvider.GetRequiredService<VendasDbContext>();
        db.Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
