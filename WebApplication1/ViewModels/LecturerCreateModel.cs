using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineChat.ViewModels
{
    public class LeturerCreateModel
    {
        public int Id { get; set; }

        public string Achivements { get; set; }

        public string Publications { get; set; }

        public string TeachingInfo { get; set; }

        public int UserId { get; set; }

        public IFormFile Photo { get; set; }

   
    }
}
