using System;
using System.Collections.Generic;

namespace ShuanWebAPI.Models;

public partial class Todo
{
    public bool? Status { get; set; }

    public string? Thing { get; set; }

    public bool? Editing { get; set; }

    public Guid TodoId { get; set; }

    public bool? CanEdit { get; set; }

    public int? Seqno { get; set; }

    public DateTime CreateTime { get; set; }

    public string? AccountId { get; set; }

    public string? GroupId { get; set; }
}
