using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OnlineChat.Models.Messages;
namespace OnlineChat.Hubs
{
    public class ChatHub : Hub
    {
        MessageDAO repos;
        public ChatHub(MessageDAO _repos)
		{
            repos = _repos;
		}
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task SendMessage( string login,Message message,string roomName,string file)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            repos.CreateInChat(message);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", login, message,file);
        }
    }
}
