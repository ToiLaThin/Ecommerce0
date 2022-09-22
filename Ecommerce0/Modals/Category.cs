using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name{ get; set; }

        public string Description { get; set; }

        //public Category ParentCategory { get; set; }
    }
}
