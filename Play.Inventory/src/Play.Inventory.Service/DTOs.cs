
namespace Play.Inventory.Service.DTOs
{
    public record GrantItemDTO(Guid userId, Guid catalogItemId, int quantity);
    public record InventoryItemDTO(Guid catalogItemId, int quantity, string name, string description, DateTimeOffset acquiredDate);
    public record CatalogItemDTO(Guid id, string Name, string Description);

}