using Carter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Saga.Example.Database.DbContexts;
using OrderManagement.Saga.Example.Events.Handlers;
using OrderManagement.Saga.Example.Sagas.OrderSaga;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopDb"));
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<OrderCompletedEventHandler>();
    busConfigurator.AddConsumer<OrderPaidEventHandler>();
    busConfigurator.AddConsumer<OrderProductInventoryProcessedHandler>();

    busConfigurator.AddSagaStateMachine<OrderSagaStateMachine, OrderSagaStateMachineInstance>()
        .EntityFrameworkRepository(repositoryConfig =>
        {
            repositoryConfig.ExistingDbContext<ShopDbContext>();
            repositoryConfig.UseSqlServer();
        });

    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["Masstransit:RabbitMqEndpoint"]!), hst =>
        {
            hst.Username("guest");
            hst.Password("guest");
        });
        
        cfg.UseInMemoryOutbox(context);
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCarter();
app.Run();
