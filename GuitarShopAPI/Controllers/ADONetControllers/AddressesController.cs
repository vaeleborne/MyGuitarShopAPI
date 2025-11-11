using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesADONetController(
        ILogger<AddressesADONetController> logger,
        AddressRepoADO repo)
        : BaseController<AddressesDTO, AddressEntityADO>(repo, logger)

    {

    }
}
