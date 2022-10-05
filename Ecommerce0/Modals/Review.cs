using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public IdentityUser User { get; set; }

        [DataType(DataType.Text)]
        public string ReviewDetail { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedOn { get; set; }
    }
}
