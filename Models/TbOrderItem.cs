using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbOrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public string? CategoryName { get; set; }

    public string? DoctorName { get; set; }

    public virtual TbOrder Order { get; set; } = null!;

    public virtual TbService Service { get; set; } = null!;
}
