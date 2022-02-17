using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChat.Models.Users
{
	public class User
	{
		public int Id { get; set; }

		public string FIO { get; set; }

		public DateTime BirthDate { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string Status { get; set; }
		public byte[] IV { get; set; }
		public byte[] Key { get; set; }
		public override string ToString()
		{
			return "Id = " + Id + '\n' +
				"FirstName = " + FIO + '\n' +
				"Birthday = " + BirthDate + '\n' +
				"Email = " + Email + '\n' +
				"Password = " + Password + '\n' +
				"Role = " + Status + '\n';
		}
	}
}
