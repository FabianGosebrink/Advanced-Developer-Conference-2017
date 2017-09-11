using System;
using System.Linq;

namespace DotnetcliWebApi.Models
{
    public class QueryParameters
    {
        private const int maxPageCount = 5;
        public int Page { get; set; } = 1;

        private int _pageCount = 5;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }

        public bool HasQuery { get { return !String.IsNullOrEmpty(Query); } }
        public string Query { get; set; }

        public string OrderBy { get; set; } = "Name";
        public bool Descending
        {
            get
            {
                if (!String.IsNullOrEmpty(OrderBy))
                {
                    return OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
                }
                return false;
            }
        }
    }
}