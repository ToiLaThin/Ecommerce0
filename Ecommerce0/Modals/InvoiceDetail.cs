using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book{ get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float PricePerUnit { get; set; }
        public float TotalItemPrice { get; set; }
    }
}
