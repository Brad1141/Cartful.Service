// using Microsoft.AspNetCore.Mvc;
// using Cartful.Service.Dtos;
// using Cartful.Service.Entities;
// using Cartful.Service.Repositories;

// namespace Cartful.Service.Controllers;

// [ApiController]
// [Route("item")]
// public class ItemController : ControllerBase
// {

//     private readonly CartfulRepository cartfulRepository;
//     public ItemController(CartfulRepository cartfulRepository)
//     {
//         this.cartfulRepository = cartfulRepository;
//     }

//     [HttpGet]
//     [Route("GetItems")]
//     public async Task<List<Item>> GetItems(Guid listID)
//     {
//         List<Item> items = await cartfulRepository.GetAllItemsAsync(listID);
//         return items;
//     }

//     [HttpPost]
//     [Route("CreateItems")]
//     public async Task<IActionResult> CreateItems(List<Item> items)
//     {
//         await cartfulRepository.CreateAllItemsAsync(items);
//         return CreatedAtAction(nameof(CreateItems), new { id = items[0].itemID }, items);
//     }

//     //Delete Items
// }