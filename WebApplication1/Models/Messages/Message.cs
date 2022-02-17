using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineChat.Models.Chats;
using Microsoft.AspNetCore.Http;
using OnlineChat.Models.Documents;
namespace OnlineChat.Models.Messages
{
    public class Message
    {
		public int Id { get; set; }
	    public Document Doc { get; set; }
		public string Text { get; set; }
		public int ChatId { get; set; }
		public int ForumId { get; set; }
		public int UserId { get; set; }
		public int DocId { get; set; }
	}
}
