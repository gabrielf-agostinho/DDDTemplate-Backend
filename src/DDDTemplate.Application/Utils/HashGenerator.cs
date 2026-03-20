using System.Security.Cryptography;
using System.Text;

namespace DDDTemplate.Application.Utils;

public static class HashGenerator
{
  public static string GenerateSHA256(string text)
  {
    if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
      throw new ArgumentNullException(nameof(text));

    byte[] bytes = Encoding.UTF8.GetBytes(text);
    byte[] hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash);
  }
}