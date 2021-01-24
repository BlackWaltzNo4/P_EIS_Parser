using FluentFTP;
using FTPApp_Server.XML.PurchasePlanPosition;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Insert
    {

        public static void InsertPositionIntoDatabase(string sql, SqlConnection connection, Position position, DateTime modified, int color)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FZ", position.FZ.GetText());

                //command.Parameters.AddWithValue("@isPositionCancelled", Boolean.TryParse(position.positionCanceled.ToString(), out bool result));

                Value.TryToAdd(command, "@isPositionCancelled", position.positionCanceled);
                Value.TryToAdd(command, "@publishYear", position.publishYear);
                Value.TryToAdd(command, "@purchaseObjectInfo", position.purchaseObjectInfo);
                Value.TryToAdd(command, "@totalSum", position.total);
                Value.TryToAdd(command, "@positionNumber", position.positionNumber);
                Value.TryToAdd(command, "@region", position.region);
                Value.TryToAdd(command, "@fullName", position.fullName);
                Value.TryToAdd(command, "@INN", position.INN);
                Value.TryToAdd(command, "@phone", position.phone);
                Value.TryToAdd(command, "@email", position.email);
                Value.TryToAdd(command, "@URL", position.URL);
                Value.TryToAdd(command, "@IKZ", position.IKZ);
                Value.TryToAdd(command, "@OKPDCode", position.OKPDCode);
                Value.TryToAdd(command, "@OKPDName", position.OKPDName);
                Value.TryToAdd(command, "@KBK", position.KBK);
                Value.TryToAdd(command, "@KBKTotal", position.KBKtotal);
                Value.TryToAdd(command, "@KVRCode", position.KVRcode);
                Value.TryToAdd(command, "@KVRName", position.KVRname);

                command.Parameters.AddWithValue("@Modified", modified);
                command.Parameters.AddWithValue("@filterAllow", position.filters[0]);
                command.Parameters.AddWithValue("@filterRestr", position.filters[1]);
                command.Parameters.AddWithValue("@filterOKPDAllow", position.filters[2]);
                command.Parameters.AddWithValue("@filterOKPDRestr", position.filters[3]);
                command.Parameters.AddWithValue("@Color", color);
                command.Parameters.AddWithValue("@Commentary", $"-&{modified.ToString()}-&*(+)*");
                command.Parameters.AddWithValue("@FirstEntry", modified);
                command.Parameters.AddWithValue("@DatabaseName", DateTimeFormatInfo.InvariantInfo.GetMonthName(modified.Month));

                command.ExecuteNonQuery();
            }
        }

    }
}
