namespace Schiavello.Shared;

public class PhotoUploadResult
{
    public string PublicId { get; set; }
    public string Url { get; set; }
    public string SecureUrl { get; set; }
    public bool Success { get; set; } = false;
    public string Failures { get; set; }
}

