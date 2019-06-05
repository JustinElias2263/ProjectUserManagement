using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class NameChangeRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string oldFirstName { get; set; }
        public string oldMiddleName { get; set; }
        public string oldLastName { get; set; }

        public string newFirstName { get; set; }
        public string newMiddleName { get; set; }
        public string newLastName { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<NameChangeAttachements> NameChangeAttachements { get; set; }
    }
}
