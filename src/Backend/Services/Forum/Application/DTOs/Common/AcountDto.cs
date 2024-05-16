namespace Application.DTOs.Common;

public class AcountDto
{
    public AcountDto(string name,Guid id)
    {
        Name = name;
        Id = id;
    }
    
    public AcountDto()
    {
        
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
}