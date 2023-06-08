//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace ChalinStore.Models.ViewModels
//{
//    public class CommentViewModel
//    {
//        [Key]
//        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
//        public long Id { get; set; }
//        [Required(ErrorMessage = "Bạn không để trống tiêu đề tin")]
//        [StringLength(150)]
//        public string CommentMsg { get; set; }
//        public Nullable<System.DateTime> CommentDate { get; set; }
//        public long ProductId { get; set; }
//        [StringLength(150)]
//        public long UserId { get; set; }
//        public string UserName { get; set; }
//        public long ParenId { get; set; }
//        [Required(ErrorMessage = "Bạn không để trống đánh giá")]
//        [StringLength(150)]
//        public int Rate { get; set; }

//    }
//}