namespace NanaCaptcha;

public class captchaRequest
{
    public string? captcha { get; set; }
    public string? input { get; set; }
}
public class captchaResponse
{
    public string? captcha { get; set; }
    public string? base64 { get; set; }
}