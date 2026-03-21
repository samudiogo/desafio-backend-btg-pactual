using BtgPactual.Infrastructure.DI;
using BtgPactual.OrdersApi.Workers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<OrderConsumerWorker>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(opt =>
{
    opt.WithTitle("Desafio Engenheiro de software - BTG Pactual")
        .ForceDarkMode()
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithTheme(ScalarTheme.BluePlanet);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
