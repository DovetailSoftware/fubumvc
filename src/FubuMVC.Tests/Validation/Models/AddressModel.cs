﻿using FubuMVC.Core.Validation;

namespace FubuMVC.Tests.Validation.Models
{
    public class AddressModel
    {
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StateOrProvince { get; set; }
        [Required]
        public string Country { get; set; }
        [Required, MaximumStringLength(5)]
        public string PostalCode { get; set; }
    }
}