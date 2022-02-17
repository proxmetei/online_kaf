using System.Collections.Generic;
namespace OnlineChat.Models.Lecturer
{

        public interface IWallMessageRepository
        {
            public IEnumerable<WallMessageModel> GetAllPosts(int lecturerId);

            public IEnumerable<WallMessageModel> GetAllPostsBetween(int fromId, int toId);

            public IEnumerable<WallMessageModel> GetLastFivePosts(int lecturerId);

            public bool AddPost(WallMessageModel news);

            public bool DeletePost(int id);
        }
    
}
