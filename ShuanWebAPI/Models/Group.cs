using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Group
{
    public Guid GroupId { get; set; }

    public string Name { get; set; } = null!;
}
