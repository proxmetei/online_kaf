using System;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using OnlineChat.Models.Chats;
namespace OnlineChat.Models.Lecturer
{
    public class WallMessageRepository : IWallMessageRepository
    {

        private readonly AESCrypt crypt;
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        public WallMessageRepository(IDbConnectionFactory _connectionFactory, AESCrypt _crypt)
        {
            connectionFactory = _connectionFactory;
            crypt = _crypt;
            connection = connectionFactory.GetDbConnection();
        }
        public IEnumerable<WallMessageModel> GetAllPosts(int lecturerId)
        {

                string sql = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID";


                return connection.Query<WallMessageModel>(sql, new { LID = lecturerId });
            
        }

        public IEnumerable<WallMessageModel> GetAllPostsBetween(int fromId, int toId)
        {

                string sqlGet = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE news_id BETWEEN @FIRST AND @LAST";


                return connection.Query<WallMessageModel>(sqlGet, new
                {
                    FIRST = fromId,
                    LAST = toId
                });
            
        }

        public IEnumerable<WallMessageModel> GetLastFivePosts(int lecturerId)
        {

                string sqlMaxId = "SELECT MAX(news_id) FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID";
                string sqlGet = "SELECT lecturer_id AS LecturerId, news_id AS Id, " +
                    "content AS Content " +
                    "FROM public.lecturer_news " +
                    "WHERE lecturer_id = @LID AND news_id BETWEEN @FIRST AND @LAST";


                int maxId = connection.Query<int>(sqlMaxId, new { LID = lecturerId }).FirstOrDefault();

                if (maxId <= 5)
                {
                    return GetAllPosts(lecturerId);
                }
                else
                {
                    return connection.Query<WallMessageModel>(sqlGet, new
                    {
                        LID = lecturerId,
                        FIRST = maxId - 5,
                        LAST = maxId
                    });
                }
            
        }

        public bool AddPost(WallMessageModel news)
        {

                string sqlAdd = "INSERT INTO public.lecturer_news(lecturer_id,  content) " +
                    "VALUES (@LID, @CONT)";

                connection.Open();

                connection.Execute(sqlAdd, new { LID = news.LecturerId, CONT = news.Content });

                return true;
            
        }

        public bool DeletePost(int id)
        {

                string sqlCheck = "SELECT news_id FROM public.lecturer_news " +
                    "WHERE news_id = @NID";
                string sqlDelete = "DELETE FROM public.lecturer_news " +
                    "WHERE news_id = @NID";



                if (connection.Query<string>(sqlCheck, new { NID = id }) == null) return false;

                connection.Execute(sqlDelete, new { NID = id });
                return true;
            }
        
    }
}
