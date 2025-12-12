using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbDoctor
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? CategoryId { get; set; }

    public string? Position { get; set; }

    public string? Qualification { get; set; }

    public int? ExperienceYears { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? AccountId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual TbCategory? Category { get; set; }

    public virtual ICollection<TbDepartment> TbDepartments { get; set; } = new List<TbDepartment>();

    public virtual ICollection<TbProductReview> TbProductReviews { get; set; } = new List<TbProductReview>();

    public virtual ICollection<TbProduct> TbProducts { get; set; } = new List<TbProduct>();

    public virtual ICollection<TbService> TbServices { get; set; } = new List<TbService>();
}
