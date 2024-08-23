namespace me_academy.core.Models.Input.AnnotatedAgreements;

public class AnnotatedAgreementSearchModel : PagingOptionModel
{
    public bool? IsActive { get; set; }
    public bool WithDeleted { get; set; } = false;
}