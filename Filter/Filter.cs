using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Filter
{
    class Filter
    {

        string[] filterListString;

        List<int> filterListInt;

        string[] tagList;

        string[] list;

        string[] separator;

        string[] spaceSeparator;

        public enum FilterType
        {
            Text,
            OKPD2AllowFilter,
            OKPD2RestrictFilter
        }

        enum Rule
        {
            Strong,
            Weak
        }

        public Filter(string filter)
        {

            separator = new String[1];

            separator[0] = @", ";

            spaceSeparator = new String[1];

            spaceSeparator[0] = @" ";

            filterListString = filter.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        }

        public Filter(List<int> filter)
        {

            separator = new String[1];

            separator[0] = @", ";

            spaceSeparator = new String[1];

            spaceSeparator[0] = @" ";

            //filterListString = filter.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            filterListInt = filter;

        }

        public bool FilterApplication(string _string, FilterType type)
        {

            if (String.IsNullOrEmpty(_string)) return false;

            foreach (string substring in filterListString)
            {

                //String _substring = substring.Replace("*", "");

                tagList = substring.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

                Int16 counter = 0;

                list = _string.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < list.Length; i++)
                {

                    string s = list[i];

                    if (type == FilterType.Text) s = s.Replace(",", "").Replace("(", "").Replace(")", "").Replace(".", "");
                    else if (type == FilterType.OKPD2AllowFilter) s = s.Replace(",", "");
                    else if (type == FilterType.OKPD2RestrictFilter) s = s.Substring(0, s.IndexOf("."));
                    //else s = s;

                    list[i] = s;

                }

                foreach (String _tag in tagList)
                {

                    if (type == FilterType.Text)
                    {
                        if (_tag.EndsWith("*"))
                        {

                            if (NonStrictFilter(_tag.Replace("*", ""), list)) counter++;

                        }
                        else
                        {

                            if (StrictFilter(_tag, list, Rule.Strong)) counter++;

                        }
                    }
                    else if (type == FilterType.OKPD2AllowFilter)
                    {
                        if (StrictFilter(_tag, list, Rule.Strong)) counter++;
                    }
                    else if (type == FilterType.OKPD2RestrictFilter)
                    {
                        if (StrictFilter(_tag, list, Rule.Weak)) counter++;
                    }

                }

                if (type == FilterType.Text) { if (counter > 0) return true; }
                else if (type == FilterType.OKPD2AllowFilter) { if (counter > 0) return true; }
                else if (type == FilterType.OKPD2RestrictFilter) { if (counter > 0) return true; }

            }

            return false;

        }

        private bool NonStrictFilter(String substring, String[] _list)
        {

            Int16 counter = 0;

            foreach (String singleWord in _list)
            {

                if (singleWord.Contains(substring, StringComparison.OrdinalIgnoreCase)) counter++;

                //if (String.Equals(singleWord, substring, StringComparison.OrdinalIgnoreCase)) counter++;

            }

            //if (counter >= _list.Length) return true;

            if (counter > 0) return true;

            return false;

        }

        private bool StrictFilter(String substring, String[] _list, Rule rule)
        {

            Int16 counter = 0;

            foreach (String singleWord in _list)
            {

                //if (singleWord.Contains(substring, StringComparison.OrdinalIgnoreCase)) counter++;

                if (String.Equals(singleWord, substring, StringComparison.OrdinalIgnoreCase)) counter++;

            }

            if (rule == Rule.Strong) { if (counter >= _list.Length) return true; }
            else if (rule == Rule.Weak) if (counter > 0) return true;

            return false;

        }

    }
}

