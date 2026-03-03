using System;

namespace OrderSystem.Application.Services;

public static class GenerateCode
{
    public static string Generate(int length = 8)
    {
        // Definimos os caracteres permitidos (Letras e Números)
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        Random random = new Random();

        // Geramos a string escolhendo caracteres aleatórios da lista acima
        char[] result = Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray();

        var resultString = new string(result);

        return resultString;
    }
}
