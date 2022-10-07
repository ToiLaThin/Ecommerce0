using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string OrderName { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public MyIdentityUser User { get; set; }

        [Required]
        public string ContacePhoneNumber { get; set; }

        [Required]
        public string DropOffAddress { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PaidOn { get; set; }
            
        [Required]
        public int PaymentStatus { get; set; }
    }
}
