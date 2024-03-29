using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("list")]
public class ListController : ControllerBase
{

    private readonly CartfulRepository cartfulRepository;
    public ListController(CartfulRepository cartfulRepository)
    {
        this.cartfulRepository = cartfulRepository;
    }

    [HttpGet]
    [Route("GetLists")]
    public async Task<ActionResult<List<ItemList>>> GetLists(Guid userId)
    {
        // from list repo, call item repo to get all items 
        List<ItemList> itemLists = await cartfulRepository.GetAllListsAsync(userId);
        return itemLists;
    }

    [HttpPost]
    [Route("CreateList")]
    public async Task<IActionResult> CreateList(ListDto listDto)
    {
        // generate guid
        Guid newGuid = Guid.NewGuid();

        ItemList newList = new ItemList
        {
            title = listDto.title,
            userID = listDto.userID,
            listID = newGuid
        };

        List<Item> newItems = listDto.items;
        foreach (Item item in newItems)
        {
            item.itemID = Guid.NewGuid();
            item.listID = newList.listID;
        }
        newList.items = newItems;

        await cartfulRepository.CreateListAsync(newList);
        await cartfulRepository.CreateAllItemsAsync(newItems);
        return CreatedAtAction(nameof(CreateList), new { id = newGuid }, newList);

    }

    [HttpDelete]
    [Route("DeleteList")]
    public async Task<IActionResult> DeleteList(Guid listId)
    {
        await cartfulRepository.DeleteListAsync(listId);
        return Ok();
    }
}