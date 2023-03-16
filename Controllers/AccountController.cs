using Microsoft.AspNetCore.Mvc;
using Cartful.Service.Dtos;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

namespace Cartful.Service.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{

    private readonly CartfulRepository cartfulRepository;
    public AccountController(CartfulRepository cartfulRepository)
    {
        this.cartfulRepository = cartfulRepository;
    }

    [HttpGet]
    [Route("GetAccount")]
    public async Task<ActionResult<Account>> GetAccount(string username, string password)
    {
        var creds = new Account
        {
            userName = username,
            password = password
        };

        var result = await cartfulRepository.GetAsync(creds);
        return result;
    }

    [HttpPost]
    [Route("CreateAccount")]
    public async Task<IActionResult> CreateAccount(AccountDto accountDto)
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

        await cartfulRepository.CreateAsync(newAccount);
        return CreatedAtAction(nameof(CreateAccount), new { id = newAccount.userId }, newAccount);
        
    }
}