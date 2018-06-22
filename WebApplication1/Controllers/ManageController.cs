using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using System.Collections.Generic;
using System;
using System.IO;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    //验证登录
    [Authorize]
    public class ManageController : Controller
    {
        Repository repositiory = new Repository();
        BamnContext bamnContext = new BamnContext();
        //添加分类
        public ActionResult AddCatalog()
        {
            var list = repositiory.GetSelect<Catalog,int>(m => true,m => m.Sort).ToList();
            ViewData["CatalogList"] = GetTimeHourList(list, list.Count == 0 ? 0 : list.LastOrDefault().Sort);
            return View();
        }
        //检查分类identy
        public JsonResult CheckIdentity(string identity)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(identity))
            {
                var model = repositiory.Find<Catalog>(m => m.Identity == identity.ToLower());
                if (model == null)
                {
                    res = true;
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //检查课程identity
        public JsonResult CheckIdentityForCourse(string identity,int catalogId)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(identity))
            {
                var model = repositiory.Find<Course>(m => m.Identity == identity.ToLower()&& m.CatalogID == catalogId);
                if (model == null)
                {
                    res = true;
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //检查文章identity
        public JsonResult CheckIdentityForArticle(string identity, int courseId)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(identity))
            {
                var model = repositiory.Find<Article>(m => m.Identity == identity.ToLower() && m.CourseID == courseId);
                if (model == null)
                {
                    res = true;
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetTimeHourList(List<Catalog> catalogList, int lastSort)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (lastSort == 0)
            {
                list.Add(new SelectListItem { Text = "请选择上一条分类", Value = "0", Selected = true });
            }
            else
            {
                list.Add(new SelectListItem { Text = "请选择上一条分类", Value = "0" });
            }


            for (int i = 0; i < catalogList.Count; i++)
            {
                if (catalogList[i].Sort == lastSort)
                {
                    list.Add(new SelectListItem { Text = catalogList[i].Name, Value = catalogList[i].Sort.ToString(), Selected = true });
                }
                else
                {
                    list.Add(new SelectListItem { Text = catalogList[i].Name, Value = catalogList[i].Sort.ToString() });
                }
            }

            return list;
        }
        //添加分类
        [HttpPost]
        public ActionResult AddCatalog(ViewAddCatalogModel model)
        {
            if (ModelState.IsValid)
            {
                var info = new Catalog();
                info.Name = model.Name;
                //算法计算
                info.Sort = model.LastSort + 1;
                info.AddTime = DateTime.Now;
                info.UpdateTime = DateTime.Now;
                info.Status = 1;
                info.Identity = model.Identity.ToLower();
                if (repositiory.Add<Catalog>(info))
                {
                    //改变其他的排序
                    //第一
                    if (model.LastSort == 0)
                    {
                        bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Catalogs] SET [Sort] = [Sort]+1 WHERE [ID]<{0}", info.ID);
                    }
                    //中间
                    else if (repositiory.GetCount<Catalog>(m => m.Sort >= info.Sort && m.ID != info.ID) > 0)
                    {
                        bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Catalogs] SET [Sort] = [Sort]+1 WHERE [ID]!={0} and [Sort]>={1}", info.ID, info.Sort);
                    }
                    return Redirect(string.Format("/{0}.html", info.Identity));
                }
            }
            var list = repositiory.GetSelect<Catalog>(m => true);
            ViewBag.CatalogList = list;
            return View(model);
        }
          //修改分类
        public ActionResult ModifyCatalog(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return Content("路径不对");
            }
            var model = repositiory.Find<Catalog>(m => m.Identity == identity.ToLower());
            if (model == null)
            {
                return Content("路径不对");
            }

            var list = repositiory.GetSelect<Catalog,int>(m => m.Identity != identity.ToLower(),m => m.Sort);
            ViewData["CatalogList"] = GetTimeHourList(list,model.Sort-1);
            var viewModel = new ViewAddCatalogModel();
            viewModel.Identity = model.Identity;
            viewModel.Name = model.Name;
            viewModel.LastSort = model.Sort - 1;
            return View(viewModel);
        }
        //修改分类提交到这里
        [HttpPost]
        public ActionResult ModifyCatalog(ViewAddCatalogModel model)
        {
            if (ModelState.IsValid)
            {
                var info = repositiory.Find<Catalog>(m => m.Identity == model.Identity.ToLower());
                if (info == null)
                {
                    return Content("路径不对");
                }
                info.Name = model.Name;

                if (model.LastSort!=info.Sort-1)
                {
                    if (model.LastSort< info.Sort - 1)
                    {
                        bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Catalogs] SET [Sort] = [Sort]+1 WHERE [Sort]<{0} and [Sort]>{1}", info.Sort,model.LastSort);
                    }
                    else
                    {                        
                        bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Catalogs] SET [Sort] = [Sort]-1 WHERE [Sort]>{0} and [Sort]<={1}", info.Sort, model.LastSort);
                    }
                    info.Sort = model.LastSort + 1;
                }

                if (repositiory.Update(info))
                {
                    return Redirect(string.Format("/{0}.html", info.Identity));
                }
            }
            var list = repositiory.GetSelect<Catalog>(m => true);
            ViewBag.CatalogList = list;
            return View(model);
        }
        //添加课程显示页
        public ActionResult AddCourse(int id)
        {
            var catalog = repositiory.FindByID<Catalog>(id);
            if (catalog == null)
            {
                return Content("路径不对");
            }

            ViewBag.Catalog = catalog;

            var list = repositiory.GetSelect<Course, int>(m => true, m => m.Sort);
            ViewData["CourseList"] = GetCourseList(list, list.Count == 0 ? 0 : list.LastOrDefault().Sort);

            return View();
        }
        //课程列表
        private List<SelectListItem> GetCourseList(List<Course> courseList, int lastSort)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (lastSort == 0)
            {
                list.Add(new SelectListItem { Text = "请选择上一个课程", Value = "0", Selected = true });
            }
            else
            {
                list.Add(new SelectListItem { Text = "请选择上一个课程", Value = "0" });
            }


            for (int i = 0; i < courseList.Count; i++)
            {
                if (courseList[i].Sort == lastSort)
                {
                    list.Add(new SelectListItem { Text = courseList[i].Name, Value = courseList[i].Sort.ToString(), Selected = true });
                }
                else
                {
                    list.Add(new SelectListItem { Text = courseList[i].Name, Value = courseList[i].Sort.ToString() });
                }
            }

            return list;
        }
        //添加课程提交页
        [HttpPost]
        public ActionResult AddCourse(ViewAddCourseModel model,HttpPostedFileBase thumb)
        {
            var catalog = repositiory.FindByID<Catalog>(model.CatalogID);

            if (ModelState.IsValid)
            {
                var info = new Course();
                info.Name = model.Name;
                //算法计算
                info.Sort = model.LastSort + 1;
                info.AddTime = DateTime.Now;
                info.UpdateTime = DateTime.Now;
                info.Status = 1;
                info.CatalogID = model.CatalogID;
                info.Intro = model.Intro;
                info.Identity = model.Identity.ToLower();

                //处理幅面图片
                var fileName = DateTime.Now.ToString("yyyyMMddmmhhss") + thumb.FileName.Substring(thumb.FileName.LastIndexOf("."));
                var filePath = Server.MapPath(string.Format("~/{0}", "Uploads"));
                thumb.SaveAs(Path.Combine(filePath, fileName));
                info.Thumb = string.Format("/{0}",fileName);

                try
                {
                    if (repositiory.Add<Course>(info))
                    {
                        //改变其他的排序
                        // 排第一
                        if (model.LastSort == 0)
                        {
                            bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Courses] SET [Sort] = [Sort]+1 WHERE [ID]<{0}", info.ID);
                        }
                        //排中间
                        else if (repositiory.GetCount<Course>(m => m.Sort >= info.Sort && m.ID != info.ID) > 0)
                        {
                            bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Courses] SET [Sort] = [Sort]+1 WHERE [ID]!={0} and [Sort]>={1}", info.ID, info.Sort);
                        }

                        return Redirect(string.Format("/{1}/{0}.html", info.Identity,catalog.Identity));
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }

            ViewBag.Catalog = catalog;

            var list = repositiory.GetSelect<Course, int>(m => true, m => m.Sort);
            ViewData["CourseList"] = GetCourseList(list, list.Count == 0 ? 0 : list.LastOrDefault().Sort);
            ViewBag.CatalogList = list;

            return View(model);
        }
        //添加文章
        public ActionResult AddArticle(int id)
        {
            var course = repositiory.FindByID<Course>(id);
            if (course == null)
            {
                return Content("路径不对");
            }

            ViewBag.Course = course;

            var list = repositiory.GetSelect<Article, int>(m => m.CourseID==id, m => m.Sort);
            ViewData["ArticleList"] = GetArticleList(list, list.Count == 0 ? 0 : list.LastOrDefault().Sort);

            return View();
        }
        //文章列表
        private List<SelectListItem> GetArticleList(List<Article> articlelist, int lastSort)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            if (lastSort == 0)
            {
                list.Add(new SelectListItem { Text = "请选择上一篇文章", Value = "0", Selected = true });
            }
            else
            {
                list.Add(new SelectListItem { Text = "请选择上一篇文章", Value = "0" });
            }


            for (int i = 0; i < articlelist.Count; i++)
            {
                if (articlelist[i].Sort == lastSort)
                {
                    list.Add(new SelectListItem { Text = articlelist[i].Title, Value = articlelist[i].Sort.ToString(), Selected = true });
                }
                else
                {
                    list.Add(new SelectListItem { Text = articlelist[i].Title, Value = articlelist[i].Sort.ToString() });
                }
            }

            return list;
        }

        [HttpPost]
        //解决富文本编辑器内容传输
        [ValidateInput(false)]
        public ActionResult AddArticle(ViewAddArticleModel model)
        {
            var course = bamnContext.Set<Course>().Where(m => m.ID == model.CourseID).Include("Catalog").FirstOrDefault();
            if (course == null)
            {
                return Content("路径不对");
            }

            ViewBag.Course = course;

            if (ModelState.IsValid)
            {
                var info = new Article();
                info.Title = model.Title;
                //算法计算
                info.Sort = model.LastSort + 1;
                info.AddTime = DateTime.Now;
                info.UpdateTime = DateTime.Now;
                info.Status = 1;
                info.CourseID = model.CourseID;
                info.Content = model.Content;
                info.Identity = model.Identity.ToLower();

                try
                {
                    if (repositiory.Add<Article>(info))
                    {
                        //改变其他的排序
                        //排第一
                        if (model.LastSort == 0)
                        {
                            bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Articles] SET [Sort] = [Sort]+1 WHERE [ID]<{0}", info.ID);
                        }//排中间
                        else if (repositiory.GetCount<Article>(m => m.Sort >= info.Sort && m.ID != info.ID) > 0)
                        {
                            bamnContext.Database.ExecuteSqlCommand("UPDATE [dbo].[Articles] SET [Sort] = [Sort]+1 WHERE [ID]!={0} and [Sort]>={1}", info.ID, info.Sort);
                        }

                        return Redirect(string.Format("/{2}/{1}/{0}.html", info.Identity, course.Identity,course.Catalog.Identity));
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }

            var list = repositiory.GetSelect<Article, int>(m => m.CourseID == model.CourseID, m => m.Sort);
            ViewData["ArticleList"] = GetArticleList(list, list.Count == 0 ? 0 : list.LastOrDefault().Sort);

            return View(model);
        }
       

        //修改文章页面
        public  ActionResult ModifyArticle(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                return Content("路径不对");
            }
            var model = repositiory.Find<Article>(m => m.Identity == identity.ToLower());
            if (model ==null)
            {
                return Content("路径不对");
            }
            var article = new ViewAddArticleModel();
            article.Identity = identity;
            article.Content = model.Content;
            article.Title = model.Title;

            return View(article);
        }

        //修改文章提交
        [HttpPost]
        //解决富文本编辑器内容传输
        [ValidateInput(false)]
        public ActionResult ModifyArticle(ViewAddArticleModel model)
        {
            
            if (ModelState.IsValid)
            {
                var article = repositiory.Find<Article>(m => m.Identity == model.Identity.ToLower());
                if (article == null)
                {
                    return Content("路径不对");
                }
               
                article.UpdateTime = DateTime.Now;
                article.Title = model.Title;
                article.Content = model.Content;

                if(repositiory.Update(article))
                {
                    return RedirectToAction("Article", "Home", new { catalogidentity=article.Course.Catalog.Identity, courseidentity=article.Course.Identity,identity=article.Identity });   
                }
            } 
            return View();
        }
        //删除文章
        public ActionResult DeleteArticle(string identity)
        {
            var article = repositiory.Find<Article>(m=>m.Identity==identity);
            if (repositiory.Delete<Article>(article))
            {
                var course = repositiory.FindByID<Course>(article.CourseID);
                var catalog = repositiory.Find<Catalog>(m => m.ID == course.CatalogID);
                return RedirectToAction("Course","Home",new { catalogidentity=catalog.Identity, identity=course.Identity });
                
            }
            return Content("错误");
        }
        //统计信息
        public ActionResult Chart()
        {
            BamnContext b = new BamnContext();
            string key = "";
            var modellist = b.Article.Where(m => m.Title.Contains(key)&&m.Click>0).ToList();
            ViewBag.ArticleList = modellist;
            return View();
        }
        //显示上传页面
        public ActionResult Up()
        {
            return View();
        }
        //上传处理
        [HttpPost]
        public ActionResult Upload(FormCollection form)
        {
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                file = Request.Files[0];
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                //服务器上的UpLoadFile文件夹必须有读写权限
                //取得目标文件夹的路径
                string target = Server.MapPath("/") + ("/Files/");
                //取得文件名字
                string filename = file.FileName;
                //获取存储的目标地址
                string path = target + filename;
                file.SaveAs(path);
                //对此文件信息存到数据库
                Doc doc = new Doc { Name = filename, Path = target };
                BamnContext b = new BamnContext();
                b.Doc.Add(doc);
                b.SaveChanges();

            }
            return RedirectToAction("Document", "Home");
        }
    }
}