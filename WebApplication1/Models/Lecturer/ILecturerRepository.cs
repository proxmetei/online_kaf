using System;
using System.Collections.Generic;
namespace OnlineChat.Models.Lecturer
{



        public interface ILecturerRepository
        {
            public IEnumerable<LecturerModel> GetAllLecturers();

            public LecturerModel GetLecturerByUserId(int id);

            public bool AddLecturer(LecturerModel lecturer);

            public bool UpdateLecturerAchivements(string achiv, int lecId);

            public bool UpdateLecturerPublications(string publ, int lecId);

            public bool UpdateLecturerTeachingInfo(string teacInf, int lecId);

            public bool UpdateLecturerPhoto(byte[] photo, string photoName, int lecId);
        public void UpdateLecturer(LecturerModel lec);

        }
    }
