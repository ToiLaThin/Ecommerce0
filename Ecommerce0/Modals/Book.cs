using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title{ get; set; }

        [Required]
        public float Price{ get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }


        //navigation property of Entity Framework: do not carry data
        public Category Category { get; set; }
    }
}
