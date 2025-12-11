using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbAccount
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    public virtual TbRole? Role { get; set; }

    public virtual ICollection<TbAppointment> TbAppointments { get; set; } = new List<TbAppointment>();

    public virtual ICollection<TbBlog> TbBlogs { get; set; } = new List<TbBlog>();

    public virtual ICollection<TbCart> TbCarts { get; set; } = new List<TbCart>();

    public virtual ICollection<TbDoctor> TbDoctors { get; set; } = new List<TbDoctor>();

    public virtual ICollection<TbOrder> TbOrders { get; set; } = new List<TbOrder>();
}
