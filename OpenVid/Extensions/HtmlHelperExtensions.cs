using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.RegularExpressions;

namespace OpenVid.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IDisposable BeginPanel(this IHtmlHelper htmlHelper, string title, bool minimisable = false, bool expandable = false, bool reloadable = false, bool deletable = false)
        {
            htmlHelper.ViewContext.Writer.Write($@"<div id=""{Regex.Replace(title, @"\s+", "")}"" class=""panel panel-inverse"">");
            htmlHelper.ViewContext.Writer.Write(PanelHeader(title, minimisable, expandable, reloadable, deletable));
            htmlHelper.PanelBody();
            return new DisposableHtmlHelper(htmlHelper.EndPanel);
        }

        public static IDisposable BeginPanel(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write(@"<div class=""panel panel-inverse"">");
            htmlHelper.PanelBody();
            return new DisposableHtmlHelper(htmlHelper.EndPanel);
        }
        public static IDisposable BeginTable(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write(@"<div class=""panel panel-inverse"">");
            htmlHelper.PanelBody("table-responsive");
            return new DisposableHtmlHelper(htmlHelper.EndPanel);
        }

        private static TagBuilder PanelHeader(string title, bool minimisable = false, bool expandable = false, bool reloadable = false, bool deletable = false)
        {
            TagBuilder panelHeading = new TagBuilder("div");
            panelHeading.AddCssClass("panel-heading");

            TagBuilder panelTitle = new TagBuilder("h4");
            panelTitle.AddCssClass("panel-title");
            panelTitle.InnerHtml.Append(title);

            TagBuilder panelButtons = new TagBuilder("div");
            panelButtons.AddCssClass("panel-heading-btn");
            if (expandable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-default\" data-toggle=\"tooltip\" title=\"Maximise\" data-click=\"panel-expand\"><i class=\"fa fa-expand\"></i></a>");
            if (reloadable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-success\" data-toggle=\"tooltip\" title=\"Refresh\" data-click=\"panel-reload\"><i class=\"fa fa-redo\"></i></a>");
            if (minimisable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-warning\" data-toggle=\"tooltip\" title=\"Minimise\" data-click=\"panel-collapse\"><i class=\"fa fa-minus\"></i></a>");
            if (deletable)
                panelButtons.InnerHtml.AppendHtml("<a href=\"javascript:;\" class=\"btn btn-xs btn-icon btn-circle btn-danger\" data-toggle=\"tooltip\" title=\"Close\" data-click=\"panel-remove\"><i class=\"fa fa-times\"></i></a>");

            panelHeading.InnerHtml.AppendHtml(panelTitle);
            panelHeading.InnerHtml.AppendHtml(panelButtons);

            return panelHeading;
        }

        private static void PanelBody(this IHtmlHelper htmlHelper, string bodyClass = "panel-body")
        {
            htmlHelper.ViewContext.Writer.Write(@$"<div class=""{bodyClass}"">");
        }

        private static void EndPanel(this IHtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div></div>");
        }
    }

    internal class DisposableHtmlHelper : IDisposable
    {
        private readonly Action _end;

        public DisposableHtmlHelper(Action end)
        {
            _end = end;
        }

        public void Dispose()
        {
            _end();
        }
    }
}
