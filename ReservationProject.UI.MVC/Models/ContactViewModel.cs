﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReservationProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "** Name is Required **")]
        public string Name { get; set; }

        [Required(ErrorMessage = "** Email is Required **")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "** Message is Required **")]
        [UIHint("MultiLineText")]
        public string Message { get; set; }
    }
}