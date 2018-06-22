using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System;
using System.Configuration;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string path)
        {
            //获取URL为空，地址不对
            if (string.IsNullOrEmpty(path))
            {
                return Content("路径不对");
            }
            //与指定URL匹配
            var loginPath = ConfigurationManager.AppSettings["LoginPath"];
            if (!path.Equals(loginPath))
            {
                return Content("路径不对");
            }

            ViewBag.Path = path;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(ViewLoginModel model,string path)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (string.IsNullOrEmpty(path))
                {
                    return Content("路径不对");
                }

                var loginPath = ConfigurationManager.AppSettings["LoginPath"];
                if (!path.Equals(loginPath))
                {
                    return Content("路径不对");
                }
                //从配置文件读取账号和MD5加密的密码
                var adminName = ConfigurationManager.AppSettings["AdminName"];
                var adminPassword = ConfigurationManager.AppSettings["AdminPassword"];
                if (adminName.Equals(model.Name) && adminPassword.Equals(GetMD5(model.Password)))
                {
                    //写登录票据
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                       // 版本号
                       1, 
                       // 与身份验证票关联的用户名
　                     adminName,
                        // cookie 的发出时间
　                     DateTime.Now, 
　                     // cookie 的到期日期
                       DateTime.Now.Add(FormsAuthentication.Timeout),
                       // 如果 cookie 是持久化的，为 true；否则为 false。 
　                     true,
                       // 将存储在 cookie 中的用户定义数据。roles是一个角色字符串数组 
　                     "administrator"); 
                        //加密 
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket); 

                   //存入cookie
                   HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,encryptedTicket);
                   Response.Cookies.Add(authCookie);
                   return Redirect("/");
                }
                else
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return View(model);
        }

        //MD5加密
        private string GetMD5(string input)
        {
            //加密对象获取,将输入内容加密后返回
            var md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                //X为     十六进制  
                //2为 每次都是两位数
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public void Logoff(string path)
        {
            //调用这个方法就可以注销了
            FormsAuthentication.SignOut();
            //退出后重定向到登录页，这两行是配合使用的。
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}