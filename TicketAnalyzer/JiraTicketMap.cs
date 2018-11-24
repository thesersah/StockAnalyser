namespace TicketAnalyzer
{
    using CsvHelper.Configuration;

    public sealed class JiraTicketMap : ClassMap<JiraTicket>
    {
        public JiraTicketMap()
        {
            AutoMap();
            Map(m => m.IssueKey).Name("Issue key");
            Map(m => m.IssueId).Name("Issue id");
            Map(m => m.Assignee).Default("unassigned");
            Map(m => m.ScmUrl).Name("Custom field (scm_url)");
            Map(m => m.EffectedLoc).Name("Custom field (Effected LoC)");
            Map(m => m.CodeReviewTicketUrl).Name("Custom field (Code Review Ticket URL)");
        }
    }
}