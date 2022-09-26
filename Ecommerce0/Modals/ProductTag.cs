using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class ProductTag
    {
        [Key]
        public int Id { get; set; }
        public int IdTag { get; set; }
        [ForeignKey("IdTag")]
        public Tag Tag { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey("IdProduct")]
        public Book Product{ get; set; }
    }
}
