using Bogus;
using ComplexOpenApiExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplexOpenApiExample.Services
{
    public interface IOrderService
    {
        Task<Customer> CreateCustomerAsync(Customer customer);

        Task<IEnumerable<Customer>> FetchCustomersAsync();
    }

    public class OrderService
        : IOrderService
    {
        private readonly IList<Customer> customers;

        public OrderService()
        {
            var orderItemIds = 0;
            var orderItems = new Faker<OrderItem>()
                .RuleFor(oi => oi.Id, f => orderItemIds++)
                .RuleFor(oi => oi.Title, v => v.Commerce.ProductName())
                .RuleFor(oi => oi.Description, v => v.Commerce.ProductMaterial())
                .RuleFor(oi => oi.Price, v => decimal.Parse(v.Commerce.Price()))
                .RuleFor(oi => oi.Quantity, v => v.Random.Int(1, 10))
                .Generate(10000);

            orderItems.ForEach(oi => oi.TotalPrice = (oi.Price * oi.Quantity));

            var orderIds = 0;
            var orders = new Faker<Order>()
                .RuleFor(o => o.Id, f => orderIds++)
                .RuleFor(o => o.OrderDate, v => v.Date.Between(new DateTime(2010, 1, 1), DateTime.Now.AddYears(18)))
                .RuleFor(o => o.OrderItems, v => v.PickRandom(orderItems, 100))
                .Generate(1000);

            var customerIds = 0;
            customers = new Faker<Customer>()
                .RuleFor(c => c.Id, f => customerIds++)
                .RuleFor(c => c.FirstName, v => v.Name.FirstName())
                .RuleFor(c => c.LastName, v => v.Name.LastName())
                .RuleFor(c => c.BirthDate, v => v.Date.Between(new DateTime(1945, 1, 1), DateTime.Now.AddYears(18)))
                .RuleFor(c => c.Orders, v => v.PickRandom(orders, 10))
                .Generate(100);
        }

        public async Task<IEnumerable<Customer>> FetchCustomersAsync()
        {
            return await Task.FromResult(customers);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var lastCustomerId = customers.OrderBy(c => c.Id)
                .Last().Id;
            var lastOrderId = customers.SelectMany(c => c.Orders)
                .OrderBy(o => o.Id)
                .Last().Id;
            var lastOrderItemId = customers.SelectMany(c => c.Orders)
                .SelectMany(o => o.OrderItems)
                .OrderBy(oi => oi.Id)
                .Last().Id;

            customer.Id = lastCustomerId++;
            foreach (var order in customer.Orders)
            {
                order.Id = lastOrderId++;
                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Id = lastOrderItemId++;
                }
            }

            customers.Add(customer);
            return await Task.FromResult(customer);
        }
    }
}
