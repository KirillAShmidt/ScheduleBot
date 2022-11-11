using ParsingLogic;
using ParcingLogic.Extentions;
using ParcingLogic.Models;
using Fizzler.Systems.HtmlAgilityPack;

namespace ParcingLogic
{
    internal class PgupsScedule
    {
        const string MAIN_SCHEDULE_PAGE = "https://rasp.pgups.ru/schedule/group";

        public HttpClient HtmlClient { get; }

        internal PgupsScedule()
        {
            HtmlClient = new HttpClient(); 
        }

        private IEnumerable<string> GetGroupLinks()
        {
            var mainPageContent = new PageContent(HtmlClient, new Uri(MAIN_SCHEDULE_PAGE));

            return mainPageContent.Document.FindGroupLinks();
        }

        private IEnumerable<Subject> GetGroupSubjects(Uri url)
        {
            var linkContent = new PageContent(HtmlClient, url);

            var subjects = new List<Subject>();

            foreach (var day in linkContent.Document.QuerySelectorAll(".m-table"))
            {
                if (day.QuerySelector(".kt-font-dark") != null)
                {
                    foreach (var item in day.QuerySelectorAll("tr"))
                    {
                        subjects.Add(new Subject
                        {
                            DayOfWeek = day.QuerySelector(".kt-font-dark").ChildNodes[2].OuterHtml.Substring(6).Trim(),
                            GroupName = linkContent.Document.QuerySelector(".kt-portlet__head-title").FirstChild.OuterHtml.Trim().Substring(7),
                            SubjectNumber = Int32.Parse(item.QuerySelector(".align-middle").FirstChild.OuterHtml.Substring(0, 1)),
                            Time = item.QuerySelector("span").FirstChild.OuterHtml.Substring(0, 5).Trim(),
                            Room = item.QuerySelector(".mt-2") != null ? item.QuerySelector(".mt-2").QuerySelector("a").ChildNodes[2].OuterHtml.Substring(6).Trim() : "--",
                            SubjectName = item.QuerySelector(".mr-1").FirstChild.OuterHtml.Trim()
                        });
                    }
                }
            }

            return subjects;
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();

            foreach(var link in GetGroupLinks())
            {
                subjects.AddRange(GetGroupSubjects(new Uri(link)));
            }

            return subjects;
        }
    }
}
