using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("item")]
public class ItemController : ControllerBase
{

    private readonly AccountRepository accountRepository;
    public ItemController(AccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpPost]
    [Route("CreateItems")]
    public async Task<IActionResult> CreateItems(List<Item> items)
    {
        await itemRepository.CreateAllAsync(items);
        return CreatedAtAction(nameof(CreateItems), new { id = items[0].itemID }, items);
    }
}