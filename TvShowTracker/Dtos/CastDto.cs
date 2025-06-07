public class CastDto
{
    public required PersonDto Person { get; set; }
}

public class PersonDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
