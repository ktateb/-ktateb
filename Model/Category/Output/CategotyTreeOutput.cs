using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Category.Output
{
    public class CategotyTreeOutput
    {
        public int Id { get; set; }
        public string  name { get; set; }
        public int?  ParentId { get; set; }
        public List<CategotyTreeOutput> Childs { get; set; }
    }
}