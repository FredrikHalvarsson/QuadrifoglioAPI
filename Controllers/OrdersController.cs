﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadrifoglioAPI.Data;
using QuadrifoglioAPI.Models;

namespace QuadrifoglioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrders(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        [HttpGet("latest/{username}")]
        public async Task<ActionResult<Order>> GetLatestOrder(string username)
        {
            var user = await _context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(u => u.UserName == username);
            System.Diagnostics.Debug.WriteLine($"Received userName: {username}");
            if (user == null)
            {
                return NotFound($"Användaren med användarnamnet '{username}' hittades inte.");
            }
            // Hämta den senaste ordern för den hittade användaren
            var latestOrder = user.Orders.LastOrDefault();

            if (latestOrder == null)
            {
                return NotFound($"Det finns ingen oskickad order för användaren med användarnamnet '{username}'.");
            }

            return latestOrder;
        }

        [HttpGet("all/{username}")]
        public async Task<ActionResult<List<Order>>> GetUsersOrders(string username)
        {
            // Hämta användaren inklusive dess ordrar och orderprodukter
            var user = await _context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(u => u.UserName == username);

            System.Diagnostics.Debug.WriteLine($"Received userName: {username}");

            if (user == null)
            {
                return NotFound($"Användaren med användarnamnet '{username}' hittades inte.");
            }

            // Hämta alla ordrar för den hittade användaren
            var userOrders = user.Orders;

            if (userOrders == null || !userOrders.Any())
            {
                return NotFound($"Det finns inga ordrar för användaren med användarnamnet '{username}'.");
            }

            // Returnera sorterad lista av ordrar
            return userOrders.OrderBy(o => o.OrderStatus).ToList();
        }



        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrders", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
