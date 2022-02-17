using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OnlineChat.Models.News;

namespace OnlineChat.ViewModels
{
    public class CreateModel
    {
        //[Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public IFormFile File1 { get; set; }
        public string Text { get; set; }
        public NewsModel News { get; set; }
    }
}
