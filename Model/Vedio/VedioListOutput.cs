using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Vedio
{
    public class VedioListOutput
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public String VedioTitle { get; set; }
        public String ShortDescription { get; set; }
        public int TimeInSeconds { get; set; } 
        public String ImgeURL { get; set; }
        public DateTime AddedDate { get; set; }
    }
}