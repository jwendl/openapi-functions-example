using System;
using System.Collections.Generic;

namespace ComplexOpenApiExample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
