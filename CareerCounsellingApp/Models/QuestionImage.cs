namespace CareerCounsellingApp.Models;

public class QuestionImage
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public byte[]? ImageData { get; set; }

    public Question? Question { get; set; }
}
