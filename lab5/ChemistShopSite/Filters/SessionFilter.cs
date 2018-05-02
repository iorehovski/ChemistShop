using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Newtonsoft.Json;

namespace ChemistShopSite.Filters
{
    public class SessionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Count > 0)
            {
                string keyStr = "";

                foreach (var item in context.ActionArguments.Keys)
                {
                    keyStr += item;
                }

                string str = "";

                foreach (var item in context.ActionArguments.Values)
                {
                    str += JsonConvert.SerializeObject(item);
                }

                switch (keyStr)
                {
                    case "medicament": context.HttpContext.Session.Set("medSession", Encoding.UTF8.GetBytes(str)); break;
                    case "reception": context.HttpContext.Session.Set("recSession", Encoding.UTF8.GetBytes(str)); break;
                    case "consumption": context.HttpContext.Session.Set("conSession", Encoding.UTF8.GetBytes(str)); break;
                    case "MedicamentName": context.HttpContext.Session.Set("medNameFilterSession", Encoding.UTF8.GetBytes(str)); break;
                    case "medSortOrder": context.HttpContext.Session.Set("medSortOrderSession", Encoding.UTF8.GetBytes(str)); break;
                    case "recSortOrder": context.HttpContext.Session.Set("recSortOrderSession", Encoding.UTF8.GetBytes(str)); break;
                    case "conSortOrder": context.HttpContext.Session.Set("conSortOrderSession", Encoding.UTF8.GetBytes(str)); break;
                }  
            }
        }
    }
}
