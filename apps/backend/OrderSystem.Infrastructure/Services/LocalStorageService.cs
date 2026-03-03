using System;
using OrderSystem.Application.Services;

namespace OrderSystem.Infrastructure.Services;

public class LocalStorageService : IStorageService
{
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        // 1. Define o nome único
        var newName = Guid.NewGuid() + Path.GetExtension(fileName);

        // 2. Caminho para salvar (Físico)
        var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

        // 3. Garante que a pasta existe
        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        var fullPath = Path.Combine(uploadDirectory, newName);

        // 4. Salva o arquivo
        using (var file = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(file);
        }

        // 5. Retorna apenas o caminho relativo para o navegador/banco
        return $"/images/products/{newName}";
    }
}
