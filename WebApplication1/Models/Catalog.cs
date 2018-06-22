using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Catalog
    {
        public Catalog()
        {
            this.Courses = new HashSet<Course>();
        }
        public int ID { set; get; }

        public string Name { set; get; }

        public string Identity { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime UpdateTime { set; get; }

        public int Sort { set; get; }

        public int Status { set; get; }

        public virtual ICollection<Course> Courses { set; get; }
    }
}