using dbs.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace dbs.Context
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        {
        }

        //[DbFunction(Name = "SoundEx", IsBuiltIn = true, IsNullable = false)]
        //public static string CallAccountCreation(string input)
        //{
        //    throw new NotImplementedException();
        //}

        public DbSet<AccountResponse> Account { get; set; }
        public DbSet<AccountObj> AccountObjs { get; set; }
    }
}
