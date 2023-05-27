using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ContactDM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } // Required
        public string LastName { get; set; } // Required
        public int GenderValue { get; set; } // Required
        public string GenderText { get; set; } // Required
        public string MobilePhone { get; set; } // Required
        public string AddressLine { get; set; } // Required
        public string AddressCity { get; set; } // Required
        public string AddressState { get; set; } // Required
        public string AddressPostalCode { get; set; } // Required
        public string AddressCountry { get; set; } // Required
        public int ContactTypeValue { get; set; } // Required
        public string ContactTypeText { get; set; } // Required
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public bool ForgotPassword { get; set; }

    }
}
