using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOJava.Models
{
    public class TodoElement
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string? filename { get; set; }

        [NotMapped]
        public IFormFile? image { set; get; }
        public string? datecreate { get; set; }

        public bool isImportant { get; set; }
        public bool isDone { get; set; }
    }
}
