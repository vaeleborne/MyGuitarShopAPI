using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsADONetController(
        ILogger<ProductsADONetController> logger,
        ProductRepoADO repo) 
        : BaseController<ProductDTO, ProductEntityADO>(repo, logger)
    {

    }
}
