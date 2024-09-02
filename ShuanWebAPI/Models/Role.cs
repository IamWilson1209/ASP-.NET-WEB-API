using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Role
{
    public Guid RoleId { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;
}
