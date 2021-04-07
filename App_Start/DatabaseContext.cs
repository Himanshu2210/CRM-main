using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CRM.Models;
using System.Data;
using CRM;

namespace CRM.App_Start
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=CRMEntities")
        {

        }
        public DbSet<user_Model> User_Log { get; set; }

    }

}