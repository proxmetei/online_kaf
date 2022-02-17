using System.ComponentModel.DataAnnotations;
namespace OnlineChat.Models.News
{
    public class NewsModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан Текст")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Не указано Название")]
        public string Title { get; set; }

        public byte[] PictureBytes { get; set; }

        public string PictureName { get; set; }

        public byte[] DocBytes { get; set; }

        public string DocName { get; set; }
    }
}
