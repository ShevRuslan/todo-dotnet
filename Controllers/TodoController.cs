﻿using Microsoft.AspNetCore.Mvc;
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
                _todoElement.name = todoElement.name;
                _todoElement.content = todoElement.content;
                _todoElement.isDone = todoElement.isDone;
                _todoElement.isImportant = todoElement.isImportant;
                _todoContext.SaveChanges();
            }
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