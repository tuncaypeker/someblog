using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.RytrMe.Dto
{
    public class Daily
    {
        public string display { get; set; }
        public int usage { get; set; }
    }

    public class Usage
    {
        public int used { get; set; }
        public int total { get; set; }
        public string percentage { get; set; }
    }

    public class PlanId
    {
        public string _id { get; set; }
        public bool isUnlimited { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public int units { get; set; }
    }

    public class Subscription
    {
        public string _id { get; set; }
        public string userId { get; set; }
        public PlanId planId { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class UsageSummaryDataDto
    {
        public List<Daily> daily { get; set; }
        public Usage usage { get; set; }
        public bool isExhausted { get; set; }
        public Subscription subscription { get; set; }
        public string couponSuggestion { get; set; }
        public int couponDiscount { get; set; }
        public bool manageBilling { get; set; }
        public bool isTeamAdmin { get; set; }
        public bool isTeamMember { get; set; }
        public int teamMemberCount { get; set; }
        public DateTime resetDate { get; set; }
    }

    public class UsageSummaryDto
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public UsageSummaryDataDto data { get; set; }
    }
}
