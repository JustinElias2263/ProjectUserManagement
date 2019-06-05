using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.ManageViewModels
{
    public class NameChangeRequestViewModel
    {
        public string oldFirstName { get; set; }
        public string oldMiddleName { get; set; }
        public string oldLastName { get; set; }

        public string newFirstName { get; set; }
        public string newMiddleName { get; set; }
        public string newLastName { get; set; }

        public string Reason { get; set; }
        public string StatusMessage { get; set; }

        public NameChangeAttachements Attachement { get; set; }
        public IFormFile fileBase { get; set; }


    }
}
