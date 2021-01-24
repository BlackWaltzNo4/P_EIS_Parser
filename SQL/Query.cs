using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.SQL
{
    class Query
    {

        public static string InsertIntoPositionInDatabase(string database)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"INSERT INTO {database}..Data (FZ, isPositionCancelled, publishYear, purchaseObjectInfo, totalSum, positionNumber, region, ");
            stringBuilder.Append($"fullName, INN, phone, email, URL, IKZ, OKPDCode, OKPDName, KBK, KBKTotal, KVRCode, KVRName, ");
            stringBuilder.Append($"Modified, filterAllow, filterRestr, filterOKPDAllow, filterOKPDRestr, Color, Commentary, FirstEntry, DatabaseName) ");
            stringBuilder.Append($"VALUES (@FZ, @isPositionCancelled, @publishYear, @purchaseObjectInfo, @totalSum, @positionNumber, @region, ");
            stringBuilder.Append($"@fullName, @INN, @phone, @email, @URL, @IKZ, @OKPDCode, @OKPDName, @KBK, @KBKTotal, @KVRCode, @KVRName, ");
            stringBuilder.Append($"@Modified, @filterAllow, @filterRestr, @filterOKPDAllow, @filterOKPDRestr, @Color, @Commentary, @FirstEntry, @DatabaseName)");

            return stringBuilder.ToString();
        }

        public static string UpdatePosition(string databaseName, string positionNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string parameters = "publishYear = @publishYear, totalSum = @totalSum, " +
                                "isPositionCancelled = @isPositionCancelled, OKPDCode = @OKPDCode, " +
                                "purchaseObjectInfo = @purchaseObjectInfo, KVRCode = @KVRCode, KBK = @KBK, " +
                                "Modified = @Modified, Commentary = Commentary + @Commentary, Status = 0";

            stringBuilder.Append($"UPDATE {databaseName}..Data ");
            stringBuilder.Append($"SET {parameters} ");
            stringBuilder.Append($"WHERE positionNumber={positionNumber} ");

            return stringBuilder.ToString();
        }

        public static string GetValueForFilterQuery()
        {
            StringBuilder sb = new StringBuilder();

            string parameters = "ID, purchaseObjectInfo, OKPDCode";

            sb.Append($"SELECT {GetParametersWithPrefix(parameters, "U")} FROM (");
            sb.Append($"SELECT {parameters} FROM July..Data ");
            //sb.Append($"UNION ALL SELECT U.COUNT(*) FROM June..June ");
            sb.Append($") AS U ");

            return sb.ToString();
        }

        public static string GetCountOfRowsWithIdentifier()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT COUNT(*) FROM (");
            sb.Append("SELECT positionNumber FROM January..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM February..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM March..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM April..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM May..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM June..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM July..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM August..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM September..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM October..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM November..Data ");
            sb.Append($"UNION ALL SELECT positionNumber FROM December..Data ");
            sb.Append(") AS U ");
            sb.Append("WHERE U.positionNumber = @positionNumber");

            return sb.ToString();
        }

        public static string GetRoWithIdentifier()
        {
            StringBuilder sb = new StringBuilder();

            string parameters = "publishYear, totalSum, isPositionCancelled, OKPDCode, Modified, FirstEntry, DatabaseName, positionNumber, purchaseObjectInfo, KVRCode, KBK";

            sb.Append($"SELECT {GetParametersWithPrefix(parameters, "U")} FROM (");
            sb.Append($"SELECT {parameters} FROM January..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM February..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM March..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM April..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM May..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM June..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM July..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM August..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM September..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM October..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM November..Data ");
            sb.Append($"UNION ALL SELECT {parameters} FROM December..Data ");
            sb.Append($") AS U ");
            sb.Append($"WHERE U.positionNumber = @positionNumber");

            return sb.ToString();
        }

        public static string GetParametersWithPrefix(string parameters, string prefix)
        {
            string[] parametersArray = parameters.Split(' ');

            string parametersLine = "";

            foreach (string s in parametersArray) parametersLine += $"{prefix}.{s} ";

            return parametersLine;
        }

    }
}
