namespace OrderManagement.Saga.Example.Events.Models;

public record OrderPaidEventModel(Guid OrderId,string UserName);