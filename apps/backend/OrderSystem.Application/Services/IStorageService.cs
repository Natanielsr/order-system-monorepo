using System;

namespace OrderSystem.Application.Services;

public interface IStorageService
{
    public Task<string> UploadFileAsync(Stream fileStream, string fileName);
}
