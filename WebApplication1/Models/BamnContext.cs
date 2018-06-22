using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BamnContext:DbContext
    {
        public BamnContext() : base("bamnlink")
        {

        }
        public DbSet<Catalog> Catalog { set; get; }
        public DbSet<Course> Course { set; get; }
        public DbSet<Article> Article { set; get; }
        public DbSet<Doc> Doc { set; get; }
    }
}