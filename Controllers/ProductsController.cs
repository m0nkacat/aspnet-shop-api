using Microsoft.AspNetCore.Mvc;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/products")]
public class ProductsController(IProductService service)
    : CrudController<ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>(service);
