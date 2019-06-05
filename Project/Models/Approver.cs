using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Approver
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
