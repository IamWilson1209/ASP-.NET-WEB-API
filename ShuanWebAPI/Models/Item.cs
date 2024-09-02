using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Item
{
    public Guid ItemID { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }
}
