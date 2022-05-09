using MyContactBookInWpf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactBookInWpf
{
    public class MyContactBookDb
    {
        private static string connectionString;

        public MyContactBookDb()
        {
            try
            {
                connectionString = "Server=tcp:turtleshellsoftwaretestserver.database.windows.net,1433;" +
                    "Initial Catalog=MyContactBookDb;Persist Security Info=False;User ID=Turtleshell;" +
                    "Password={your password here};MultipleActiveResultSets=False;Encrypt=True;" +
                    "TrustServerCertificate=False;Connection Timeout=30;";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"********** MyContactBookDB Exception: {ex} **********");
            }
        }

        public async Task<List<Contact>> GetContacts()
        {
            using (MyContactBookInWpfContext context = new MyContactBookInWpfContext(connectionString))
            {
                List<Contact> contacts = new List<Contact>();

                try
                {
                    contacts = await context.Contacts.ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"********** GetContacts Exception: {ex} **********");
                }

                return contacts;
            }
        }

        public async Task<List<Contact>> GetContactsByFirstName(string searchString)
        {
            using (MyContactBookInWpfContext context = new MyContactBookInWpfContext(connectionString))
            {
                List<Contact> contacts = new List<Contact>();

                try
                {
                    contacts = await context.Contacts.Where(c => c.FirstName.Contains(searchString)).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"********** GetContacts Exception: {ex} **********");
                }

                return contacts;
            }
        }

        public bool NewContact(Contact contact)
        {
            using (MyContactBookInWpfContext context = new MyContactBookInWpfContext(connectionString))
            {
                try
                {
                    context.Contacts.Add(contact);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"********** NewContact Exception: {ex} **********");
                    return false;
                }

                return true;
            }
        }

        public bool UpdateContact(Contact contact)
        {
            using (MyContactBookInWpfContext context = new MyContactBookInWpfContext(connectionString))
            {
                try
                {
                    Contact contactToUpdate = context.Contacts.Where(c => c.Id == contact.Id).FirstOrDefault();
                    context.Entry(contactToUpdate).CurrentValues.SetValues(contact);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"********** UpdateContact Exception: {ex} **********");
                    return false;
                }

                return true;
            }
        }

        public bool DeleteContact(Contact contact)
        {
            using (MyContactBookInWpfContext context = new MyContactBookInWpfContext(connectionString))
            {
                try
                {
                    // Delete by Id to avoid any updates that may have been passed through to the contact object.
                    Contact contactToDelete = context.Contacts.Where(c => c.Id == contact.Id).FirstOrDefault();
                    context.Contacts.Remove(contactToDelete);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"********** DeleteContact Exception: {ex} **********");
                    return false;
                }

                return true;
            }
        }
    }
}
