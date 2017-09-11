using System;
using DotnetcliWebApi.Models;

namespace DotnetcliWebApi.Helpers
{
    public static class QueryParametersExtensions
    {
        public static bool HasPrevious(this QueryParameters queryParameters)
        {
            return (queryParameters.Page > 1);
        }

        public static bool HasNext(this QueryParameters queryParameters, int totalCount)
        {
            return (queryParameters.Page < (int)Math.Ceiling(totalCount / (double)queryParameters.PageCount));
        }

        public static double GetTotalPages(this QueryParameters queryParameters, int totalCount)
        {
            return Math.Ceiling(totalCount / (double)queryParameters.PageCount);
        }
    }
}