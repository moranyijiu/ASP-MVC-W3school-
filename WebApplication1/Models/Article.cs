using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Article
    {
        public int ID { set; get; }

        public string Title { set; get; }

        public string Content { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime UpdateTime { set; get; }

        public int Sort { set; get; }

        public int Status { set; get; }

        public string Identity { set; get; }

        public int CourseID { set; get; }

        public int Click { set; get; }

        [ForeignKey("CourseID")]
        public Course Course { set; get; }
    }
}