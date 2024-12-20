﻿
using System.Text.RegularExpressions;

namespace RoutingParam.CustomConstraints
{
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if(!values.ContainsKey(routeKey))
            {
                return false;
            }
            Regex regex = new Regex("^(apr|jul|jan|oct)$");
            string? monthValue = Convert.ToString(values[routeKey]);
            if(regex.IsMatch(monthValue))
            {
                return true;
            }
            return false;
        }
    }
}
