using CustomerApi.DataAccess;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/customers/{customerId}/addresses")]
public class AddressController : ControllerBase
{
    private readonly CustomerContext _context;

    public AddressController(CustomerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == customerId)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer.Addresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid customerId, Guid id)
    {
        var address = await _context.Addresses
            .Where(a => a.CustomerId == customerId && a.Id == id)
            .FirstOrDefaultAsync();

        if (address == null)
        {
            return NotFound();
        }

        return Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Guid customerId, [FromBody] Address address)
    {
        address.Id = Guid.NewGuid();
        address.CustomerId = customerId;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { customerId, id = address.Id }, address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid customerId, Guid id, [FromBody] Address address)
    {
        address.Id = id;
        address.CustomerId = customerId;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Entry(address).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(address);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid customerId, Guid id)
    {
        var address = await _context.Addresses
            .Where(a => a.CustomerId == customerId && a.Id == id)
            .FirstOrDefaultAsync();

        if (address == null)
        {
            return NotFound();
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();

        return Ok();
    }
}