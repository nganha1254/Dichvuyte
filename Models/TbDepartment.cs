using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbDepartment
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Alias { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? Position { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? SeoKeywords { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public int? CategoryId { get; set; }

    public int? DoctorId { get; set; }

    public bool IsNew { get; set; }

    public virtual TbCategory? Category { get; set; }

    public virtual TbDoctor? Doctor { get; set; }
}
