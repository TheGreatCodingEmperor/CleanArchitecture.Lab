using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

public class CustomerDomain
{
    [Key]
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}