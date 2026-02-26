
using System.Text.Json;
using System.Text.Json.Serialization;
namespace QuizApp
{
  internal class Questions
  {
    public List<QuizItem> userAnswerItems = [];

    public static List<QuizItem> GetQuizItems(string quizSetJSON)
    {
      try
      {
        using var stream = FileSystem.OpenAppPackageFileAsync(quizSetJSON).GetAwaiter().GetResult();
        using var reader = new StreamReader(stream);
        string jsonString = reader.ReadToEnd();

        var quizItems = JsonSerializer.Deserialize<List<QuizItem>>(jsonString);
        return quizItems ?? new List<QuizItem>();
      }
      catch (Exception e)
      {
        System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
        return new List<QuizItem>();
      }
    }
  }

  public class QuizItem
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("options")]
    public QuizOptions Options { get; set; } = new QuizOptions();

    [JsonPropertyName("answer")]
    public string Answer { get; set; } = string.Empty;
  }

  public class QuizOptions
  {
    [JsonPropertyName("A")]
    public string A { get; set; } = string.Empty;

    [JsonPropertyName("B")]
    public string B { get; set; } = string.Empty;

    [JsonPropertyName("C")]
    public string C { get; set; } = string.Empty;

    [JsonPropertyName("D")]
    public string D { get; set; } = string.Empty;
  }

}