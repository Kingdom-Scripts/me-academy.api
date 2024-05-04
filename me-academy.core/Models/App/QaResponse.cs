namespace me_academy.core.Models.App;

public class QaResponse : BaseAppModel
{
    public int QaId { get; set; }
    public string? Answer { get; set; }
    public int? OptionId { get; set; }

    public int CreatedById { get; set; }

    public QuestionAndAnswer? Question { get; set; }
    public QaOption? Option { get; set; }
    public User? CreatedBy { get; set; }
}