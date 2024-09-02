using Microsoft.AspNetCore.Mvc;
using ShuanWebAPI.Dtos;
using ShuanWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShuanWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        // 先在全域宣告資料庫物件
        private readonly AccountBookContext _AccountBookContext;

        // 依賴注入使用剛設定好的資料庫物件
        public GroupController(AccountBookContext webContext)
        {
            _AccountBookContext = webContext;
        }

        // GET: api/<GroupController>
        [HttpGet]
        public IEnumerable<GroupDto> GetDailyExpenseAll()
        {
            // Dto寫法
            var result = from a in _AccountBookContext.Group
                         select new GroupDto
                         {
                             // 要撈什麼欄位在這裡決定
                             GroupId = a.GroupId,
                             Name = a.Name,
                         };
            return result;
        }
    }
}
