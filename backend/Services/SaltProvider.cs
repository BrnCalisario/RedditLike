namespace Reddit.Services;

using System;

public interface ISaltProvider
{
    string ProvideSalt();
}


public class BasicSaltProvider : ISaltProvider
{
    public readonly byte[] lengthOptions = new byte[2] { 12, 18 };


    public string ProvideSalt()
    {
        Random rnd = new Random();

        int length = lengthOptions[rnd.Next(0, lengthOptions.Length)];

        byte[] saltBytes = new byte[length];
        rnd.NextBytes(saltBytes);

        var saltBase64 = Convert.ToBase64String(saltBytes);

        return saltBase64;
    }
}