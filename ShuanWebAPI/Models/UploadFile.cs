using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class UploadFile
{
    public Guid UploadFileID { get; set; }

    public string Name { get; set; } = null!;

    public string Src { get; set; } = null!;

    public Guid ID { get; set; }

    public virtual DailyExpense IDNavigation { get; set; } = null!;
}
