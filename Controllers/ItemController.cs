using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("item")]
public class ItemController : ControllerBase
{

    private readonly CartfulRepository cartfulRepository;
    public ItemController(CartfulRepository cartfulRepository)
    {
        this.cartfulRepository = cartfulRepository;
    }

    [HttpGet]
    [Route("GetItems")]
    public async Task<List<Item>> GetItems(Guid listID)
    {
        List<Item> items = await cartfulRepository.GetAllItemsAsync(listID);
        return items;
    }

    // replace Item w/ ItemDto
    [HttpPost]
    [Route("CreateItems")]
    public async Task<IActionResult> CreateItems([FromQuery] Guid listID, [FromBody] List<Item> items)
    {
        await cartfulRepository.CreateAllItemsAsync(items);
        return CreatedAtAction(nameof(CreateItems), new { id = items[0].itemID }, items);
    }

    //Delete Items
    [HttpDelete]
    [Route("DeleteItems")]
    public async Task<ActionResult> DeleteItems([FromQuery] Guid listID, [FromBody] List<Guid> items)
    {
        await Task.Delay(1);
        return new OkResult();
    }
}