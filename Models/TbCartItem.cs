using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbCartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public string? CategoryName { get; set; }

    public string? DoctorName { get; set; }

    public virtual TbCart Cart { get; set; } = null!;

    public virtual TbService Service { get; set; } = null!;
}
