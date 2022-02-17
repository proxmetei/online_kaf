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
namespace OnlineChat.Models.News
{
    public class NewsRepository : INewsRepository
    {

        private readonly AESCrypt crypt;
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        public NewsRepository(IDbConnectionFactory _connectionFactory, AESCrypt _crypt)
        {
            connectionFactory = _connectionFactory;
            crypt = _crypt;
            connection = connectionFactory.GetDbConnection();
        }
        public List<NewsModel> GetAllNews()
        {

                string sql = "SELECT id AS Id, content AS Content, " +
                     "picture AS PictureBytes, doc AS DocBytes, title AS Title, " +
                     "picture_name AS PictureName, doc_name AS DocName " +
                     "FROM public.global_news";


                return connection.Query<NewsModel>(sql).ToList();
            
        }

        public bool AddNews(NewsModel news)
        {


                string sql1 = "INSERT INTO public.global_news(content, picture, doc, picture_name, doc_name, title) " +
                    "VALUES(@CON, @PIC, @DOC, @PICN, @DOCN, @TITN)";
                string sql2 = "INSERT INTO public.global_news(content, doc, doc_name, title) " +
                    "VALUES(@CON, @DOC, @DOCN, @TITN)";
                string sql3 = "INSERT INTO public.global_news(content, picture, picture_name, title) " +
                    "VALUES(@CON, @PIC,@PICN, @TITN)";
                string sql4 = "INSERT INTO public.global_news(content, title) " +
                    " VALUES(@CON, @TITN) ";



                if (news.DocName == null && news.PictureName == null)
                {
    
                    connection.Execute(sql4, new { CON = news.Content , TITN=news.Title });
                    return true;
                }
                else if (news.DocName == null && news.PictureName != null)
                {
     
                    connection.Execute(sql3, new { CON = news.Content, PIC = news.PictureBytes, PICN = news.PictureName , TITN = news.Title });
                    return true;
                }
                else if (news.DocName != null && news.PictureName == null)
                {

                    connection.Execute(sql2, new { CON = news.Content, DOC = news.DocBytes, DOCN = news.DocName, TITN = news.Title });
                    return true;
                }
                else if (news.DocName != null && news.PictureName != null)
                {

                    connection.Execute(sql1,
                        new
                        {
                            CON = news.Content,
                            DOC = news.DocBytes,
                            DOCN = news.DocName,
                            PIC = news.PictureBytes,
                            PICN = news.PictureName,
                            TITN = news.Title
                        });
                    return true;
                }
                else return false;

            
        }
        public bool UpdateNews(NewsModel news)
        {


            string sql1 = "UPDATE public.global_news SET (content, picture, doc, picture_name, doc_name, title) = " +
                "(@CON, @PIC, @DOC, @PICN, @DOCN, @TITN) WHERE id=@ID";
            string sql2 = "UPDATE public.global_news SET (content, doc, doc_name, title) = " +
                "(@CON, @DOC, @DOCN, @TITN) WHERE id=@ID";
            string sql3 = "UPDATE public.global_news SET (content, picture, picture_name, title) = " +
                "(@CON, @PIC,@PICN, @TITN) WHERE id=@ID";
            string sql4 = "UPDATE public.global_news SET (content, title) = " +
                " (@CON, @TITN) WHERE id=@ID";



            if (news.DocName == null && news.PictureName == null)
            {

                connection.Execute(sql4, new { CON = news.Content, TITN = news.Title, ID=news.Id });
                return true;
            }
            else if (news.DocName == null && news.PictureName != null)
            {

                connection.Execute(sql3, new { CON = news.Content, PIC = news.PictureBytes, PICN = news.PictureName, TITN = news.Title, ID = news.Id });
                return true;
            }
            else if (news.DocName != null && news.PictureName == null)
            {

                connection.Execute(sql2, new { CON = news.Content, DOC = news.DocBytes, DOCN = news.DocName, TITN = news.Title , ID = news.Id });
                return true;
            }
            else if (news.DocName != null && news.PictureName != null)
            {

                connection.Execute(sql1,
                    new
                    {
                        CON = news.Content,
                        DOC = news.DocBytes,
                        DOCN = news.DocName,
                        PIC = news.PictureBytes,
                        PICN = news.PictureName,
                        TITN = news.Title,
                        ID = news.Id
                    });
                return true;
            }
            else return false;


        }
        public IEnumerable<NewsModel> GetAllNewsBetween(int fromId, int toId)
        {

                string sql = "SELECT id AS Id, content AS Content, " +
                    "picture AS PictureBytes, doc AS DocBytes, title AS Title, " +
                    "picture_name AS PictureName, doc_name AS DocName " +
                    "FROM public.global_news " +
                    "WHERE id BETWEEN @FIRST AND @LAST";

             
                return connection.Query<NewsModel>(sql, new { FIRST = fromId, LAST = toId });
            
        }

        public IEnumerable<NewsModel> GetLastTenNews(int index)
        {

                string sql1 = "SELECT MAX(id) FROM public.global_news";

                int lastId = connection.Query<int>(sql1).FirstOrDefault()-index;

                int firstId = lastId <= 10 ? firstId = 1 : lastId - 10;


              
                return GetAllNewsBetween(firstId, lastId);
            
        }

        //public NewsModel GetNews(int id)
        //{

        //    string sql = "SELECT id AS Id, content AS Content, " +
        //              "picture AS PictureBytes, doc AS DocBytes, title AS Title " +
        //              "picture_name AS PictureName, doc_name AS DocName " +
        //              "FROM public.global_news WHERE id=@ID";

        //   return connection.Query<NewsModel>(sql, new { ID = id}).FirstOrDefault();



        //}
        public bool DeleteNews(int id)
        {

            string sql1 = "SELECT id FROM public.global_news WHERE id = @ID";
            string sql2 = "DELETE FROM public.global_news WHERE id = @ID";



            string check = connection.Query<string>(sql1, new { ID = id }).FirstOrDefault();

            if (connection.Query<string>(sql1, new { ID = id }).FirstOrDefault() == null)
            {
                return false;
            }

            connection.Execute(sql2, new { ID = id });

            return true;

        }

        public NewsModel GetNews(int id)
        {

            string sql = "SELECT id AS Id, content AS Content, " +
                "picture AS PictureBytes, doc AS DocBytes, title AS Title, " +
                "picture_name AS PictureName, doc_name AS DocName " +
                "FROM public.global_news WHERE id=@ID";



            return connection.Query<NewsModel>(sql, new { ID = id }).FirstOrDefault();

             
            
        }
    }
}
