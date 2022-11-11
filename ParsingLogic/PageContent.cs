using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace ParsingLogic
{
    public class PageContent
    {
        public HtmlNode Document { get; set; }

        private HttpClient _httpClient;


        public PageContent(HttpClient httpClient, Uri url)
        { 
            _httpClient = httpClient;

            var html = new HtmlDocument();
            html.LoadHtml(GetPageAsync(url).Result);

            Document = html.DocumentNode;
        }

        private async Task<string> GetPageAsync(Uri url)
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}
