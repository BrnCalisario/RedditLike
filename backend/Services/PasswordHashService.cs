using System;
using System.Text;
using System.Security.Cryptography;

namespace Reddit.Services;

public interface IPasswordHasher
{
    (byte[], string) GetHashAndSalt(string password);
    bool Validate(string password, byte[] realPasswordHashed, string salt);
}

public abstract class PasswordHasher : IPasswordHasher
{
    protected ISaltProvider saltProvider { get; set; }

    public (byte[], string) GetHashAndSalt(string password)
    {
        string salt = saltProvider.ProvideSalt();

        byte[] hashedPassword = hash(password, salt);

        return (hashedPassword, salt);
    }

    public bool Validate(string password, byte[] realPasswordHashed, string salt)
    {
    
        var hashed = this.hash(password, salt);

        for(int i = 0; i < realPasswordHashed.Length; i++)
        {
            if(hashed[i] != realPasswordHashed[i])
                return false;
        }
        return true;
    }

    protected virtual byte[] hash(string password, string salt)
    {
        using var sha = SHA256.Create();
        
        var salty = password + salt;

        var bytes = Encoding.UTF8.GetBytes(salty);
        
        var hashBytes = sha.ComputeHash(bytes);

        return hashBytes;
    }
}

public class BasicPasswordHasher : PasswordHasher
{
    public BasicPasswordHasher() 
    {
        this.saltProvider = new BasicSaltProvider();
    }
}