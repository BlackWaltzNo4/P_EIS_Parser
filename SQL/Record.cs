using FluentFTP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Record
    {

        public static void InsertPositionsToDatabase(XML.PurchasePlanPosition.Position position, string database, DateTime modified)
        {
            using (SqlConnection connection = new SqlConnection(Builder.Line().ConnectionString))
            {
                connection.Open();

                try
                {
                    Insert.InsertPositionIntoDatabase(Query.InsertIntoPositionInDatabase(database), connection, position, modified, 0);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void UpdatetPositionInDatabase(XML.PurchasePlanPosition.Position position, DateTime modified, string databaseName, string positionNumber, Entry.CheckResult result)
        {
            using (SqlConnection connection = new SqlConnection(Builder.Line().ConnectionString))
            {
                connection.Open();

                try
                {
                    Update.UpdatePositionInDatabase(Query.UpdatePosition(databaseName, positionNumber), connection, position, modified, 0, result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
