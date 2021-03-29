using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoreFrontJordan.MVC.UI.Models
{
    public class ContactViewModel
    {

        [Required(ErrorMessage = "* Name is required.")]
        public string Name { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "* Message is required.")]
        [UIHint("MultiLineText")]

        public string Message { get; set; }

        [Required(ErrorMessage = "* Email is required.")]
        [EmailAddress(ErrorMessage = "* Please make sure your Email is in the proper formatting.")]
        [Display(Name = "Your Email")]
        public string EmailAddress { get; set; }

    }
}