using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.System.Models
{
    public class AccessViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
