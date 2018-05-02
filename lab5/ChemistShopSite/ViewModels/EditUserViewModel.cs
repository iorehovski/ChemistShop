using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChemistShopSite.ViewModels
{
    public class EditUserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }
    }
}
