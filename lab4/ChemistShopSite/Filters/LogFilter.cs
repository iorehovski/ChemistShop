using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ChemistShopSite.Filters
{
    public class LogFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Action: ");
            Console.ResetColor();
            Console.WriteLine(context.RouteData.Values["action"]);
        }
    }
}
