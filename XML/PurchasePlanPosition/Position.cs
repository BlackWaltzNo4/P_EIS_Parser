using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML.PurchasePlanPosition
{
    class Position
    {
        public Element region = new Element("region");
        public Element fullName = new Element("fullName");
        public Element INN = new Element("INN");
        public Element phone = new Element("phone");
        public Element email = new Element("email");
        public Element URL = new Element("URL");

        public Element isPositionSpecial = new Element("isPositionSpecial");
        public Element FZ = new Element("FZ");
        public Element positionNumber = new Element("positionNumber");
        public Element extNumber = new Element("extNumber");
        public Element IKZ = new Element("IKZ");

        //public Element publishYear = new Element("publishYear");
        public Int32 publishYear;

        public Element IKU = new Element("IKU");
        public Element purchaseNumber = new Element("purchaseNumber");
        public Element OKPDCode = new Element("OKPDCode");
        public Element OKPDName = new Element("OKPDName");
        public Element KVRcode = new Element("KVRcode");
        public Element KVRname = new Element("KVRname");
        public Element purchaseObjectInfo = new Element("purchaseObjectInfo");
        public Element publicDiscussion = new Element("publicDiscussion");

        //public Element positionCanceled = new Element("positionCanceled");
        public bool positionCanceled;

        //public Element total = new Element("total");
        public float total;

        public Element KBK = new Element("KBK");
        public Element KBKtotal = new Element("KBKtotal");

        public bool[] filters = new bool[4];

        //Первые две цифры ОКПД для фильтра
        public Element OKPDCodeMain = new Element("OKPDCodeMain");

        public string GetPurchaseText()
        {

            return purchaseObjectInfo.GetText();

        }

        public string GetOKPD()
        {

            return OKPDCode.GetText();

        }

        public string GetOKPDMain()
        {

            return OKPDCodeMain.GetText();

        }

        public string GetPurchaseNumber()
        {
            return positionNumber.GetText();
        }
    }
}
