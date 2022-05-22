using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
            string currentDate = DateTime.Now.ToString(); 
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
