using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OnlineChat.Models.Users;
using OnlineChat.Models.Messages;
namespace OnlineChat.Models.Chats
{
    public class ChatDAO
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        UserDAO repos;
        public ChatDAO(UserDAO _repos, IDbConnectionFactory _connectionFactory)
        {
            connectionFactory = _connectionFactory;
            repos = _repos;
            connection = connectionFactory.GetDbConnection();
        }
        public void Create(Chat chat, string email)
        {
            var sqlQuery = "INSERT INTO Chats (Name,GUID) VALUES(@Name,@GUID)";
            connection.Execute(sqlQuery, chat);
            string p = chat.Name;
            Chat chat2 = GetOnGUID(chat.GUID);
            User user = repos.GetUserOnEmail(email);
            Join(user.Id, chat2.Id );
        }
        public void Join(int id, int chat_id)
        {
            var sqlQuery = "INSERT INTO relation_chats (chat_id,user_id) VALUES(@chat,@user)";
           //Cha chat = GetChatOnName(chat.Name);
            
            connection.Execute(sqlQuery, new { chat = chat_id, user = id });
            
        }
        public Chat GetChatOnName(string name)
        {
            return connection.Query<Chat>("SELECT * FROM Chats WHERE Name = @name", new { name }).FirstOrDefault();
            
        }
        public List<Message> GetMessages(int id)
        {
            return connection.Query<Message>("SELECT M.id AS Id,M.text AS Text,M.chat_id AS ChatId,M.user_id AS UserId,M.doc_id AS DocId FROM Messages AS M Inner Join Chats AS C  on M.chat_id = c.id WHERE C.id=@id", new { id }).ToList();
           
        }
        public Chat Get(int id)
        {
            return connection.Query<Chat>("SELECT * FROM Chats WHERE id = @id", new { id }).FirstOrDefault();
            
        }

        public Chat GetOnGUID(string id)
        {
            return connection.Query<Chat>("SELECT * FROM Chats WHERE guid = @id", new { id }).FirstOrDefault();

        }
        public Chat GetOnUsers(int id1,int id2)
        {
            int id= connection.Query<int>("SELECT b.chat_id FROM  relation_chats AS b Inner Join relation_chats AS a on b.chat_id=a.chat_id  WHERE b.user_id = @ID1  and a.user_id=@ID2 ", new { ID1=id1,ID2=id2 }).FirstOrDefault();
            return connection.Query<Chat>("SELECT * FROM Chats WHERE id = @id", new { id }).FirstOrDefault();

        }
    }
}
