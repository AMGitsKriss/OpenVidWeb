namespace OpenVid.Extensions
{
    public class PageLocation
    {
        public PageLocation(string areaName, string controllerName, string actionName)
        {
            Area = areaName;
            Controller = controllerName;
            Action = actionName;
        }
        public PageLocation(string controllerName, string actionName)
        {
            Controller = controllerName;
            Action = actionName;
        }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
