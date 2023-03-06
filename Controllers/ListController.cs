using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("list")]
public class ListController : ControllerBase
{

    private readonly AccountRepository accountRepository;
    public ListController(AccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpPost]
    [Route("CreateList")]
    public async Task<IActionResult> CreateList(AccountDto accountDto)
    {
        // generate guid
        Guid newGuid = Guid.NewGuid();

        var newAccount = new Account
        {
            userId = newGuid,
            firstName = accountDto.firstName,
            lastName = accountDto.lastName,
            userName = accountDto.userName,
            password = accountDto.password,
            phoneNumber = accountDto.phoneNumber

        };

        await accountRepository.CreateAsync(newAccount);
        return CreatedAtAction(nameof(CreateList), new { id = newAccount.userId }, newAccount);

    }
}