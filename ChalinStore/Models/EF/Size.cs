using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChalinStore.Models.EF
{
    [Table("tb_Size")]
    public class Size : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public bool IsDefault { get; set; }
        public virtual Product Product { get; set; }
    }
}