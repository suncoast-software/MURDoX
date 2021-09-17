using System;
using System.ComponentModel.DataAnnotations;

namespace MURDoX.Model
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public ulong LogId { get; set; }
        public ulong AuthorId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
