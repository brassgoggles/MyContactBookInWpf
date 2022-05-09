using MyContactBookInWpf.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactBookInWpf
{
    public class MyContactBookInWpfContext : DbContext
    {
        public MyContactBookInWpfContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
