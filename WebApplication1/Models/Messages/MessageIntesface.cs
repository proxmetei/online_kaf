using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineChat.Models.Messages
{
    public interface MessageIntesface
    {
        void Create(Message message);
    }
}
