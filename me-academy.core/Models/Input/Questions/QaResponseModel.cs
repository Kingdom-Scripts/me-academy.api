namespace me_academy.core.Models.Input.Questions;

public class QuestionResponseModel
{
    public int QuestionId { get; set; }
    public string? Answer { get; set; }
    public int? OptionId { get; set; }
}