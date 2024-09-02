using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShuanWebAPI.Dtos
{
    public class TodoDtos
    {
        public Guid TodoId { get; set; }
        public bool? Status { get; set; }
        public string Thing { get; set; }
        public bool? Editing { get; set; }
        public bool? CanEdit { get; set; }
        public int Seqno { get; set; }
        public DateTime? CreatTime { get; set; }

    }
}
