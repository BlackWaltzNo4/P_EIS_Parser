using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Filter
{
    class BlackBox
    {
        private static string allowFilterPath = $"{Misc.Environment.filtersFolder}\\Allow.txt";
        private static string restrFilterPath = $"{Misc.Environment.filtersFolder}\\Restrict.txt";
        private static string okpd_allow_filter_path = $"{Misc.Environment.filtersFolder}\\OKPD_allow.txt";
        private static string okpd_restr_filter_path = $"{Misc.Environment.filtersFolder}\\OKPD_restrict.txt";
        private static string preliminary_filter_path = $"{Misc.Environment.filtersFolder}\\Preliminary.txt";

        private Filter allowanceFilter;
        private Filter restrictionFilter;
        private Filter okpd_allow_filter;
        private Filter okpd_restr_filter;

        //Reference
        //0 - allowanceFilterIsEnabled
        //1 - restrictionFilterIsEnabled
        //2 - okpd_allow_filterIsEnabled
        //3 - okpd_restr_filterIsEnabled

        public BlackBox()
        {
            this.allowanceFilter = new Filter(new FilterList(allowFilterPath).GetFilter());
            this.restrictionFilter = new Filter(new FilterList(restrFilterPath).GetFilter());
            this.okpd_allow_filter = new Filter(new FilterList(okpd_allow_filter_path).GetFilter());
            this.okpd_restr_filter = new Filter(new FilterList(okpd_restr_filter_path).GetFilter());
        }

        public bool[] GetFilterResult(string objectPurchase, string OKPD, string OKPDMain)
        {

            bool[] filterResult = new bool[4];

            if (allowanceFilter.FilterApplication(objectPurchase, Filter.FilterType.Text)) filterResult[0] = true;
            if (!restrictionFilter.FilterApplication(objectPurchase, Filter.FilterType.Text)) filterResult[1] = true;
            if (okpd_allow_filter.FilterApplication(OKPD, Filter.FilterType.OKPD2AllowFilter)) filterResult[2] = true;
            if (!okpd_restr_filter.FilterApplication(OKPD, Filter.FilterType.OKPD2RestrictFilter)) filterResult[3] = true;

            return filterResult;

        }

        public static bool GetPreliminaryFilterResult(string text)
        {
            Filter filter = new Filter(new FilterList(preliminary_filter_path).GetFilter());
            return filter.FilterApplication(text, Filter.FilterType.OKPD2RestrictFilter);
        }

    }
}
