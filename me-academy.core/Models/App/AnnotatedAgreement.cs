using me_academy.core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace me_academy.core.Models.App;

public class AnnotatedAgreement : ContentBase, ISoftDeletable
{

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int DocumentId { get; set; }

    public Document Document { get; set; }
}
