using System.Security.Cryptography;
using System.Text;
using ModU.Abstract.Security;

namespace ModU.Infrastructure.Security;

internal sealed class Hasher : IHasher
{
    public string ComputeMD5Hash(string input)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        for (var i = 0; i < hash.Length; i++)
        {
            builder.AppendFormat("{0:x}", hash[i]);
        }

        return builder.ToString();
    }
}