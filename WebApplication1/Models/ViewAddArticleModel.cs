using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class ViewAddArticleModel
    {
        public int CourseID { set; get; }

        [Required]
        [Display(Name = "标题")]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "内容")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "标识符")]
        [Remote("CheckIdentityForArticle","Manage","",ErrorMessage = "标识符已存在", AdditionalFields = "CourseID")]
        [StringLength(30)]
        [RegularExpression("^[_a-zA-Z0-9]+$", ErrorMessage = "分类名只能是字母数字下划线组成")]
        public string Identity { get; set; }

        [Required]
        [Display(Name = "上一篇")]
        public int LastSort { get; set; }

    }
}