using System.ComponentModel.DataAnnotations;

namespace Subscribers.Repositories;
public abstract class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
