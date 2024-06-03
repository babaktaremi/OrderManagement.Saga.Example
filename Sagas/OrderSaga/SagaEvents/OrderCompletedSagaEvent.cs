namespace OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

public record OrderCompletedSagaEvent(Guid OrderId);