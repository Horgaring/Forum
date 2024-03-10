using Microsoft.EntityFrameworkCore;

namespace Domain.ValueObjects;

[Owned]
public class CustomerInfo
{
    public Guid Id { get; set; }
    
}