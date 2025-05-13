using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    [Key]
    [Column("book_id")]
    public int BookId { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = "";

    [Required]
    [Column("first_name")]
    public string FirstName { get; set; } = "";

    [Required]
    [Column("last_name")]
    public string LastName { get; set; } = "";

    [Column("total_copies")]
    public int TotalCopies { get; set; }

    [Column("copies_in_use")]
    public int CopiesInUse { get; set; }

    [Column("type")]
    public string? Type { get; set; }   

    [Column("isbn")]
    public string? ISBN { get; set; }

    [Column("category")]
    public string? Category { get; set; }

    [Column("status")]
    public string? Status { get; set; }
}
