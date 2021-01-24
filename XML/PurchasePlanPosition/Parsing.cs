using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML.PurchasePlanPosition
{
    class Parsing
    {

        //enum FZ { "44", 44SP, 223}

        public static List<Position> GetPositions44FZ(ref Document document, XmlNode xPosition, out (ushort positions, ushort errors) counters)
        {
            //counters
            ushort positions = 0;
            ushort errors = 0;

            List<Position> currentList = new List<Position>();

            foreach (XmlNode pNode in xPosition)
            {
                try
                {
                    positions++;
                    currentList.Add(SetPosition44FZ(ref document, pNode));
                }
                catch 
                {
                    errors++;
                }
            }

            counters = (positions, errors);

            return currentList;
        }

        public static List<Position> GetPositions44FZSP(ref Document document, XmlNode xPosition, out (ushort positions, ushort errors) counters)
        {
            //counters
            ushort positions = 0;
            ushort errors = 0;

            List<Position> currentList = new List<Position>();

            foreach (XmlNode pNode in xPosition)
            {
                try
                {
                    positions++;
                    currentList.Add(SetPosition44FZSP(ref document, pNode));
                }
                catch 
                {
                    errors++;
                }
            }

            counters = (positions, errors);

            return currentList;
        }

        //
        //
        //

        private static Position SetPosition44FZ(ref Document document, XmlNode xPosition)
        {
            if (xPosition == null) throw new Exception("XmlNode is empty");

            Position position = new Position();

            position.region.SetElement(document.commonInfo.region.GetText());

            position.fullName.SetElement(document.commonInfo.fullName.GetText());
            position.INN.SetElement(document.commonInfo.INN.GetText());
            position.phone.SetElement(document.commonInfo.phone.GetText());
            position.email.SetElement(document.commonInfo.email.GetText());
            position.URL.SetElement(document.link.URL.GetText());

            foreach (XmlNode rNode in xPosition)
            {

                position.isPositionSpecial.SetElement("false");
                position.FZ.SetElement("44FZ");

                if (rNode.Name == "ns5:commonInfo")
                {
                    foreach (XmlNode tNode in rNode)
                    {
                        if (tNode.Name == "ns5:positionNumber") position.positionNumber.SetElement(tNode);
                        if (tNode.Name == "ns5:extNumber") position.extNumber.SetElement(tNode);
                        if (tNode.Name == "ns5:IKZ") position.IKZ.SetElement(tNode);
                        if (tNode.Name == "ns5:publishYear") Int32.TryParse(tNode.InnerText, out position.publishYear);
                        if (tNode.Name == "ns5:IKU") position.IKU.SetElement(tNode);
                        if (tNode.Name == "ns5:purchaseNumber") position.purchaseNumber.SetElement(tNode);
                        if (tNode.Name == "ns5:OKPD2Info")
                        {
                            foreach (XmlNode yNode in tNode)
                            {
                                if (yNode.Name == "ns4:OKPDCode") position.OKPDCode.SetElement(yNode);
                                if (yNode.Name == "ns4:OKPDName") position.OKPDName.SetElement(yNode);
                            }
                        }
                        if (tNode.Name == "ns5:undefined")
                        {
                            foreach (XmlNode fNode in tNode)
                            {
                                if (fNode.Name == "ns5:OKPD2s")
                                {
                                    foreach (XmlNode yNode in fNode)
                                    {
                                        if (yNode.Name == "ns5:OKPD2Info")
                                        {
                                            foreach (XmlNode gNode in yNode)
                                            {
                                                if (gNode.Name == "ns4:OKPDCode") position.OKPDCode.AddToElement(gNode);
                                                if (gNode.Name == "ns4:OKPDName") position.OKPDName.AddToElement(gNode);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Получение первых двух цифр ОКПД кода
                        string o = position.OKPDCode.GetText();
                        if (o != null)
                        {
                            string t = o.Substring(0, o.IndexOf("."));
                            position.OKPDCodeMain.SetElement(t);
                        }
                        //

                        if (tNode.Name == "ns5:KVRInfo")
                        {
                            foreach (XmlNode uNode in tNode)
                            {
                                if (uNode.Name == "ns5:KVR")
                                {
                                    foreach (XmlNode iNode in uNode)
                                    {
                                        if (iNode.Name == "ns4:code") position.KVRcode.SetElement(iNode);
                                        if (iNode.Name == "ns4:name") position.KVRname.SetElement(iNode);
                                    }
                                }
                            }
                        }
                        if (tNode.Name == "ns5:purchaseObjectInfo") position.purchaseObjectInfo.SetElement(tNode);
                        if (tNode.Name == "ns5:publicDiscussion") position.publicDiscussion.SetElement(tNode);
                        if (tNode.Name == "ns5:positionCanceled") bool.TryParse(tNode.InnerText, out position.positionCanceled);
                    }
                }
                if (rNode.Name == "ns5:financeInfo")
                {
                    foreach (XmlNode oNode in rNode)
                    {
                        if (oNode.Name == "ns5:total")
                        {
                            //double test;
                            //double.TryParse(oNode.InnerText, out test);
                            //test = double.Parse(oNode.InnerText, NumberStyles.Number, CultureInfo.InvariantCulture);
                            float.TryParse(oNode.InnerText, NumberStyles.Number, CultureInfo.InvariantCulture, out position.total);
                        }
                        if (oNode.Name == "ns5:KBKsInfo")
                        {
                            foreach (XmlNode aNode in oNode)
                            {
                                if (aNode.Name == "ns5:KBKInfo")
                                {
                                    foreach (XmlNode sNode in aNode)
                                    {
                                        if (sNode.Name == "ns5:KBK") position.KBK.SetElement(sNode);
                                        if (sNode.Name == "ns5:KBKYearsInfo")
                                        {
                                            foreach (XmlNode dNode in sNode)
                                            {
                                                if (dNode.Name == "ns5:total") position.KBKtotal.SetElement(dNode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            position.filters = new Filter.BlackBox().GetFilterResult(position.purchaseObjectInfo.GetText(), position.GetOKPD(), position.GetOKPDMain());

            return position;

        }

        private static Position SetPosition44FZSP(ref Document document, XmlNode xPosition)
        {
            if (xPosition == null) throw new Exception("XmlNode is empty");

            Position position = new Position();

            position.region.SetElement(document.commonInfo.region.GetText());

            position.fullName.SetElement(document.commonInfo.fullName.GetText());
            position.INN.SetElement(document.commonInfo.INN.GetText());
            position.phone.SetElement(document.commonInfo.phone.GetText());
            position.email.SetElement(document.commonInfo.email.GetText());
            position.URL.SetElement(document.link.URL.GetText());

            position.isPositionSpecial.SetElement("true");
            position.FZ.SetElement("44FZ_S");

            foreach (XmlNode rNode in xPosition)
            {
                if (rNode.Name == "ns5:positionNumber") position.positionNumber.SetElement(rNode);
                if (rNode.Name == "ns5:IKZ") position.IKZ.SetElement(rNode);
                if (rNode.Name == "ns5:publishYear") Int32.TryParse(rNode.InnerText, out position.publishYear);
                if (rNode.Name == "ns5:IKU") position.IKU.SetElement(rNode);
                if (rNode.Name == "ns5:purchaseNumber") position.purchaseNumber.SetElement(rNode);

                if (rNode.Name == "ns5:KVRInfo")
                {
                    foreach (XmlNode uNode in rNode)
                    {
                        if (uNode.Name == "ns5:KVR")
                        {
                            foreach (XmlNode iNode in uNode)
                            {
                                if (iNode.Name == "ns4:code") position.KVRcode.SetElement(iNode);
                                if (iNode.Name == "ns4:name") position.KVRname.SetElement(iNode);
                            }
                        }
                    }
                }

                if (rNode.Name == "ns5:financeInfo")
                {
                    foreach (XmlNode oNode in rNode)
                    {
                        if (oNode.Name == "ns5:total") float.TryParse(oNode.InnerText, NumberStyles.Number, CultureInfo.InvariantCulture, out position.total);
                    }
                }

            }

            position.filters = new Filter.BlackBox().GetFilterResult(position.purchaseObjectInfo.GetText(), position.GetOKPD(), position.GetOKPDMain());

            return position;

        }

        public static void SetCommonInfo(ref Document document, XmlNode xCommonInfo)
        {
            if (xCommonInfo == null) throw new Exception("XmlNode is empty");

            document.commonInfo.region.SetElement(document.GetRegion());

            foreach (XmlNode rNode in xCommonInfo)
            {
                if (rNode.Name == "ns5:planYear") document.commonInfo.planYear.SetElement(rNode);
                if (rNode.Name == "ns5:planPeriod")
                {
                    foreach (XmlNode pNode in rNode)
                    {
                        if (pNode.Name == "ns5:firstYear") document.commonInfo.firstYear.SetElement(pNode);
                        if (pNode.Name == "ns5:secondYear") document.commonInfo.secondYear.SetElement(pNode);
                    }
                }
                if (rNode.Name == "ns5:createDate") document.commonInfo.createDate.SetElement(rNode);

                if (rNode.Name == "ns5:customerInfo")
                {
                    foreach (XmlNode pNode in rNode)
                    {
                        if (pNode.Name == "ns5:regNum") document.commonInfo.regNum.SetElement(pNode);
                        if (pNode.Name == "ns5:fullName") document.commonInfo.fullName.SetElement(pNode);
                        if (pNode.Name == "ns5:INN") document.commonInfo.INN.SetElement(pNode);
                        if (pNode.Name == "ns5:KPP") document.commonInfo.KPP.SetElement(pNode);
                        if (pNode.Name == "ns5:factAddress") document.commonInfo.factAddress.SetElement(pNode);
                        if (pNode.Name == "ns5:phone") document.commonInfo.phone.SetElement(pNode);
                        if (pNode.Name == "ns5:email") document.commonInfo.email.SetElement(pNode);
                    }
                }
            }
        }

        public static void SetLink(ref Document document, XmlNode xTenderPlan)
        {
            foreach (XmlNode rNode in xTenderPlan)
            {
                if (rNode.Name == "ns5:planNumber")
                {
                    document.link.planNumber.SetElement(rNode);
                    document.link.URL.SetElement(string.Format(@"https://zakupki.gov.ru/epz/orderplan/pg2020/general-info.html?plan-number={0}", document.link.planNumber.GetText()));
                }
            }
        }

    }
}
