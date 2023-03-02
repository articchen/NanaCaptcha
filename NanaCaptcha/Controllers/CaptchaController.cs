using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NanaCaptcha.Controllers;

[ApiController]
[Route("captcha")]
public class CaptchaController : ControllerBase
{
    private const string XORKey = "Nana"; 

    //--------------------------------------
    /// Generate random numbers
    /// <summary>
    /// Generate random numbers, with the length as the parameter, only containing numbers.
    /// </summary>
    /// <param name="num">Length</param>
    /// <returns>A string of numbers with the specified length</returns>
    //--------------------------------------
    public string RandomNumber(int num)
    {
        string str = "0123456789";
        Random random = new Random();
        string result = "";
        for (int i = 0; i < num; i++)
        {
            int index = random.Next(0, str.Length);
            result += str[index];
        }
        return result;
    }
    //For Now only support IIS Server
    [HttpGet]
    public string Captcha()
    {
        string captcha = RandomNumber(6);
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
            captcha = XOREncrypt(captcha,XORKey),base64 = base64};
        
        string json = JsonConvert.SerializeObject(tmp);
        return json;
    }
    [HttpPost]
    public string CheckCaptcha(captchaRequest request)
    {
        if (request.captcha == XOREncrypt(request.input!,XORKey))
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }
    
    //--------------------------------------
    // XOREncrypt
    /// <summary>
    /// Encrypts a string with XOR and then converts it to a BASE64 string
    /// </summary>
    /// <param name="input">Unencrypted string</param>
    /// <param name="key">Encryption key</param>
    /// <returns>Encrypted string in BASE64</returns>
    public string XOREncrypt(string input, string key)
    {
        //把input用xor加密
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] outputBytes = new byte[inputBytes.Length];
        for (int i = 0; i < inputBytes.Length; i++)
        {
            outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }
        return Convert.ToBase64String(outputBytes);
    }
    //--------------------------------------
    // XORDecrypt
    /// <summary>
    /// Convert the BASE64 string back to a regular string, and then XOR decrypt according to the Key, returning the decrypted string
    /// </summary>
    /// <param name="input">The encrypted string, must be a BASE64 string</param>
    /// <param name="key">The key for decryption</param>
    /// <returns>The decrypted string</returns>
    //--------------------------------------
    public string XORDecrypt(string input, string key)
    {
        try
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                outputBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Encoding.UTF8.GetString(outputBytes);
        }
        catch
        {
            throw new Exception("Error decrypting. Make sure the key is correct and the string is a valid BASE64 string.");
        }
    }

}

