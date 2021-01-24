using FTPApp_Server.XML.PurchasePlanPosition;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Update
    {

        public static void UpdatePositionInDatabase(string sql, SqlConnection connection, Position position, DateTime modified, int color, Entry.CheckResult result)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@publishYear", position.publishYear);
                Value.TryToAdd(command, "@totalSum", position.total);
                Value.TryToAdd(command, "@isPositionCancelled", position.positionCanceled);
                Value.TryToAdd(command, "@OKPDCode", position.OKPDCode);
                Value.TryToAdd(command, "@purchaseObjectInfo", position.purchaseObjectInfo);
                Value.TryToAdd(command, "@KVRCode", position.KVRcode);
                Value.TryToAdd(command, "@KBK", position.KBK);
                command.Parameters.AddWithValue("@Modified", modified);

                if (result == Entry.CheckResult.Cancelled) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(-)*");
                else if (result == Entry.CheckResult.M_KBK) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(K)*");
                else if (result == Entry.CheckResult.M_KVR) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(R)*");
                else if (result == Entry.CheckResult.M_OKPD) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(O)*");
                else if (result == Entry.CheckResult.M_publishYear) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(Y)*");
                else if (result == Entry.CheckResult.M_purchase) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*(~)*");
                else if (result == Entry.CheckResult.M_tot) command.Parameters.AddWithValue("@Commentary", $"$^-&{modified.ToString()}-&*($)*");

                command.ExecuteNonQuery();
            }
        }
    }
}
