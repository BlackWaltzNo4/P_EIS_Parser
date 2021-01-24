using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML.PurchasePlanPosition
{
    class Nodes
    {

        public void GetNodes(XmlDocument document, out XmlNode xPurchasePlan, out XmlNode xCommonInfo, out XmlNode xPositions, out XmlNode xSPPositions)
        {
            XmlNode xRoot = document.DocumentElement;

            xPurchasePlan = xRoot.ChildNodes[0];
            xCommonInfo = null;
            xPositions = null;
            xSPPositions = null;

            if (xPurchasePlan != null)
            {
                foreach (XmlNode node in xPurchasePlan)
                {
                    if (node.Name == "ns5:commonInfo") xCommonInfo = node;
                    if (node.Name == "ns5:positions") xPositions = node;
                    if (node.Name == "ns5:specialPurchasePositions") xSPPositions = node;
                }
            }
        }

    }
}
