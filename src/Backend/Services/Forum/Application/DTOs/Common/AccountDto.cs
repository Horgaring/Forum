namespace Application.DTOs.Common;

public class AccountDto
{
    public AccountDto(string name,Guid id)
    {
        Name = name;
        Id = id;
    }
    
    public AccountDto()
    {
        
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
}