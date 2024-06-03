using MassTransit;

namespace OrderManagement.Saga.Example.Sagas.OrderSaga;

public class OrderSagaStateMachineInstance:SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid OrderId { get; set; }
    public string UserName { get; set; }
    
    public bool IsOrderPaid { get; set; }
    public bool IsOrderInventoryProcessed { get; set; }
    public bool IsOrderCompleted { get; set; }
}