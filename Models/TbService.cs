using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbService
{
    public int ServiceId { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? Icon { get; set; }

    public string? Image { get; set; }

    public string? ShortDescription { get; set; }

    public string? Detail { get; set; }

    public int? DoctorId { get; set; }

    public int? CategoryId { get; set; }

    public int? Position { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? SeoKeywords { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsNew { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsActive { get; set; }

    public int? Price { get; set; }

    public int? PriceSale { get; set; }

    public virtual TbCategory? Category { get; set; }

    public virtual TbDoctor? Doctor { get; set; }

    public virtual ICollection<TbCartItem> TbCartItems { get; set; } = new List<TbCartItem>();

    public virtual ICollection<TbOrderItem> TbOrderItems { get; set; } = new List<TbOrderItem>();

    public virtual ICollection<TbServiceBooking> TbServiceBookings { get; set; } = new List<TbServiceBooking>();
}
