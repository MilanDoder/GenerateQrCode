using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationQR.Models
{
    public class Document
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public string fileContent { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateModifier { get; set; }


        public int creatorId { get; set; }
    }
}
