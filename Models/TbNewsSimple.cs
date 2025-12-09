using System;
using System.Collections.Generic;

namespace Doan.Models;

public partial class TbNewsSimple
{
    public int NewsSimpleId { get; set; }

    public string? Title { get; set; }

    public string? Image { get; set; }

    public string? Detail { get; set; }

    public DateTime? CreatedDate { get; set; }
}
