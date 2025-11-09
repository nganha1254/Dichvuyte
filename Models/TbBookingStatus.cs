using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbBookingStatus
{
    public int BookingStatusId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbOrder> TbOrders { get; set; } = new List<TbOrder>();
}
