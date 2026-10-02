using Microsoft.Extensions.Caching.Memory;
using InventoryHub.Models;

namespace InventoryHub.Services;

public class InventoryService : IInventoryService
{
    private readonly IMemoryCache _cache;
    private const string CacheKey = "InventoryItemsList";
    private static readonly List<InventoryItem> _items = new()
    {
        new InventoryItem { Id = 1, Name = "Wireless Mouse", SKU = "LOG-M100", Quantity = 45, Price = 24.99m },
        new InventoryItem { Id = 2, Name = "Mechanical Keyboard", SKU = "KBD-RGB1", Quantity = 18, Price = 89.99m },
        new InventoryItem { Id = 3, Name = "27-inch Monitor", SKU = "MON-4K27", Quantity = 12, Price = 299.99m }
    };

    public InventoryService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<IEnumerable<InventoryItem>> GetItemsAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out List<InventoryItem>? items))
        {
            await Task.Delay(100); // Simulate asynchronous database read
            items = _items;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(CacheKey, items, cacheEntryOptions);
        }

        return items ?? new List<InventoryItem>();
    }

    public async Task<InventoryItem?> GetItemByIdAsync(int id)
    {
        var items = await GetItemsAsync();
        return items.FirstOrDefault(i => i.Id == id);
    }

    public async Task<InventoryItem> AddItemAsync(InventoryItem item)
    {
        item.Id = _items.Count > 0 ? _items.Max(i => i.Id) + 1 : 1;
        item.LastUpdated = DateTime.UtcNow;
        _items.Add(item);
        _cache.Remove(CacheKey);
        return await Task.FromResult(item);
    }

    public async Task<bool> UpdateItemAsync(InventoryItem item)
    {
        var existingIndex = _items.FindIndex(i => i.Id == item.Id);
        if (existingIndex == -1) return false;

        item.LastUpdated = DateTime.UtcNow;
        _items[existingIndex] = item;
        _cache.Remove(CacheKey);
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item == null) return false;

        _items.Remove(item);
        _cache.Remove(CacheKey);
        return await Task.FromResult(true);
    }
}
