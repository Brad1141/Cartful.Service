using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{

    private readonly AccountRepository accountRepository;
    public AccountController(AccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> Post(AccountDto accountDto)
    {
        // generate guid
        Guid newGuid = new Guid();

        var newAccount = new Account
        {
            userId = newGuid,
            firstName = accountDto.firstName,
            lastName = accountDto.lastName,
            password = accountDto.password,
            phoneNumber = accountDto.phoneNumber
            
        };

        await accountRepository.CreateAsync(newAccount);
        return CreatedAtAction("New Account", new { userId = newAccount.userId }, newAccount);
    }
}
