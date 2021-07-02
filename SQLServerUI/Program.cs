using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SQLServerUI
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlCrud sql = new SqlCrud(GetConnectionString());

            ReadAllContacts(sql);

            ReadContact(sql, 1);

            CreateNewContact(sql);

            UpdateContact(sql);

            RemovePhoneNumberFromContact(sql, 1, 2);

            Console.ReadLine();
        }

        private static void CreateNewContact(SqlCrud sql)
        {
            FullContactModel user = new FullContactModel
            {
                Info = new ContactModel
                {
                    FirstName = "Andrei",
                    LastName = "Bagan"
                },
            };

            user.EmailAddresses.Add(new EmailAddressModel
            {
                EmailAddress = "andrei.bagan1@mail.ru"
            });
            user.EmailAddresses.Add(new EmailAddressModel
            {
                EmailAddress = "andrei.bagan2@mail.ru"
            });
            user.EmailAddresses.Add(new EmailAddressModel
            {
                EmailAddress = "andrei.bagang@mail.ru"
            });

            user.PhoneNumbers.Add(new PhoneNumberModel
            {
                PhoneNumber = "+375292334228"
            });
            user.PhoneNumbers.Add(new PhoneNumberModel
            {
                PhoneNumber = "+375333622646"
            });

            sql.CreateContact(user);
        }
        private static void ReadAllContacts(SqlCrud sql)
        {
            var contacts = sql.GetAllContacts();

            foreach (var contact in contacts)
            {
                Console.WriteLine($"{contact.Id}: {contact.FirstName} {contact.LastName}");
            }
        }

        private static void ReadContact(SqlCrud sql, int contactId)
        {
            var contact = sql.GetFullContactById(contactId);

            Console.WriteLine($"{contact.Info.Id}: {contact.Info.FirstName} {contact.Info.LastName}");
        }

        private static void UpdateContact(SqlCrud sql)
        {
            ContactModel contact = new ContactModel
            {
                Id = 1,
                FirstName = "Andrew",
                LastName = "Bagan"
            };
            sql.UpdateContactName(contact);
        }
        private static void RemovePhoneNumberFromContact(SqlCrud sql, int contactId, int phoneNumberId)
        {
            sql.RemovePhoneNumberFromContact(contactId, phoneNumberId);
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = string.Empty;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
