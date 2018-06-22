using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Course
    {
        public Course()
        {
            this.Articles = new HashSet<Article>();
        }

        public int ID { set; get; }

        [Required]
        [StringLength(150)]
        public string Name { set; get; }

        [Required]
        [StringLength(250)]
        public string Intro { set; get; }

        [Required]
        [StringLength(50)]
        public string Thumb { set; get; }

        public DateTime AddTime { set; get; }

        public DateTime UpdateTime { set; get; }

        public int Sort { set; get; }

        public int Status { set; get; }

        [Required]
        [StringLength(30)]
        public string Identity { set; get; }

        public int CatalogID { set; get; }

        [ForeignKey("CatalogID")]
        public Catalog Catalog { set; get; }

        public virtual ICollection<Article> Articles { set; get; }
    }
}