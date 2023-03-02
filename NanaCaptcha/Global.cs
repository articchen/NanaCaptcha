using System.Text;

namespace NanaCaptcha;

public class Global
{
    public const string XORKey = "Nana"; 
    
    //--------------------------------------
    // XOREncrypt
    /// <summary>
    /// Encrypts a string with XOR and then converts it to a BASE64 string
    /// </summary>
    /// <param name="input">Unencrypted string</param>
    /// <param name="key">Encryption key</param>
    /// <returns>Encrypted string in BASE64</returns>
    public static string XOREncrypt(string input, string key)
    {
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
    public static string XORDecrypt(string input, string key)
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
   

    //--------------------------------------
    /// Generate random numbers
    /// <summary>
    /// Generate random numbers, with the length as the parameter, only containing numbers.
    /// </summary>
    /// <param name="num">Length</param>
    /// <returns>A string of numbers with the specified length</returns>
    //--------------------------------------
    public static string RandomNumber(int num)
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
}