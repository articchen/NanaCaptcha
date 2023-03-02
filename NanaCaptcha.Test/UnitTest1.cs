using NanaCaptcha.Controllers;
using Newtonsoft.Json;

namespace NanaCaptcha.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var captcha= new CaptchaController();
        var result = captcha.Captcha();
        //Convert Result to captchaResponse
        var response = JsonConvert.DeserializeObject<captchaResponse>(result);
        var captchaText = Global.XORDecrypt(response.captcha!,Global.XORKey);
        var request = new captchaRequest()
        {
            captcha = response.captcha,
            input = captchaText
        };
        var check = captcha.CheckCaptcha(request);
        Assert.That(actual: check, expression: Is.EqualTo(expected: "true"));
    }
}