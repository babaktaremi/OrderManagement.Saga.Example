namespace OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

public record OrderInventoryProcessedSagaEvent(Guid OrderId);