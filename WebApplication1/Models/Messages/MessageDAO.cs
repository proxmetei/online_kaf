using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using OnlineChat.Models.Documents;
namespace OnlineChat.Models.Messages
{
    public class MessageDAO
        //: MessageIntesface
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        public MessageDAO(IDbConnectionFactory _connectionFactory)
        {
            connectionFactory = _connectionFactory;
            connection = connectionFactory.GetDbConnection();
        }
        public void CreateInChat(Message message)
        {
 
                var sqlQuery = "INSERT INTO Messages (text, chat_id, user_id) VALUES(@Text, @ChatId,@UserId)";
            connection.Execute(sqlQuery, message);
        }
        public void CreateInForum(Message message)
        {

            var sqlQuery = "INSERT INTO Messages (text, forum_id, user_id) VALUES(@Text, @ForumId,@UserId)";
            connection.Execute(sqlQuery, message);
        }
        public void CreateWithDoc(Message message,Document doc)
        {
            var sqlQuery1 = "INSERT INTO Documents (guid, name, Data) VALUES(@GUID, @Name, @Data)";
            connection.Execute(sqlQuery1, doc);
            message.DocId = GetDocument(doc.GUID).Id;
            var sqlQuery2 = "INSERT INTO Messages (text, chat_id, user_id, doc_id) VALUES(@Text, @ChatId,@UserId,@DocId)";
            connection.Execute(sqlQuery2, message);
        }

        public Document GetDocument(string GUID)
        {
            return connection.Query<Document>("Select * FROM Documents WHERE GUID = @GUID", new { GUID }).FirstOrDefault();
        }
        public Message Get(int id)
        {
            return connection.Query<Message>("SELECT * FROM Messages WHERE Id = @id", new { id }).FirstOrDefault();
        }
        public Document GetDoc(int id)
        {
            return connection.Query<Document>("SELECT * FROM Documents WHERE id = @id", new { id }).FirstOrDefault();
        }
    }
}
