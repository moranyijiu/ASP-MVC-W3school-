using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ViewLoginModel
    {
        [Required]
        [Display(Name = "账号")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}