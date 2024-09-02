using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuanWebAPI.Parameters;
using ShuanWebAPI.Dtos;
using ShuanWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShuanWebAPI.Controllers
{
    // 專門上傳ID來上傳單一一筆的
    [Route("api/[controller]")]
    [ApiController]
    public class DailyExpenseUploadFileController : ControllerBase
    {

        // 先在全域宣告資料庫物件
        private readonly AccountBookContext _AccountBookContext;

        // 依賴注入使用剛設定好的資料庫物件
        public DailyExpenseUploadFileController(AccountBookContext webContext)
        {
            _AccountBookContext = webContext;
        }

        // GET: api/<DailyExpenseUploadFileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DailyExpenseUploadFileController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExpenseController>
        // 5-2 從父資料ID新增一筆子資料
        [HttpPost("{id}")]
        public string Post(Guid ID, [FromBody] UplaodFilePOSTDto value)
        {
            if (!_AccountBookContext.DailyExpense.Any(a => a.ID == ID))
            {
                return "Not Found...";
            }
            UploadFile insert = new UploadFile
            {
                Name = value.Name,
                Src = value.Src,
                ID = ID
            };
            _AccountBookContext.UploadFile.Add(insert);
            _AccountBookContext.SaveChanges();

            return "ok.";
        }

        // PUT api/<DailyExpenseUploadFileController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DailyExpenseUploadFileController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
