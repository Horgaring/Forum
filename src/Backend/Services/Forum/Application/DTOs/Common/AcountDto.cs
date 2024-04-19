namespace Application.DTOs.Common;

public class AcountDto
{
    public AcountDto(string name)
    {
        Name = name;
    }
    
    public AcountDto()
    {
        
    }

    public string Name { get; set; }
}