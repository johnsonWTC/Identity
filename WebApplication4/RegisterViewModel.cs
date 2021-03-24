using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50,MinimumLength =5)]
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(50,MinimumLength =5)]
        public string ConfirmPassWord { get; set; }
    }
}
