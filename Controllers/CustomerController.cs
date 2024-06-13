using CustomerApi.DataAccess;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerContext _context;

    public CustomerController(CustomerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Customers);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var customer = _context.Customers.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Customer customer)
    {
        customer.Id = Guid.NewGuid();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Customers.Add(customer);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Customer customer)
    {
        customer.Id = id;
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Entry(customer).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var customer = _context.Customers.Find(id);
        if (customer == null)
        {
            return NotFound();
        }

        _context.Customers.Remove(customer);
        _context.SaveChanges();

        return Ok();
    }
}