using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce0.Modals
{
    public class MyIdentityUser: IdentityUser
    {
        //là nullable ?
        public string? AvatarPath { get; set; } 
    }
}
