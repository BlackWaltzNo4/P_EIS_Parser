using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML.PurchasePlanPosition
{
    class CommonInfo
    {

        public CommonInfo() {}

        public Element region = new Element("region");
        public Element planYear = new Element("planYear");
        public Element firstYear = new Element("firstYear");
        public Element secondYear = new Element("secondYear");
        public Element createDate = new Element("createDate");

        //CustomerInfo
        public Element regNum = new Element("regNum");
        public Element fullName = new Element("fullName");
        public Element INN = new Element("INN");
        public Element KPP = new Element("KPP");
        public Element factAddress = new Element("factAddress");
        public Element phone = new Element("phone");
        public Element email = new Element("email");

    }
}
