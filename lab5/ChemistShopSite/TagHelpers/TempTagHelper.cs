using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChemistShopSite.TagHelpers
{
    public class TempTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("id", "getTime");
            output.Content.SetContent("get request");
        }
    }
}
