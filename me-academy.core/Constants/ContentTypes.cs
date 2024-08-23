namespace me_academy.core.Constants;

internal static class ContentTypes
{
    public static readonly string Course = "Course";
    public static readonly string Series = "Series";
    public static readonly string SmeHub = "SmeHub";
    public static readonly string AnnotatedAgreement = "AnnotatedAgreement";

    public static readonly string DB_CONSTRAINT = $"('{Course}', '{Series}', '{SmeHub}', '{AnnotatedAgreement}')";
}