using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FTPApp_Server.XML
{
    class Element
    {

        private string text = String.Empty;

        private string name;
        public Element(string name) => this.name = name;
        public void SetElement(XmlNode node) => text = node.InnerText;

        public void SetElement(string text) => this.text = text;

        public void AddToElement(XmlNode node)
        {
            if (String.IsNullOrEmpty(text)) text += node.InnerText;
            else
            {
                text += ", " + node.InnerText;
            }
        }

        public void GetElement() => Console.WriteLine(string.Format("{0}: {1}", name, text));

        public string GetText()
        {
            if (String.IsNullOrEmpty(text)) return null;
            return text;
        }

    }
}
