using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChalinStore.Models.EF
{
    [Table("tb_Comment")]
    public class Comment 
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn không để trống tiêu đề tin")]
        [StringLength(150)]
        public string CommentMsg { get; set; }

        public DateTime ? CommentDate { get; set; }
        public int? ProductId { get; set; }
        [StringLength(150)] public string UserId { get; set; }
        public int? ParenId  { get; set; }

        [Required(ErrorMessage = "Bạn không để trống đánh giá")]
        [StringLength(150)]
        public string Rate { get; set; }
    }
}