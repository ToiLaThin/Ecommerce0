using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce0.Modals
{
    public class Invoice
    {

        [Key]
        public int Id { get; set; }
            
        public string InvoiceName{ get; set; }

        [Required]
        public string CustomerId{ get; set; }

        [ForeignKey("CustomerId")]
        public MyIdentityUser User { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]        
        public int PaymentStatus { get; set; }
    }
}
