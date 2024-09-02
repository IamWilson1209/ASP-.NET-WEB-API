using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Category
{
    public Guid CategoryID { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}
