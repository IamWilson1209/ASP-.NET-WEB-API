using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShuanWebAPI.Dtos
{
    public class DailyExpenseDtos
    {
        public Guid ID { get; set; }
        public DateTime RecordDateTime { get; set; }
        public string Category { get; set; }
        public string Item  { get; set; }
        public int Cost  { get; set; }
        public string Bank  { get; set; }
        public ICollection<UploadFileDto> UploadFile { get; set; }
    }
}
