using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Value
    {

        public static void TryToAdd(SqlCommand command, string parameter, XML.Element element)
        {
            if (element == null || element.GetText() == null)
            {
                command.Parameters.AddWithValue(parameter, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameter, element.GetText());
            }
        }

        public static void TryToAdd(SqlCommand command, string parameter, int element)
        {
            if (element == 0)
            {
                command.Parameters.AddWithValue(parameter, 0);
            }
            else
            {
                command.Parameters.AddWithValue(parameter, element);
            }
        }

        public static void TryToAdd(SqlCommand command, string parameter, double element)
        {
            if (element == 0)
            {
                command.Parameters.AddWithValue(parameter, 0);
            }
            else
            {
                command.Parameters.AddWithValue(parameter, element);
            }
        }

        public static void TryToAdd(SqlCommand command, string parameter, bool element)
        {
            if (element == true)
            {
                command.Parameters.AddWithValue(parameter, 1);
            }
            else
            {
                command.Parameters.AddWithValue(parameter, 0);
            }
        }

    }
}
