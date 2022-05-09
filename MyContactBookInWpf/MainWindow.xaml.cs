using MyContactBookInWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyContactBookInWpf
{
    public partial class MainWindow : Window
    {
        private MyContactBookDb Db;

        public List<Contact> Contacts = new List<Contact>();
        public Contact SelectedContact = new Contact();

        public string SearchString = "";

        public MainWindow()
        {
            InitializeComponent();
            UpdateBinding();

            Db = new MyContactBookDb();

            // Retrieve contacts from DB.
            _ = UpdateContactsAsync();
        }

        // TODO: Remove after implementing MVVM.
        private void UpdateBinding()
        {
            lblId.DataContext = SelectedContact;
            txtFirstName.DataContext = SelectedContact;
            txtLastName.DataContext = SelectedContact;
            txtPhoneNumber.DataContext = SelectedContact;
        }

        private async Task UpdateContactsAsync()
        {
            if (SearchString == "")
                lsvContacts.ItemsSource = await Db.GetContacts();
            else
                lsvContacts.ItemsSource = await Db.GetContactsByFirstName(SearchString);
        }

        #region Events
        private void lsvContacts_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedContact = (Contact)lsvContacts.SelectedItem;
            UpdateBinding();
        }

        private void btnNewContact_Click(object sender, RoutedEventArgs e)
        {
            SelectedContact = new Contact();
            UpdateBinding();
        }

        private void btnSaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedContact.Id == 0)
            {
                // Create new
                if (Db.NewContact(SelectedContact))
                {
                    // Success msg
                    Console.WriteLine("********** New Contact added successfully. **********");
                }
                else
                {
                    // Unsuccessfull msg
                    Console.WriteLine("********** Error: New Contact UNSUCCESSFUL. **********");
                }
            }
            else
            {
                // Update
                if (Db.UpdateContact(SelectedContact))
                {
                    // Success msg
                    Console.WriteLine("********** Contact updated successfully. **********");
                }
                else
                {
                    // Unsuccessfull msg
                    Console.WriteLine("********** Error: Contact update UNSUCCESSFUL. **********");
                }
            }

            _ = UpdateContactsAsync();
        }

        private void btnDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContact.Id != 0)
            {
                if(Db.DeleteContact(SelectedContact))
                    Console.WriteLine("********** Contact deleted successfully. **********");
                else
                    Console.WriteLine("********** Error: Contact NOT deleted. **********");
            }
            else
                Console.WriteLine("********** Error: Please select valid Contact to delete. **********");

            _ = UpdateContactsAsync();
            SelectedContact = new Contact();
            UpdateBinding();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchString = txtSearch.Text;
            _ = UpdateContactsAsync();
        }
        #endregion
    }
}
