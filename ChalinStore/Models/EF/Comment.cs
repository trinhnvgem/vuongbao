using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChalinStore.Models.EF
{
    [Table("tb_Comment")]
    public class Comment: CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn không để trống tiêu đề tin")]
        [StringLength(150)]
        public string Productid { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public virtual Product Product { get; set; }
    }
}