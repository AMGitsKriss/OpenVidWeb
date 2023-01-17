using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Dynamic;

namespace OpenVid.Extensions
{
    public static class SiteMap
    {
        public static PageLocation Home { get; set; } = new PageLocation("Home", "Index");
        public static PageLocation Login { get; set; } = new PageLocation("Login", "Index");
        public static PageLocation Logout { get; set; } = new PageLocation("Logout", "Index");
        public static PageLocation Register { get; set; } = new PageLocation("Register", "Index");
        public static PageLocation Playback_Play { get; set; } = new PageLocation("Playback", "Play", "Index");
        public static PageLocation Playback_Search { get; set; } = new PageLocation("Playback", "Search", "Index");
        public static PageLocation Playback_Update { get; set; } = new PageLocation("Playback", "Update", "Index");
        public static PageLocation Playback_VideoList { get; set; } = new PageLocation("Playback", "VideoList", "Index");
        public static PageLocation Playback_TagGetTags { get; set; } = new PageLocation("Playback", "Tag", "GetTags");
        public static PageLocation Playback_Thumbnail { get; set; } = new PageLocation("Playback", "Thumbnail", "Index");
        public static PageLocation Playback_Thumbnail_Auto { get; set; } = new PageLocation("Playback", "Thumbnail", "Auto");
        public static PageLocation Playback_Thumbnail_Manual { get; set; } = new PageLocation("Playback", "Thumbnail", "Manual");
        public static PageLocation VideoManagement_Destroy { get; set; } = new PageLocation("VideoManagement", "Destroy", "Index");
        public static PageLocation FlagDelete { get; set; } = new PageLocation("VideoManagement", "FlagDelete", "Index"); 
        public static PageLocation Catalog_Import { get; set; } = new PageLocation("Catalog", "Import", "Index");
        public static PageLocation Catalog_Import_Upload { get; set; } = new PageLocation("Catalog", "Import", "Upload");
        public static PageLocation Catalog_Import_Queue { get; set; } = new PageLocation("Catalog", "Import", "Queue");
        public static PageLocation Catalog_Curation { get; set; } = new PageLocation("Catalog", "Curation", "Index");
        public static PageLocation Catalog_ImportPreprocessed { get; set; } = new PageLocation("Catalog", "ImportPreprocessed", "Index");
        public static PageLocation Catalog_ImportSave { get; set; } = new PageLocation("Catalog", "ImportPreprocessed", "Save");
        public static PageLocation Catalog_Edit { get; set; } = new PageLocation("Catalog", "Edit", "Index");
        public static PageLocation Catalog_Edit_VideoDetails { get; set; } = new PageLocation("Catalog", "Edit", "VideoDetails");
        public static PageLocation Catalog_Edit_DeleteThumbnail { get; set; } = new PageLocation("Catalog", "Edit", "DeleteThumbnail");
        public static PageLocation Tags_Management { get; set; } = new PageLocation("Tags", "Management", "Index");
        public static PageLocation Account_Management { get; set; } = new PageLocation("Account", "Management", "Index");


        public static List<PageLocation> AdminMenu = new List<PageLocation>()
        {
            Account_Management,
            Catalog_Import,
            Catalog_ImportPreprocessed,
            Catalog_Curation,
            Tags_Management
        };

        public static string Action(this IUrlHelper urlHelper, PageLocation page)
        {
            return urlHelper.Action(page.Action, page.Controller, new { page.Area });
        }
        public static string Action(this IUrlHelper urlHelper, PageLocation page, object routeValues)
        {
            return urlHelper.Action(page.Action, page.Controller, MergeRouteValues(page, routeValues));
        }

        public static IHtmlContent ActionLink(this IHtmlHelper helper, string linkText, PageLocation page)
        {
            return helper.ActionLink(linkText, page.Action, page.Controller, page);
        }

        public static IHtmlContent ActionLink(this IHtmlHelper helper, string linkText, PageLocation page, object routeValues)
        {
            return helper.ActionLink(linkText, page.Action, page.Controller, MergeRouteValues(page, routeValues));
        }

        private static object MergeRouteValues(PageLocation page, object routeValues)
        {
            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in routeValues.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(routeValues, null);
            }
            result["Area"] = page.Area;

            return result;
        }
    }
}
