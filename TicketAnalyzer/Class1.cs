﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var searchResult = SearchResult.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using TicketAnalyzer;

    public static class RegExHelpers
    {
        public static readonly Regex RegexIssueLocationFinder = new Regex(
            @"[{]noformat[}]\s*(?<locations>[^]]*])\s*[{]noformat[}]",
            RegexOptions.Compiled | RegexOptions.Multiline);
    }
    ;
    public partial class SearchResult
    {
        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("startAt")]
        public long StartAt { get; set; }

        [JsonProperty("maxResults")]
        public long MaxResults { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("issues")]
        public Issue[] Issues { get; set; }
    }

    public partial class Issue
    {
        private Fields _fields;

        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get => _fields;
            set
            {
                _fields = value;
                if (value != null)
                {
                    if(!string.IsNullOrWhiteSpace(value.Description))
                    {
                        AnalyseCode(value.Description);
                    
                    }
                }
            }
        }

        public string IssueType =>
            Fields?.Issuetype == null
                ? string.Empty
                : Fields.Issuetype.Name;

        public string Assignee =>
            Fields?.Assignee == null
                ? string.Empty
                : Fields.Assignee.DisplayName;

        public IList<IssueLocation> IssueLocations { get; set; }

        private void AnalyseCode(string codeDescription)
        {
            var match = RegExHelpers.RegexIssueLocationFinder.Match(codeDescription);
            if (match.Success)
            {
                var locText = match.Groups["locations"].Value;
                IssueLocations = JsonConvert.DeserializeObject<List<IssueLocation>>(locText);
            }
        }
    }

    public partial class Fields
    {
        [JsonProperty("resolution")]
        public object Resolution { get; set; }

        [JsonProperty("issuelinks")]
        public object[] Issuelinks { get; set; }

        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty("subtasks")]
        public object[] Subtasks { get; set; }

        [JsonProperty("issuetype")]
        public Issuetype Issuetype { get; set; }

        [JsonProperty("customfield_11908")]
        public string ScmUrl { get; set; }

        [JsonProperty("customfield_11909")]
        public string Branch { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("labels")]
        public object[] Labels { get; set; }

        [JsonProperty("progress")]
        public Progress Progress { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public partial class Progress
    {
        [JsonProperty("progress")]
        public long ProgressProgress { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Assignee
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }



    public partial class Customfield1
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Id { get; set; }
    }

    public partial class Customfield23605
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("mandatory")]
        public bool Mandatory { get; set; }

        [JsonProperty("option")]
        public bool Option { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }
    }

    public partial class Issuetype
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subtask")]
        public bool Subtask { get; set; }

        [JsonProperty("avatarId")]
        public long AvatarId { get; set; }
    }


    public partial class Project
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("statusCategory")]
        public StatusCategory StatusCategory { get; set; }
    }

    public partial class StatusCategory
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("colorName")]
        public string ColorName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class SearchResult
    {
        public static SearchResult FromJson(string json) => JsonConvert.DeserializeObject<SearchResult>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SearchResult self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}
