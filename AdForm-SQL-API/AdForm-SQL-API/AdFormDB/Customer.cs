using System;
using System.Collections.Generic;

namespace AdForm_SQL_API.AdFormDB;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
