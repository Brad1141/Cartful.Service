using Cartful.Service.Entities;

namespace Cartful.Service.Dtos
{
    public record AccountDto(string firstName, string lastName, string userName, string password, string phoneNumber);

    public record ListDto(string title, Guid userID, List<Item> items);

}