using InventoryHub.Models;

namespace InventoryHub.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItem>> GetItemsAsync();
    Task<InventoryItem?> GetItemByIdAsync(int id);
    Task<InventoryItem> AddItemAsync(InventoryItem item);
    Task<bool> UpdateItemAsync(InventoryItem item);
    Task<bool> DeleteItemAsync(int id);
}