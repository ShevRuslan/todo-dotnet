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
        public TodoController (Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }
        [HttpPost("createTODO")]
        public TodoElement Post([FromForm] TodoElement todoElement)
        {
            var uniqueFileName = GetUniqueFileName(todoElement.image.FileName);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            todoElement.image.CopyTo(new FileStream(filePath, FileMode.Create));
            return todoElement;
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
