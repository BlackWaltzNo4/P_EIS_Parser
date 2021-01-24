using FTPApp_Server.XML.PurchasePlanPosition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML
{
    class Data
    {
        public static Document GetDocumentData(string path, string region, out (ushort positions, ushort errors) counter)
        {
            //counters
            ushort positions = 0;
            ushort errors = 0;
            counter = (positions, errors);

            Document document = new Document(region);
            XmlDocument xDocument = new XmlDocument();

            using (var fileStream =  new FileStream(path, FileMode.Open))
            {

                xDocument.Load(fileStream);

                new Nodes().GetNodes(xDocument, out XmlNode xPurchasePlan, out XmlNode xCommonInfo, out XmlNode xPositions, out XmlNode xSPPositions);

                try
                {
                    Parsing.SetLink(ref document, xPurchasePlan);
                }
                catch { }

                try
                {
                    Parsing.SetCommonInfo(ref document, xCommonInfo);
                }
                catch { }

                if (xPositions != null)
                {
                    try
                    {
                        document.positionsList.AddRange(Parsing.GetPositions44FZ(ref document, xPositions, out (ushort positions, ushort errors) counters));
                        counter.positions += counters.positions;
                        counter.errors += counters.errors;
                    }
                    catch { }
                }

                if (xSPPositions != null)
                {
                    try
                    {
                        document.positionsList.AddRange(Parsing.GetPositions44FZSP(ref document, xSPPositions, out (ushort positions, ushort errors) counters));
                        counter.positions += counters.positions;
                        counter.errors += counters.errors;
                    }
                    catch { }
                }
            }

            return document;

        }
    }
}
