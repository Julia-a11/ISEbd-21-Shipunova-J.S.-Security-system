using Microsoft.EntityFrameworkCore;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemDatabaseImplement
{
    public class SecuritySystemDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SecuritySystemDatabase;Integrated Security=True;MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Component> Components { set; get; }

        public virtual DbSet<Secure> Secures { set; get; }

        public virtual DbSet<SecureComponent> SecureComponents { set; get; }
        
        public virtual DbSet<Order> Orders { set; get; }

        public virtual DbSet<Client> Clients { set; get; }

        public virtual DbSet<Implementer> Implementers { set; get; }
    }
}
