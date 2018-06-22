using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        Repository repositiory = new Repository();
        private readonly static DbContext dbContext = new BamnContext();
        //返回主页
        public ActionResult Index()
        {
            ViewBag.CourseList = repositiory.GetSelect<Course, DateTime>(m => true, m => m.AddTime, "Catalog");
            return View();
        }
        //返回相应分类页
        public ActionResult Catalog(string identity)
        {
            var model = repositiory.Find<Catalog>(m => m.Identity == identity.ToLower());
            if (model == null)
            {
                return Content("页面不存在");
            }

            ViewBag.CourseList = repositiory.GetSelect<Course, int>(m => m.CatalogID == model.ID, m => m.Sort);

            return View(model);
        }
        //文章页
        public ActionResult Article(string catalogidentity, string courseidentity, string identity)
        {
            var course = dbContext.Set<Course>().Where(m => m.Identity == courseidentity).Include("Catalog").FirstOrDefault();
            ViewBag.Course = course;

            var articleList = repositiory.GetSelect<Article, int>(m => m.CourseID == course.ID, m => m.Sort);
            ViewBag.ArticleList = articleList;

            var model = repositiory.Find<Article>(m => m.Identity == identity.ToLower());
            model.Click +=1;
            if (repositiory.Update(model))
            {
                return View(model);
            }
            return View();           
        }
        //课程页面
        public ActionResult Course(string catalogidentity, string identity)
        {
            var model = dbContext.Set<Course>().Where(m => m.Identity == identity).Include("Catalog").FirstOrDefault();
            if (model == null)
            {
                return Content("页面不存在");
            }
            ViewBag.Course = model;

            var articleList = repositiory.GetSelect<Article, int>(m => m.CourseID == model.ID, m => m.Sort);
            ViewBag.ArticleList = articleList;

            return View();
        }
        //关于界面
        public ActionResult About()
        {
            return View();
        }
        //搜索界面
        public ActionResult Search()
        {
            BamnContext b = new BamnContext();
            string key = "";
            var modellist = b.Article.Where(m => m.Title.Contains(key)).ToList();
            ViewBag.ArticleList = modellist;
            return View();
        }
        //搜索结果返回
        [HttpGet]
        public ActionResult SearchResult(string key)
        {
            BamnContext b = new BamnContext();
            var modellist = b.Article.Where(m => m.Title.Contains(key)).ToList();
            if (modellist.Count==0)
            {
                return Content("你所搜索内容不存在！");
            }
            else
            { 
                ViewBag.ArticleList = modellist;
                return View();
            }

        }
        //根据查询页面跳转到文章页
        public ActionResult Handle(string identity)
        {
            var a = identity;
            var article = repositiory.Find<Article>(m => m.Identity == identity);
            return RedirectToAction("Article", "Home", new { catalogidentity = article.Course.Catalog.Identity, courseidentity = article.Course.Identity, identity = article.Identity });
        }
        //展示下载文档页面
        public ActionResult Document()
        {
            BamnContext b = new BamnContext();
            var list = repositiory.GetSelect<Doc, int>(m => true, m => m.ID).ToList();
            ViewBag.list = list;
            return View();
        }
        //下载逻辑
        public ActionResult Download(string filePath, string fileName)
        {
           
            Encoding encoding;
            string outputFileName = "uploadFile";
            fileName = fileName.Replace("'", "");
            
            string browser = Request.UserAgent.ToUpper();
            if (browser.Contains("MS") == true && browser.Contains("IE") == true)
            {
                outputFileName = HttpUtility.UrlEncode(fileName);
                encoding = Encoding.Default;
            }
            else if (browser.Contains("FIREFOX") == true)
            {
                outputFileName = fileName;
                encoding = Encoding.GetEncoding("GB2312");
            }
            else
            {
                outputFileName = HttpUtility.UrlEncode(fileName);
                encoding = Encoding.Default;
            }
            FileStream fs = new FileStream(filePath+"\\"+fileName,FileMode.Open);
           
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = encoding;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + outputFileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

    }
}