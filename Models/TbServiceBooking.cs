using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbServiceBooking
{
    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public string PatientName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TbService Service { get; set; } = null!;
}
