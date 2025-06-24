using System;
using System.Collections.Generic;

namespace AdForm_SQL_API.AdFormDB;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductCategory { get; set; }

    public float Price { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
