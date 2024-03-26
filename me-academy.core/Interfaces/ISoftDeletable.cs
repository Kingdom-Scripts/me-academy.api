namespace me_academy.core.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTime? DeletedOn { get; set; }
}