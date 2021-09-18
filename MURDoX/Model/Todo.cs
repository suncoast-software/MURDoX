using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
   public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public Status Status { get; set; }

    }
}
