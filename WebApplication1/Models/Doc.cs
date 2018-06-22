using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Doc
    {
        [Key]
        public int ID { set; get; }
        public string Name { set; get; }
        public string Path { set; get; }
    }
}