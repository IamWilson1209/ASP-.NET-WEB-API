using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Bank
{
    public Guid BankID { get; set; }

    public string BankName { get; set; } = null!;

    public string? Description { get; set; }
}
