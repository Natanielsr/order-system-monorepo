using System;
using AutoMapper;
using MediatR;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Application.Services;
using OrderSystem.Application.Validator;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Application.Products.Commands.CreateProduct;

public class CreateProductHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStorageService storageService
    ) : IRequestHandler<CreateProductCommand, CreateProductResponseDto>
{
    public async Task<CreateProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = mapper.Map<Product>(request);

        var IsValidImage = ImageValidator.IsValidImage(
            request.FileStream,
            request.FileName,
            request.ContentType);

        if (!IsValidImage)
            throw new NotValidImageException();

        string filePath = await storageService.UploadFileAsync(request.FileStream, request.FileName);
        product.ImagePath = filePath;

        var createdProduct = await productRepository.AddAsync(product);
        var response = mapper.Map<CreateProductResponseDto>(createdProduct);

        await unitOfWork.CommitAsync();

        return response;
    }
}
