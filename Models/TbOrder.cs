using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbOrder
{
    public int OrderId { get; set; }

    public string? Code { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public int? TotalAmount { get; set; }

    public int? BookingStatusId { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual TbBookingStatus? BookingStatus { get; set; }

    public virtual TbDoctor? Doctor { get; set; }

    public virtual TbPatient? Patient { get; set; }

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; } = new List<TbOrderDetail>();
}
