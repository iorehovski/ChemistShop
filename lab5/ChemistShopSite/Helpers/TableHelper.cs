using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChemistShopSite.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ChemistShopSite.Helpers
{
    public static class TableHelper
    {
        public static HtmlString CreateMedicamentsList(this IHtmlHelper html, IEnumerable<Medicament> medicaments)
        {
            string result = "";
            foreach (Medicament item in medicaments)
            {
                result += "<tr>";
                result += $"<td>{item.MedicamentName}</td>";
                result += $"<td>{item.Manufacturer}</td>";
                result += $"<td>{item.Storage}</td>";
                result += $"<td class=\"act\"><form action=\"/Home/Delete/ "+item.Id + "\" method=\"post\"><a class=\"btn btn-sm btn-primary\" href=\"/Home/Edit/" + item.Id + "\">Изменить</a><button type = \"submit\" class=\"btn btn-sm btn-danger\">Удалить</button></form></td>";
                result += "</tr>";
            }
            return new HtmlString(result);
        }
    }
}
