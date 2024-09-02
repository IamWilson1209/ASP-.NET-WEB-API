using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuanWebAPI.Dtos;
using ShuanWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShuanWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class TodoController : ControllerBase
    {

        // 先在全域宣告資料庫物件
        private readonly AccountBookContext _AccountBookContext;

        public TodoController(AccountBookContext webContext)
        {
            _AccountBookContext = webContext;
        }


        //  取得全部資料 API GET: api/<TodoController>
        [HttpGet]
        public IEnumerable<TodoDtos> GetTodoAll()
        {
            // Dto寫法
            var result = from a in _AccountBookContext.Todo
                         select new TodoDtos
                         {
                             // 要撈什麼欄位在這裡決定
                             TodoId = a.TodoId,
                             Status = a.Status,
                             Thing = a.Thing,
                             Editing = a.Editing,
                             CanEdit = a.CanEdit,
                             Seqno = (int)a.Seqno,
                             CreatTime = a.CreateTime,
                         };
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult GetTodoById(Guid id)
        {
            string groupIdString = id.ToString();
            try
            {
                var result = from a in _AccountBookContext.Todo
                             where a.GroupId == groupIdString
                             select new TodoDtos
                             {
                                 TodoId = a.TodoId,
                                 Status = a.Status,
                                 Thing = a.Thing,
                                 Editing = a.Editing,
                                 CanEdit = a.CanEdit,
                                 Seqno = (int)a.Seqno,
                                 CreatTime = a.CreateTime,
                             };

                if (result == null || !result.Any())
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // 記錄錯誤信息
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddTodo([FromBody] TodoDtos todoDto)
        {
            try
            {
                if (todoDto == null)
                {
                    return BadRequest("Todo object is null");
                }

                // 將接收到的 TodoDtos 資料轉換成 Todo 實體
                Todo todo = new Todo
                {
                    TodoId = todoDto.TodoId,
                    Status = todoDto.Status,
                    Thing = todoDto.Thing,
                    Editing = todoDto.Editing,
                    CanEdit = todoDto.CanEdit,
                    Seqno = todoDto.Seqno
                };

                _AccountBookContext.Todo.Add(todo);
                _AccountBookContext.SaveChanges();

                return CreatedAtAction(nameof(GetTodoById), new { id = todo.TodoId }, todoDto);
            }
            catch (Exception ex)
            {
                // 日誌記錄錯誤信息
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
