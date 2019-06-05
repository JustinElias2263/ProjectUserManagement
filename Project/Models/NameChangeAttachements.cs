using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class NameChangeAttachements
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NameChangeId { get; set; }
        public NameChangeRequest NameChangeRequest { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
