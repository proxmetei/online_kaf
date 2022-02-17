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
namespace OnlineChat.Models.Student
{
    public class StudentRepository : IStudentRepository
    {


        private readonly AESCrypt crypt;
        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbConnection connection;
        public StudentRepository(IDbConnectionFactory _connectionFactory, AESCrypt _crypt)
        {
            connectionFactory = _connectionFactory;
            crypt = _crypt;
            connection = connectionFactory.GetDbConnection();
        }
        public IEnumerable<StudentModel> GetAllStudents()
        {

                string sql = "SELECT student_id AS Id, course AS Course, " +
                     "status AS Status, user_id AS UserId " +
                     "FROM public.student";

      
                return connection.Query<StudentModel>(sql);
            
        }

        public StudentModel GetStudentByStudId(int id)
        {

                string sql = "SELECT student_id AS Id, course AS Course, " +
                     "status AS Status, user_id AS UserId " +
                     "FROM public.student " +
                     "WHERE student_id = @ID";


                return connection.Query<StudentModel>(sql, new { ID = id }).FirstOrDefault();
            
        }

        public bool AddStudent(StudentModel student)
        {
   
                string sql = "INSERT INTO public.student(course, user_id, status) " +
                "VALUES (@CRS, @USID, @ST)";
                string sqlCheck = "SELECT id FROM public.site_user " +
                    "WHERE id = ID";
                string sqlCheckStatus = "SELECT status FROM public.site_user " +
                    "WHERE id = ID";

               

                if (connection.Query<string>(sqlCheck, new { ID = student.UserId }).FirstOrDefault() == null)
                {
                    return false;
                }

                if (connection.Query<string>(sqlCheckStatus, new { ID = student.UserId }).FirstOrDefault() != "student")
                {
                    return false;
                }

                connection.Execute(sql, new { CRS = student.Course, USID = student.UserId, ST = student.Status });

                return true;

            

        }

        public bool UpdateStudent(StudentModel upStudent)
        {

                string updAll = "UPDATE public.student " +
                    "SET course=@CRS, status=@STS " +
                    "WHERE student_id = @ID";
                string updCourse = "UPDATE public.student " +
                    "SET course=@CRS " +
                    "WHERE student_id = @ID";
                string updStatus = "UPDATE public.student " +
                    "SET status=@STS " +
                    "WHERE student_id = @ID";
                string sqlCheck = "SELECT id FROM public.student " +
                    "WHERE student_id = @ID";

     

                if (upStudent.Status == null && upStudent.Course == 0)
                {
                    return false;
                }

                if (connection.Query<string>(sqlCheck, new { ID = upStudent.UserId }).FirstOrDefault() == null)
                {
                    return false;
                }

                if (upStudent.Status != null && upStudent.Course != 0)
                {
                    connection.Execute(updAll, new { CRS = upStudent.Course, STS = upStudent.Status, ID = upStudent.Id });
                }
                else if (upStudent.Status != null && upStudent.Course == 0)
                {
                    connection.Execute(updStatus, new { STS = upStudent.Status, ID = upStudent.Id });
                }
                else if (upStudent.Status == null && upStudent.Course != 0)
                {
                    connection.Execute(updCourse, new { CRS = upStudent.Course, ID = upStudent.Id });
                }


                return true;

            
        }
    }
}
