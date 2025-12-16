using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PddApp.Models
{
    public class Option
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Text")]
        public string Text { get; set; } = null!;
    }

    public class MatchingSetup
    {
        [JsonPropertyName("LeftColumnImages")]
        public List<string> LeftColumnImages { get; set; } = null!;

        [JsonPropertyName("RightColumnText")]
        public List<string> RightColumnText { get; set; } = null!;
    }

    public class Question
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Text")]
        public string Text { get; set; } = null!;

        [JsonPropertyName("ImagePath")]
        public string ImagePath { get; set; } = null!;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("MatchingSetup")]
        public MatchingSetup MatchingSetup { get; set; } = null!;

        [JsonPropertyName("Options")]
        public List<Option> Options { get; set; } = null!;

        [JsonPropertyName("CorrectAnswer")]
        public object CorrectAnswer { get; set; } = null!;

        [JsonPropertyName("Explanation")]
        public string Explanation { get; set; } = null!;
    }

    public class Ticket
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("Questions")]
        public List<Question> Questions { get; set; } = null!;
    }


}