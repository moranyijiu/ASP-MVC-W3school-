using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class ViewAddCatalogModel
    {
        [Required]
        [Display(Name = "名称")]
        [StringLength(25)]
        [RegularExpression("^[\u4e00-\u9fa5_a-zA-Z0-9]+$",ErrorMessage = "分类名只能是字母数字汉字下划线组成")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "标识符")]
        [Remote("CheckIdentity","Manage","",ErrorMessage = "标识符已存在")]
        [StringLength(16)]
        [RegularExpression("^[_a-zA-Z0-9]+$", ErrorMessage = "分类名只能是字母数字下划线组成")]
        public string Identity { get; set; }

        [Required]
        [Display(Name = "上一个分类")]
        public int LastSort { get; set; }

    }
}