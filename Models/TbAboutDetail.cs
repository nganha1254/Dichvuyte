using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbAboutDetail
{
    public int AboutDetailsId { get; set; }

    public int AboutId { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? MapLink { get; set; }

    public string? SocialLinks { get; set; }

    public string? ContactFormText { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual TbAbout About { get; set; } = null!;
}
