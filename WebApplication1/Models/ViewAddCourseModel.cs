using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class ViewAddCourseModel
    {
        public int CatalogID { set; get; }

        [Required]
        [Display(Name = "名称")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "内容")]
        [StringLength(250,MinimumLength = 60)]
        public string Intro { get; set; }

        [Required]
        [Display(Name = "幅面")]
        public string Thumb { set; get; }

        [Required]
        [Display(Name = "标识符")]
        [Remote("CheckIdentityForCourse","Manage","",ErrorMessage = "标识符已存在", AdditionalFields = "CatalogID")]
        [StringLength(30)]
        [RegularExpression("^[_a-zA-Z0-9]+$", ErrorMessage = "分类名只能是字母数字下划线组成")]
        public string Identity { get; set; }

        [Required]
        [Display(Name = "上一个课程")]
        public int LastSort { get; set; }

    }
}