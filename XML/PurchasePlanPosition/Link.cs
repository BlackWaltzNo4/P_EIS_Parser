using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML.PurchasePlanPosition
{
    class Link
    {

        public Element URL = new Element("URL");

        public Link() { }

        public Element planNumber = new Element("planNumber");

        public string GetPlanNumber()
        {
            return planNumber.GetText();
        }
    }
}
