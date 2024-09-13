namespace Subscribers.Repositories.Entities;
public class Subscriber : BaseEntity
{
    public string Email { get; set; }
    public DateTime ExpirationDate { get; set; }
}
