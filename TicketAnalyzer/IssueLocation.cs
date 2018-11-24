namespace TicketAnalyzer
{
    public class IssueLocation
    {
        public string FileName { get; set; }

        public int StartLine { get; set; }

        public int StartColumn { get; set; }

        public int EndLine { get; set; }
    }
}