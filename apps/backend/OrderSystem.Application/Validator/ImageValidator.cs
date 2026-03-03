using System;

namespace OrderSystem.Application.Validator;

public static class ImageValidator
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

    public static bool IsValidImage(Stream fileStream, string fileName, string contentType)
    {
        // 1. Validação de Extensão
        var extension = Path.GetExtension(fileName).ToLower();
        if (!AllowedExtensions.Contains(extension))
            return false;

        // 2. Validação de MIME Type (enviado pelo navegador)
        if (!AllowedMimeTypes.Contains(contentType.ToLower()))
            return false;

        // 3. Validação de Tamanho (Ex: Máximo 5MB)
        long maxFileSize = 5 * 1024 * 1024;
        if (fileStream.Length > maxFileSize)
            return false;

        return true;
    }
}
