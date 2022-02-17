using System;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OnlineChat.Models.Users;
using System.Linq;
using System.Threading.Tasks;
using OnlineChat.Models.Chats;
namespace OnlineChat.Models.Lecturer
{
        public class LecturerRepository : ILecturerRepository
        {

        private readonly AESCrypt crypt;
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        public LecturerRepository(IDbConnectionFactory _connectionFactory, AESCrypt _crypt)
        {
            connectionFactory = _connectionFactory;
            crypt = _crypt;
            connection = connectionFactory.GetDbConnection();
        }
        public IEnumerable<LecturerModel> GetAllLecturers()
            {

                    string sql = "SELECT lecturer_id AS Id, achivements AS Achivements, " +
                        "publications_list AS Publications, teaching_info AS TeachingInfo, " +
                        "user_id AS UserId, photo AS Photo,photo_name AS PhotoName " +
                        "FROM public.lecturer";

                    return connection.Query<LecturerModel>(sql);
                }
            

            public LecturerModel GetLecturerByUserId(int id)
            {
                    string sql = "SELECT lecturer_id AS Id, achivements AS Achivements, " +
                        "publications_list AS Publications, teaching_info AS TeachingInfo, " +
                        "user_id AS UserId, photo AS Photo,photo_name AS PhotoName" +
                        " FROM public.lecturer " +
                        "WHERE user_id = @UID";
                    return connection.Query<LecturerModel>(sql, new { UID = id }).FirstOrDefault();
                
            }

            public bool AddLecturer(LecturerModel lecturer)
            {

                    string sqlInsert = "INSERT INTO public.lecturer(achivements, " +
                        "publications_list, teaching_info, user_id, photo,photo_name) " +
                        "VALUES (@ACH, @PUBL, @TEACIN, @USID, @PH,@PHN)";
                    string sqlCheck = "SELECT id FROM public.site_user " +
                        "WHERE id = @ID";
                    string sqlCheckStatus = "SELECT * FROM public.site_user " +
                        "WHERE id = @ID";

              

                    if (connection.Query<string>(sqlCheck, new { ID = lecturer.UserId }).FirstOrDefault() == null)
                    {
                        return false;
                    }

                    string t = connection.Query<string>(sqlCheckStatus, new { ID = lecturer.UserId }).FirstOrDefault();
            string st = crypt.DecryptUser(connection.Query<User>(sqlCheckStatus, new { ID = lecturer.UserId }).FirstOrDefault()).Status;
                    if (crypt.DecryptUser(connection.Query<User>(sqlCheckStatus, new { ID = lecturer.UserId }).FirstOrDefault()).Status != "Teacher")
                    {
                        
                        return false;
                    }

            connection.Execute(sqlInsert, new
            {
                ACH = lecturer.Achivements,
                PUBL = lecturer.Publications,
                TEACIN = lecturer.TeachingInfo,
                USID = lecturer.UserId,
                PH = lecturer.Photo,
                PHN = lecturer.PhotoName
            }); ;

                    return true;
                
            }

            public bool UpdateLecturerAchivements(string achivs, int lecId)
            {

                    string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                        "WHERE lecturer_id = @ID";
                    string sqlUpd = "UPDATE public.lecturer SET achivements=@ACH WHERE lecturer_id = @ID";

                    if (connection.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                    {
                        return false;
                    }

                    connection.Execute(sqlUpd, new { ID = lecId, ACH = achivs });

                    return true;
                
            }

            public bool UpdateLecturerPublications(string publ, int lecId)
            {
                    string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                        "WHERE lecturer_id = @ID";
                    string sqlUpd = "UPDATE public.lecturer SET publications_list=@PBL WHERE lecturer_id = @ID";



                    if (connection.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                    {
                        return false;
                    }

                    connection.Execute(sqlUpd, new { ID = lecId, PBL = publ });

                    return true;
                
            }

            public bool UpdateLecturerTeachingInfo(string teacInf, int lecId)
            {
                    string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                        "WHERE lecturer_id = @ID";
                    string sqlUpd = "UPDATE public.lecturer SET teaching_info=@TIN WHERE lecturer_id = @ID";



                    if (connection.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                    {
                        return false;
                    }

                    connection.Execute(sqlUpd, new { ID = lecId, TIN = teacInf });

                    return true;

                
            }

            public bool UpdateLecturerPhoto(byte[] photo,string photoName, int lecId)
            {

                    string sqlCheck = "SELECT lecturer_id FROM public.lecturer " +
                        "WHERE lecturer_id = @ID";
                    string sqlUpd = "UPDATE public.lecturer SET photo=@PH,photo_name=@PHN WHERE lecturer_id = @ID";



                    if (connection.Query<string>(sqlCheck, new { ID = lecId }).FirstOrDefault() == null)
                    {
                        return false;
                    }

                    connection.Execute(sqlUpd, new { ID = lecId, PH = photo ,PHN=photoName});

                    return true;

                
            }
        public void UpdateLecturer(LecturerModel lec)
        {
            
            UpdateLecturerAchivements(lec.Achivements,lec.Id);
          
                UpdateLecturerPublications(lec.Publications, lec.Id);
            
                UpdateLecturerTeachingInfo(lec.TeachingInfo, lec.Id);
            if (lec.Photo != null)
                UpdateLecturerPhoto(lec.Photo,lec.PhotoName,lec.Id);
        }
        }
    }

