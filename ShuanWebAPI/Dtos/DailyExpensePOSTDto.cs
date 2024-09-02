using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ShuanWebAPI.Dtos
{
    public class DailyExpensePOSTDto // 新增資料專用Dto
    {
        
        public Guid ID { get; set; }

        public DateTime RecordDateTime { get; set; }

        [Required(ErrorMessage ="Category不得為空")] // 驗證資料欄位不為空
        public string Category { get; set; }
        
        [Required(ErrorMessage ="Item不得為空")] // 驗證資料欄位不為空
        public string Item { get; set; }
        
        [Range(-99999, 99999)] // 驗證資料欄位數值範圍
        public int Cost { get; set; }

        [Required(ErrorMessage = "Bank不得為空")] // 驗證資料欄位不為空
        public string Bank { get; set; }
        public string? Remark { get; set; }

        public ICollection<UploadFileDto>? UploadFile { get; set; }
    }
}
