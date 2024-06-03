namespace OrderManagement.Saga.Example.Sagas.OrderSaga.SagaEvents;

public record OrderPaidSagaEvent(Guid OrderId,string UserName);