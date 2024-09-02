using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class DailyExpense
{
    public Guid ID { get; set; }

    public DateTime RecordDateTime { get; set; }

    public string Category { get; set; } = null!;

    public string Item { get; set; } = null!;

    public int Cost { get; set; }

    public string Bank { get; set; } = null!;

    public DateTime UpdateDateTime { get; set; }

    public string? Remark { get; set; }

    public virtual ICollection<UploadFile> UploadFile { get; set; } = new List<UploadFile>();
}
