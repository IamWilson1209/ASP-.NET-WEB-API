using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuanWebAPI.Parameters;
using ShuanWebAPI.Dtos;
using ShuanWebAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;


namespace ShuanWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class DailyExpenseController : ControllerBase
    {
        // 先在全域宣告資料庫物件
        private readonly AccountBookContext _AccountBookContext;

        // 依賴注入使用剛設定好的資料庫物件
        public DailyExpenseController(AccountBookContext webContext) 
        {
            _AccountBookContext = webContext;
        }

        // ======================== GET =================================

        //  取得全部資料 API GET: api/<DailyExpenseController>
        [HttpGet]
        public IEnumerable<DailyExpenseDtos> GetDailyExpenseAll()
        {
            Console.WriteLine("Received GET request for all daily expenses");
            // Dto寫法
            var result = from a in _AccountBookContext.DailyExpense
                             select new DailyExpenseDtos
                             {
                                 // 要撈什麼欄位在這裡決定
                                 ID = a.ID,
                                 RecordDateTime = a.RecordDateTime,
                                 Category = a.Category,
                                 Item = a.Item,
                                 Cost = a.Cost,
                                 Bank = a.Bank
                             };
            Console.WriteLine($"Returning {result.Count()} daily expenses");
            Console.WriteLine("HIIIIIII");
            return result;
        }

        //  條件篩選資料 API GET: api/<DailyExpenseController>
        [HttpGet("byFilter")]
        public IEnumerable<DailyExpenseDtos> GetDailyExpensesByFilter(DateTime? startDate, DateTime? endDate, string bank, string category, string item)
        {
            Console.WriteLine("Returning hi daily expenses");
            var query = _AccountBookContext.DailyExpense.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(a => a.RecordDateTime >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(a => a.RecordDateTime <= endDate.Value);
            }
            if (!string.IsNullOrEmpty(bank) && bank != "All Banks")
            {
                query = query.Where(a => a.Bank == bank);
            }
            if (!string.IsNullOrEmpty(category) && category != "All Categories")
            {
                query = query.Where(a => a.Category == category);
            }
            if (!string.IsNullOrEmpty(item) && item != "All Items")
            {
                query = query.Where(a => a.Item == item);
            }

            var result = query.Select(a => new DailyExpenseDtos
            {
                ID = a.ID,
                RecordDateTime = a.RecordDateTime,
                Category = a.Category,
                Item = a.Item,
                Cost = a.Cost,
                Bank = a.Bank
            }).ToList();

            Console.WriteLine($"Returning {result.Count()} daily expenses");
            return result;
        }

        // 取得Bank下拉式選單的列表
        [HttpGet("getbanks")]
        public IEnumerable<string> GetAllBanks()
        {
            var result = _AccountBookContext.Bank
                                            .Select(b => b.BankName)
                                            .ToList();
            Console.WriteLine($"Returning bank ");
            return result;
        }

        // 取得Bank下拉式選單的列表
        [HttpGet("getcategories")]
        public IEnumerable<string> GetAllCategories()
        {
            var result = _AccountBookContext.Category
                                            .Select(b => b.CategoryName)
                                            .ToList();
            return result;
        }

        // 取得Bank下拉式選單的列表
        [HttpGet("getitems")]
        public IEnumerable<string> GetAllItems()
        {
            var result = _AccountBookContext.Item
                                            .Select(b => b.ItemName)
                                            .ToList();
            return result;
        }


        //  關鍵字搜索 API GET: api/<DailyExpenseController>，輸入搜尋參數，回傳Dto
        [HttpGet("keyword")]
        public IEnumerable<DailyExpenseDtos> GetDailyExpenseKeyword([FromQuery] DailyExpenseSearchParameters value)
        {
            // Dto寫法
            var result = _AccountBookContext.DailyExpense
                .Include(a => a.UploadFile) // 資料表有關聯對應才可以這樣寫
                .Select(a => a);

            // 找出指定Category的資料
            if (!string.IsNullOrWhiteSpace(value.Category))
            {
                result = result.Where(a => a.Category.IndexOf(value.Category) > -1);
            }

            // 找出指定Item的資料
            if (!string.IsNullOrWhiteSpace(value.Item))
            {
                result = result.Where(a => a.Item.IndexOf(value.Item) > -1);
            }

            // 找出指定RecordDateTime的資料
            if (value.RecordDateTime != null)
            {
                // 只指定日期
                result = result.Where(a => a.RecordDateTime.Date == value.RecordDateTime);
            }

            // 找出指定Bank的資料
            if (!string.IsNullOrWhiteSpace(value.Bank))
            {
                result = result.Where(a => a.Bank.IndexOf(value.Bank) > -1);
            }

            // ItemToDto函式化，把SQL抓出來的東西轉換成DTO，最好放在結尾
            return result.ToList().Select(a=>ItemToDto(a));
        }

        // 取得指定一筆資料 API GET api/<ExpenseController>/5
        // 如果資歷表沒有關聯對應，手動新增方式
        [HttpGet("{id}")]
        public DailyExpenseDtos GetDailyExpenseID(Guid id)
        {
            var result = (from a in _AccountBookContext.DailyExpense
                          where a.ID == id
                          select new DailyExpenseDtos
                          {
                              RecordDateTime = a.RecordDateTime,
                              Category = a.Category,
                              Item = a.Item,
                              Cost = a.Cost,
                              Bank = a.Bank,
                              UploadFile = (from b in _AccountBookContext.UploadFile
                                            where a.ID == b.ID
                                            select new UploadFileDto
                                            {
                                                Name = b.Name,
                                                Src = b.Src,
                                                ID = b.ID,
                                                UploadFileID = b.UploadFileID,
                                            }).ToList()
                          }).SingleOrDefault();  //只會撈出一筆的意思，沒撈到不會報錯
            return result;
        }

        // ======================== POST =================================

        // POST api/<ExpenseController>
        // 父子資料有關聯規則的寫法 + Dto寫法
        [HttpPost]
        public IActionResult PostDailyExpense([FromBody] DailyExpensePOSTDto value)
        {
            // 簡化使用者要上傳的欄位
            DailyExpense insert_value = new DailyExpense
            {
                RecordDateTime = DateTime.Now, // 即時指定上傳時間
                UpdateDateTime = DateTime.Now, // 即時指定上傳時間
            };
            // CurrentValues 會佔存待傳輸的資料
            // SetValues 會把Dto跟上傳資料匹配，自動補上有匹配到的資料
            _AccountBookContext.DailyExpense.Add(insert_value).CurrentValues.SetValues(value);
            _AccountBookContext.SaveChanges();

            // 沒有fk也可以這樣寫
            foreach (var temp in insert_value.UploadFile)
            {
                _AccountBookContext.UploadFile.Add(new UploadFile() 
                {
                    ID = insert_value.ID
                }).CurrentValues.SetValues(temp);
            }

            _AccountBookContext.SaveChanges();

            // 返回符合規範的內容，超好用
            return CreatedAtAction(nameof(GetDailyExpenseID), new { ID = insert_value.ID}, insert_value);
        }

        // 父子資料沒有關聯規則的寫法 + Dto寫法
        [HttpPost("nofk")]
        public void Postnofk([FromBody] DailyExpensePOSTDto value)
        {

            DailyExpense insert = new DailyExpense
            {
                RecordDateTime = DateTime.Now, // 即時指定上傳時間
                Category = value.Category,
                Item = value.Item,
                Cost = value.Cost,
                Bank = value.Bank,
                UpdateDateTime = DateTime.Now, // 即時指定上傳時間
            };
            _AccountBookContext.DailyExpense.Add(insert);
            _AccountBookContext.SaveChanges(); // 要先存檔才會有主key

            foreach (var temp in value.UploadFile)
            {
                UploadFile insert2 = new UploadFile
                {
                    Name = temp.Name,
                    Src = temp.Src,
                    ID = insert.ID
                };

                _AccountBookContext.UploadFile.Add(insert2);
            }

            _AccountBookContext.SaveChanges();
        }

        [HttpPost("postdailyexpense")]
        public IActionResult PostDailyExpenseByUploader([FromBody] DailyExpensePOSTDto value)
        {
            try
            {
                Console.WriteLine("postdailyexpense success");
                // 簡化使用者要上傳的欄位
                DailyExpense insert_value = new DailyExpense
                {
                };
                // CurrentValues 會佔存待傳輸的資料
                // SetValues 會把Dto跟上傳資料匹配，自動補上有匹配到的資料
                _AccountBookContext.DailyExpense.Add(insert_value).CurrentValues.SetValues(value);
                _AccountBookContext.SaveChanges();

                if (value.UploadFile != null && value.UploadFile.Count > 0)
                {
                    // 沒有fk也可以這樣寫
                    foreach (var temp in insert_value.UploadFile)
                    {
                        _AccountBookContext.UploadFile.Add(new UploadFile()
                        {
                            ID = insert_value.ID
                        }).CurrentValues.SetValues(temp);
                    }
                }

                _AccountBookContext.SaveChanges();

                // 返回符合規範的內容，超好用
                return CreatedAtAction(nameof(GetDailyExpenseID), new { ID = insert_value.ID }, new { message = "資料成功上傳" });
            }
            catch (Exception ex)
            {
                // 返回錯誤訊息
                return BadRequest(new { message = "資料上傳失敗", error = ex.Message });
            }
        }
        // ======================== PUT 還不能用=================================

        // PUT api/<ExpenseController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] DailyExpense value)
        {
            // 如果前端輸入的ID不是ID
            if (id != value.ID)
            {
                return BadRequest();
            }

            var update =(from a in _AccountBookContext.DailyExpense
                         where a.ID ==id
                         select a).SingleOrDefault();

            if (update != null)
            {
                // 不能修改的在這裡
                update.UpdateDateTime = DateTime.Now;

                // 自動對應完成更新
                _AccountBookContext.DailyExpense.Update(update).CurrentValues.SetValues(value);

                _AccountBookContext.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return NoContent();
        }

        // ======================== DELETE =================================

        // DELETE api/<ExpenseController>/id
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)    
        {
            var delete = (from a in _AccountBookContext.DailyExpense
                          where a.ID == id
                          select a).Include(c => c.UploadFile).SingleOrDefault();

            if (delete == null)
            {
                return NotFound("找不到可以刪除的資源");
            }

            if (delete != null)
            {
                _AccountBookContext.DailyExpense.Remove(delete);
                _AccountBookContext.SaveChanges();
            }

            return NoContent();
        
        }

        // 無外鍵情況下刪除子資料
        [HttpDelete("nofk/{id}")]
        public void Deletenofk(Guid id)
        {

            var child = from a in _AccountBookContext.UploadFile
                        where a.ID == id
                        select a;

            _AccountBookContext.UploadFile.RemoveRange(child);
            _AccountBookContext.SaveChanges();


            var delete = (from a in _AccountBookContext.DailyExpense
                          where a.ID == id
                          select a).Include(c => c.UploadFile).SingleOrDefault();

            if (delete != null)
            {
                _AccountBookContext.DailyExpense.Remove(delete);
                _AccountBookContext.SaveChanges();
            }

        }

        // 刪除多筆資料，還不能用
        [HttpDelete("list/{id}")]
        public void Delete(string ids)
        {

            List<Guid> deleteList = JsonSerializer.Deserialize<List<Guid>>(ids);

            var delete = (from a in _AccountBookContext.DailyExpense
                        where deleteList.Contains(a.ID)
                        select a).Include(c => c.UploadFile);

            _AccountBookContext.DailyExpense.RemoveRange(delete);
            _AccountBookContext.SaveChanges();

        }




        // 設定函式，名稱：ItemToDto，以後要修改直接從這邊就好
        // 傳入DailyExpense實體，傳回轉換後的Dto
        private static DailyExpenseDtos ItemToDto(DailyExpense item)
        {

            List<UploadFileDto> updto = new List<UploadFileDto>();

            foreach (var temp in item.UploadFile)
            {
                UploadFileDto up = new UploadFileDto
                {
                    Name = temp.Name,
                    Src = temp.Src,
                    ID = temp.ID,
                    UploadFileID = temp.UploadFileID,
                };
                updto.Add(up);
            }
            return new DailyExpenseDtos
            {
                ID = item.ID,
                Category = item.Category,
                Item = item.Item,
                RecordDateTime = item.RecordDateTime,
                Cost = item.Cost,
                Bank = item.Bank,
                UploadFile = updto,
            };
        }
    }
}

