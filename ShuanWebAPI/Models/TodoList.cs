using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class TodoList
{
    public Guid TodoId { get; set; }

    public string Status { get; set; } = null!;

    public string Thing { get; set; } = null!;

    public bool Editing { get; set; }

    public bool CanEdit { get; set; }

    public int Seqno { get; set; }

    public DateTime CreateTime { get; set; }

    public string? AccountId { get; set; }

    public string? GroupId { get; set; }
}
