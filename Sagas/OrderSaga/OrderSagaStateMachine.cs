using MassTransit;
using OrderManagement.Saga.Example.Events.Models;
using OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

namespace OrderManagement.Saga.Example.Sagas.OrderSaga;

public class OrderSagaStateMachine:MassTransitStateMachine<OrderSagaStateMachineInstance>
{
    public State OrderPaid { get; set; }
    public State OrderInventoryProcessed { get; set; }
    
    public Event<OrderPaidSagaEvent> OrderPaidEvent { get; set; }
    public Event<OrderInventoryProcessedSagaEvent> InventoryProcessedEvent { get; set; }
    public Event<OrderCompletedSagaEvent> OrderCompletedEvent { get; set; }

    public OrderSagaStateMachine(ILogger<OrderSagaStateMachine> logger)
    {
        InstanceState(x=>x.CurrentState);

        Event(() => OrderPaidEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => InventoryProcessedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderCompletedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        
        
        Initially(When(OrderPaidEvent)
            .Then(context =>
            {
                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.UserName = context.Message.UserName;
                context.Saga.IsOrderPaid = true;
            })
            .TransitionTo(OrderPaid)
            .Publish(context=>new OrderProductInventoryProcessedEventModel(context.Message.OrderId)));
        
        During(OrderPaid,
            When(InventoryProcessedEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.IsOrderInventoryProcessed = true;
                })
                .TransitionTo(OrderInventoryProcessed)
                .Publish(context=>new OrderCompletedEventModel(context.Message.OrderId)));
        
        During(OrderInventoryProcessed,
            When(OrderCompletedEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.IsOrderCompleted = true;
                })
                .Finalize()
               );
        
            
    }
}