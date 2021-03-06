using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Model
{
  public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public string TagName { get; set; }
        public string TagDesc { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
