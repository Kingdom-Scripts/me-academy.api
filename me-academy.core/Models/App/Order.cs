namespace me_academy.core.Models.App;

public class Order : BaseAppModel
{
    public int UserId { get; set; }
    
    public required string ItemType { get; set; }
    public int? CourseId { get; set; }
    public int? SeriesId { get; set; }
    public int? SmeHubId { get; set; }
    public int? AnnotatedAgreementId { get; set; }

    public required string BillingAddress { get; set; }
    public int DurationId { get; set; }
    public decimal DiscountApplied { get; set; } = 0m;
    public decimal ItemAmount { get; set; }
    public decimal TotalAmount { get; set; }

    public int? UpdateById { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }
    public string? Authorization_Url { get; set; }
    public string? Access_Code { get; set; }
    public string? Reference { get; set; }

  public Discount? Discount { get; set; }
}
