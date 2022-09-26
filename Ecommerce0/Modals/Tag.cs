using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string TagName { get; set; }
    }
}
