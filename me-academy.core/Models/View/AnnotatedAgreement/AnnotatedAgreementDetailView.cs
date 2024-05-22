using me_academy.core.Models.Utilities;
using me_academy.core.Models.View;
using me_academy.core.Models.View.AnnotatedAgreement;

public class AnnotatedAgreementDetailView : AnnotatedAgreementView
{
    public int DocumentId { get; set; }
    public required string Description { get; set; }
    public List<string> Tags { get; set; } = new();

    public DocumentView? Document { get; set; }
    public ReferencedUserView? CreatedBy { get; set; }
    public ReferencedUserView? UpdatedBy { get; set; }
}
