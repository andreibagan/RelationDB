using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLibrary
{
    public class MySqlCrud : ISqlCrud
    {
        private readonly string _connectionString;
        private MySqlDataAccess db = new MySqlDataAccess();

        public MySqlCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ContactModel> GetAllContacts()
        {
            string sql = "select Id, FirstName, LastName from Contacts;";

            return db.LoadData<ContactModel, dynamic>(sql, new { }, _connectionString);
        }

        public FullContactModel GetFullContactById(int id)
        {
            FullContactModel output = new FullContactModel();
            string sql = "select Id, FirstName, LastName from Contacts where Id = @id;";

            output.Info = db.LoadData<ContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.Info == null)
            {
                return null;
            }

            sql = @"select e.*
                    from EmailAddresses e
                    inner join ContactEmail ce on ce.EmailAddressId = e.Id
                    where ce.ContactId = @id";

            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            sql = @"select p.*
                    from PhoneNumbers p
                    inner join ContactPhoneNumbers cp on cp.PhoneNumberId = p.Id
                    where cp.ContactId = @id";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

            return output;
        }

        public void CreateContact(FullContactModel contact)
        {
            string sql = "insert into Contacts (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql, new { FirstName = contact.Info.FirstName, LastName = contact.Info.LastName }, _connectionString);

            sql = "select Id from Contacts where FirstName = @Firstname and LastName = @LastName;";
            int contactId = db.LoadData<IdLookupModel, dynamic>(sql, new { FirstName = contact.Info.FirstName, LastName = contact.Info.LastName }, _connectionString).First().Id;

            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                if (phoneNumber.Id == 0)
                {
                    sql = "insert into PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
                    db.SaveData(sql, new { PhoneNumber = phoneNumber.PhoneNumber }, _connectionString);

                    sql = "select Id from PhoneNumbers where PhoneNumber = @PhoneNumber";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { PhoneNumber = phoneNumber.PhoneNumber }, _connectionString).First().Id;
                }

                sql = "insert into ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
                db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
            }

            foreach (var email in contact.EmailAddresses)
            {
                if (email.Id == 0)
                {
                    sql = "insert into EmailAddresses (EmailAddress) values (@EmailAddress);";
                    db.SaveData(sql, new { EmailAddress = email.EmailAddress }, _connectionString);

                    sql = "select Id from EmailAddresses where EmailAddress = @EmailAddress";
                    email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { EmailAddress = email.EmailAddress }, _connectionString).First().Id;
                }

                sql = "insert into ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
                db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
            }
        }
        public void UpdateContactName(ContactModel contact)
        {
            string sql = "update Contacts set FirstName = @FirstName, LastName = @LastName where Id = @Id";
            db.SaveData(sql, contact, _connectionString);
        }
        public void RemovePhoneNumberFromContact(int contactId, int phoneNumberId)
        {
            string sql = "select Id, ContactId, PhoneNumberId from ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId;";
            var links = db.LoadData<ContactPhoneNumberModel, dynamic>(sql, new { PhoneNumberId = phoneNumberId }, _connectionString);

            sql = "delete from ContactPhoneNumbers where PhoneNumberId = @PhoneNumberId and ContactId = @ContactId;";
            db.SaveData(sql, new { phoneNumberId = phoneNumberId, ContactId = contactId }, _connectionString);

            if (links.Count == 1)
            {
                sql = "delete from PhoneNumbers where Id = @PhoneNumberId;";
                db.SaveData(sql, new { PhoneNumberId = phoneNumberId }, _connectionString);
            }
        }
    }
}
