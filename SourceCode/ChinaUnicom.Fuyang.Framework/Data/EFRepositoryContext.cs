using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public class EFRepositoryContext : RepositoryContext
    {
        EFDbContext dbContext;

        protected override DbContext Context
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = new EFDbContext("ChinaUnicom.Fuyang");
                    dbContext.Database.CommandTimeout = 3600;
                }

                return dbContext;
            }
        }
    }
}
