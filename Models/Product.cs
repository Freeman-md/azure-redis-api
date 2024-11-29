namespace Models;

public record Product {
    public long Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }
}