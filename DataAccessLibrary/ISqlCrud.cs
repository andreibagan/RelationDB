using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public interface ISqlCrud
    {
        void CreateContact(FullContactModel contact);
        List<ContactModel> GetAllContacts();
        FullContactModel GetFullContactById(int id);
        void RemovePhoneNumberFromContact(int contactId, int phoneNumberId);
        void UpdateContactName(ContactModel contact);
    }
}