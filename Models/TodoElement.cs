using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TODOJava.Models
{
    public class TodoElement
    {
        public string name { get; set; }
        public string content { get; set; }
        public IFormFile image { set; get; }

    }
}
