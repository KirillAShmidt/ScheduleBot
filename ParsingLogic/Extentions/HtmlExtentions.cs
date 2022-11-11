using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace ParcingLogic.Extentions
{
    internal static class HtmlExtentions
    {
        public static IEnumerable<string> FindGroupLinks(this HtmlNode document)
        {
            var groupLinks = document.QuerySelector(".tab-pane").QuerySelectorAll("a");

            var stringLinks = new List<string>();

            foreach (var link in groupLinks)
                stringLinks.Add(link.Attributes["href"].Value.ToString());

            return stringLinks;
        }
    }
}
