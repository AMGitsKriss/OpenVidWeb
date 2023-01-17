using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;

namespace OpenVid.Extensions
{
    public abstract class OpenVidController : Controller
    {
        public RedirectToActionResult RedirectToAction(PageLocation page)
        {
            return RedirectToAction(page.Action, page.Controller, new { page.Area });
        }

        public RedirectToActionResult RedirectToAction(PageLocation page, object routeValues)
        {
            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in routeValues.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(routeValues, null);
            }
            result["Area"] = page.Area;

            return RedirectToAction(page.Action, page.Controller, result);
        }
    }
}
