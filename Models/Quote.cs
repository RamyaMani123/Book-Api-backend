using System.ComponentModel.DataAnnotations.Schema;

[Table("Quotes")]
public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}