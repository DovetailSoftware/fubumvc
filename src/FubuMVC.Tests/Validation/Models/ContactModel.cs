using System.Collections.Generic;
using FubuMVC.Core.Validation;

namespace FubuMVC.Tests.Validation.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            Addresses = new List<AddressModel>();
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [CollectionLength(1)]
        public IEnumerable<AddressModel> Addresses { get; set; }
    }
}