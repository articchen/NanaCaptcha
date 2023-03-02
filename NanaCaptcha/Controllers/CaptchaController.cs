using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NanaCaptcha.Controllers;

[ApiController]
[Route("captcha")]
public class CaptchaController : ControllerBase
{
    
    //For Now only support IIS Server
    [HttpGet]
    public string Captcha()
    {
        string captcha = Global.RandomNumber(6);
        System.Drawing.Bitmap image = new System.Drawing.Bitmap(155, 50);
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
        Random random = new Random();
        g.Clear(System.Drawing.Color.White);
        for (int i = 0; i < 25; i++)
        {
            int x1 = random.Next(image.Width);
            int x2 = random.Next(image.Width);
            int y1 = random.Next(image.Height);
            int y2 = random.Next(image.Height);
            g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Silver), x1, y1, x2, y2);
        }
        for (int i = 0; i < 100; i++)
        {
            int x = random.Next(image.Width);
            int y = random.Next(image.Height);
            image.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next()));
        }
        for (int i = 0; i < captcha.Length; i++)
        {
            string fnt = "Arial";
            System.Drawing.Font font = new System.Drawing.Font(fnt, 20, System.Drawing.FontStyle.Bold);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);
            g.DrawString(captcha[i].ToString(), font, brush, 3 + (i * 25), 3);
        }
        g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        string base64 = Convert.ToBase64String(ms.ToArray());
        
        captchaResponse tmp = new captchaResponse(){
            captcha = Global.XOREncrypt(captcha,Global.XORKey),base64 = base64};
        
        string json = JsonConvert.SerializeObject(tmp);
        return json;
    }
    
    [HttpPost] 
    public string CheckCaptcha(captchaRequest request)
    {
        if (Global.XORDecrypt(request.captcha,Global.XORKey) == request.input)
        {
            return "true";
        }
        return "false";
    }
    
    

}

