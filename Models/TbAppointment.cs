using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbAppointment
{
    public int AppointmentId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public string? Message { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? DoctorId { get; set; }

    public string? DoctorName { get; set; }

    public int? AccountId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public int? Price { get; set; }

    public bool? PaymentStatus { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual TbCategory? Category { get; set; }

    public virtual TbDoctor? Doctor { get; set; }
}
