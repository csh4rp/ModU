namespace ModU.Abstract.Security;

public interface IHasher
{
    string ComputeMD5Hash(string input);
}