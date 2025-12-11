using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbOrder
{
    public int OrderId { get; set; }

    public int AccountId { get; set; }

    public int TotalAmount { get; set; }

    public bool PaymentStatus { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TbAccount Account { get; set; } = null!;

    public virtual ICollection<TbOrderItem> TbOrderItems { get; set; } = new List<TbOrderItem>();
}
