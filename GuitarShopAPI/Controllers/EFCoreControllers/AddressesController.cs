using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.EFCore.Entities;
using MyGuitarShop.Data.EFCore.Repositories;

namespace GuitarShopAPI.Controllers.EFCoreControllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AddressesEFCoreController(
        AddressRepository repository,
        ILogger<AddressesEFCoreController> logger)
        : BaseController<AddressesDTO, Address>(repository, logger)
    {
    }
}
