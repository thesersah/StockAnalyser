using System;

namespace TicketAnalyzer
{
    using System.Collections.Generic;

    public class JiraTicket
    {
        public string IssueKey { get; set; }
        public long IssueId { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Resolved { get; set; }
        public string Assignee { get; set; }
        public string ScmUrl { get; set; }
        public string EffectedLoc { get; set; }
        public string CodeReviewTicketUrl { get; set; }
        public string Description { get; set; }

        public IList<IssueLocation> IssueLocations { get; set; }
    }
}
