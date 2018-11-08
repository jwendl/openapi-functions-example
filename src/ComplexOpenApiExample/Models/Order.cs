using System;
using System.Collections.Generic;

namespace ComplexOpenApiExample.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
