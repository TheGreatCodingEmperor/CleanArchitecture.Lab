using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class InvoiceDomain
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public int CustomerId { get; set; }
}