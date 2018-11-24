namespace TicketAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Windows;
    using CsvHelper;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    using QuickType;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SearchResult searchResult;

        public MainWindow()
        {
            InitializeComponent();
        }

        public delegate void UpdateTextCallback(string message);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseWithCsvHelper();

            AppendLog("Done");
        }

        private void ParseWithCsvHelper()
        {
            var csvFile = ticketsSource.Text;

            var regex = new Regex(@"[{]noformat[}]\s*(?<locations>[^]]*])\s*[{]noformat[}]", RegexOptions.Compiled | RegexOptions.Multiline);

            using (var fileStream = File.OpenRead(csvFile))
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = "|";
                csv.Configuration.RegisterClassMap<JiraTicketMap>();

                var tickets = csv.GetRecords<JiraTicket>().ToList();

                foreach (var ticket in tickets)
                {
                    if (!string.IsNullOrWhiteSpace(ticket.Description))
                    {
                        var match = regex.Match(ticket.Description);
                        if (match.Success)
                        {
                            var locText = match.Groups["locations"].Value;
                            ticket.IssueLocations = JsonConvert.DeserializeObject<List<IssueLocation>>(locText);
                        }
                    }
                }

            }
        }

        private void AppendLog(string message, bool newline = true)
        {
            logTextBox.Dispatcher.Invoke(
                new UpdateTextCallback(
                    msg =>
                    {
                        logTextBox.AppendText(msg);
                        if (newline)
                        {
                            logTextBox.AppendText(Environment.NewLine);
                        }

                        logTextBox.ScrollToEnd();
                    }),
                message);
        }

        private async void ButtonGet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var cookie = File.ReadAllText("Cookie.txt");
                var url = @"https://jira.devfactory.com/rest/api/2/search?jql=";
                var encodedUri = Uri.EscapeUriString(ticketsSource.Text);
                url = $"{url}{encodedUri}";
                using (var handler = new HttpClientHandler()
                {
                    UseCookies = false,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                })
                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
                    var result = httpClient.GetAsync(url).Result;
                    result.EnsureSuccessStatusCode();
                    var contentAsString = await result.Content.ReadAsStringAsync();
                    searchResult = JsonConvert.DeserializeObject<SearchResult>(contentAsString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (searchResult?.Issues == null)
            {
                return;
            }

            var issuesByFile = searchResult.Issues.SelectMany(x => x.IssueLocations.Select(
                        l => new
                        {
                            Id = Guid.NewGuid(),
                            l.FileName,
                            l.StartLine,
                            l.EndLine,
                            x.Key,
                            x.Assignee,
                            x.IssueType,
                            EffectedLoc = l.EndLine - l.StartLine + 1
                        })).ToList();

            var joined = from a in issuesByFile
                         join b in issuesByFile on a.Key equals b.Key into ps
                         from b in ps.DefaultIfEmpty()
                         let ee = a.IssueType == "Duplicate Code" ? b : null
                         where ee == null || a.Id != ee.Id
                         orderby a.FileName, a.StartLine
                         select new
                         {
                             FileName1 = a.FileName,
                             StartLine1 = a.StartLine,
                             EndLine1 = a.EndLine,
                             EffectLoc1 = a.EffectedLoc,
                             a.Key,
                             a.IssueType,
                             a.Assignee,
                             FileName2 = ee?.FileName,
                             StartLine2 = ee?.StartLine,
                             EndLine2 = ee?.EndLine,
                             EffectLoc2 = ee?.EffectedLoc,
                         };

            var saveFileDialog1 =new SaveFileDialog { Filter = "Csv File|*.csv", Title = "Save an Csv File" };
            if (saveFileDialog1.ShowDialog() == true)
            {
                using (var fileOut = File.OpenWrite(saveFileDialog1.FileName))
                using (var writer = new StreamWriter(fileOut))
                using (var csvWriter = new CsvWriter(writer))
                {
                    csvWriter.WriteRecords(joined);
                }
            }
        }
    }
}
