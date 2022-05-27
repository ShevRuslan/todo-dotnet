using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using TODOJava.Models;

namespace TODOJava.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        TodoContext _todoContext;
        public TodoController (Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, TodoContext todoContext)
        {
            hostingEnvironment = environment;
            this._todoContext = todoContext;
        }
        [HttpPost("createTODO")]
        public ActionResult Post([FromForm] TodoElement todoElement)
        {
            if(todoElement.image != null)
            {
                var uniqueFileName = GetUniqueFileName(todoElement.image.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                todoElement.image.CopyTo(new FileStream(filePath, FileMode.Create));
                todoElement.filename = uniqueFileName;
            }
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            string currentDate = now.ToUnixTimeSeconds().ToString(); 
            todoElement.datecreate = currentDate;
            _todoContext.Add(todoElement);
            _todoContext.SaveChanges();
            return Ok();
        }
        [HttpGet("getElements")]
        public List<TodoElement> GetTodoElements()
        {
            return _todoContext.TodoElements.ToList();
        }
        [HttpGet("getAllDates")]
        public List<string> GetAllDates()
        {
            var dates = new List<string>();
            foreach (var todoElement in _todoContext.TodoElements)
            {
                dates.Add(todoElement.datecreate);
            }
            return dates;

        }
        [HttpGet("delete/{id}")]
        public ActionResult Delete(int id)
        {
            TodoElement todoElement = _todoContext.TodoElements.Find(id);
            if(todoElement != null)
            {
                _todoContext.TodoElements.Remove(todoElement);
                _todoContext.SaveChanges();
            }
            return Ok();
             
        }
        [HttpPost("changeIsDone")]
        public ActionResult ChangeIsDone([FromForm]int id, [FromForm] Boolean isDone)
        {
            TodoElement todoElement = _todoContext.TodoElements.Find(id);
            if(todoElement != null)
            {
                todoElement.isDone = isDone;
                _todoContext.SaveChanges();
            }
            return Ok();
        }
        [HttpPost("edit")]
        public ActionResult Edit([FromForm] TodoElement todoElement)
        {
            TodoElement _todoElement = _todoContext.TodoElements.Find(todoElement.Id);
            if(_todoElement != null)
            {
                if (todoElement.image != null)
                {
                    var uniqueFileName = GetUniqueFileName(todoElement.image.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    todoElement.image.CopyTo(new FileStream(filePath, FileMode.Create));
                    _todoElement.filename = uniqueFileName;

                }
                _todoElement.name = todoElement.name;
                _todoElement.content = todoElement.content;
                _todoElement.isDone = todoElement.isDone;
                _todoElement.isImportant = todoElement.isImportant;
                _todoContext.SaveChanges();
            }
            return Ok();
        }
        [HttpGet("getStatics")]
        public Statics GetStatics()
        {   
            Statics statics = new Statics();
            DateTime baseDate = DateTime.Now;
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            long thisWeekStartMill = (long)thisWeekStart.ToUniversalTime().Subtract(
                                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                    ).TotalMilliseconds;
            long thisWeekEndMill = (long)thisWeekEnd.ToUniversalTime().Subtract(
                                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                    ).TotalMilliseconds;

            var TodoListOfWeek = _todoContext.TodoElements.Where(el => (long)Convert.ToDouble(el.datecreate)*1000 >= thisWeekStartMill).ToList();
            statics.AllOfWeek = TodoListOfWeek.Count();
            foreach (var todoElement in TodoListOfWeek)
            {
                if (todoElement.isImportant) statics.ImportantOfWeek++;
                if (todoElement.isDone) statics.DoneOfWeek++;
            }
            var AllTodoList = _todoContext.TodoElements.ToList();
            statics.AllTodo = AllTodoList.Count();
            foreach (var todoElement in AllTodoList)
            {
                if (todoElement.isImportant) statics.AllImportant++;
                if (todoElement.isDone) statics.AllDone++;
            }
            return statics;
            
        }
        [HttpGet("export")] 
        public ActionResult GetPathToExportFile()
        {
            List<TodoElement> todoElements = _todoContext.TodoElements.ToList();
            string json = System.Text.Json.JsonSerializer.Serialize(todoElements);
            string path = Path.Combine(hostingEnvironment.WebRootPath, "uploads/export.json");
            System.IO.File.WriteAllText(path, json);
            return Ok();
        }
        [HttpPost("import")] 
        public async Task<IActionResult> ImportFile(IFormFile uploadedFile)
        {
            var uniqueFileName = uploadedFile.FileName;
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            string JSON = System.IO.File.ReadAllText(filePath);
            List<TodoElement> todoElements = JsonConvert.DeserializeObject<List<TodoElement>>(JSON);
            foreach (var todoElement in todoElements)
            {
                _todoContext.TodoElements.Add(new TodoElement { 
                    content = todoElement.content, 
                    datecreate = todoElement.datecreate,
                    isDone = todoElement.isDone,
                    filename = todoElement.filename,
                    image = todoElement.image,
                    isImportant = todoElement.isImportant,
                    name = todoElement.name,
                });
            }
            _todoContext.SaveChanges();
            return Ok();

        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
