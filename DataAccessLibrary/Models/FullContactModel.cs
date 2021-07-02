using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
    public class FullContactModel
    {
        public ContactModel Info { get; set; }
        public List<EmailAddressModel> EmailAddresses { get; set; } = new List<EmailAddressModel>();
        public List<PhoneNumberModel> PhoneNumbers { get; set; } = new List<PhoneNumberModel>();
    }
}
