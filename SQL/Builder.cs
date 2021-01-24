using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Builder
    {
        public static SqlConnectionStringBuilder Line()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"tcp:192.168.3.169,49172";
            builder.IntegratedSecurity = false;
            builder.UserID = @"User";
            builder.Password = "Gfhjkm22";
            builder.InitialCatalog = "January";
            builder.ConnectTimeout = 60;
            return builder;
        }
    }
}
