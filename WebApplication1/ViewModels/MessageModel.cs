using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChat.ViewModels
{
    public class MessageModel
    {
        //[Required(ErrorMessage = "Не указано название")]
        public string Name { get; set; }
        public int UserId { get; set; }

        public string Path { get; set; }
        public string Text { get; set; }
    }
}
