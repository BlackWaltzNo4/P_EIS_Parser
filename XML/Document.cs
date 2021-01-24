using FTPApp_Server.XML.PurchasePlanPosition;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML
{
    class Document
    {

        public Link link;
        public CommonInfo commonInfo;
        public List<Position> positionsList;

        private string region;

        public Document(string region)
        {

            this.region = region;

            link = new Link();

            commonInfo = new CommonInfo();

            positionsList = new List<Position>();

        }        

        private string SQLInsert(StringBuilder sb)
        {
            string s;

            sb.Append("INSERT INTO PositionData (FZ, publishYear, purchaseObjectInfo, totalSum, positionNumber, region, ");
            sb.Append("fullName, INN, phone, email, URL, IKZ, OKPDCode, OKPDName, KBK, KBKTotal, KVRCode, KVRName) ");
            sb.Append("VALUES (@FZ, @publishYear, @purchaseObjectInfo, @totalSum, @positionNumber, @region, ");
            sb.Append("@fullName, @INN, @phone, @email, @URL, @IKZ, @OKPDCode, @OKPDName, @KBK, @KBKTotal, @KVRCode, @KVRName)");

            s = sb.ToString();
            sb.Clear();

            return s;
        }
    

        public int GetPositionsCount()
        {
            return positionsList.Count;
        }

        public string GetRegion()
        {
            return region;
        }

    }
        
}
