using Microsoft.AspNetCore.Mvc;
using InventoryHub.Models;
using InventoryHub.Services;

namespace InventoryHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAll()
    {
        var items = await _inventoryService.GetItemsAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InventoryItem>> GetById(int id)
    {
        var item = await _inventoryService.GetItemByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItem>> Create([FromBody] InventoryItem item)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _inventoryService.AddItemAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] InventoryItem item)
    {
        if (id != item.Id) return BadRequest();
        var updated = await _inventoryService.UpdateItemAsync(item);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _inventoryService.DeleteItemAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}