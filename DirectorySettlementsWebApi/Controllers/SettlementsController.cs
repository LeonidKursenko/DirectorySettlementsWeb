using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Repositories;
using DirectorySettlementsDAL.Interfaces;

namespace DirectorySettlementsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementsController : ControllerBase
    {
        //private readonly ApplicationContext _context;
        private readonly IUnitOfWork _manager;

        public SettlementsController(IUnitOfWork unitOfWork)
        {
            //_context = context;
            _manager = unitOfWork;
        }

        // GET: api/Settlements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Settlement>>> GetSettlements()
        {
            //return await _context.Settlements.ToListAsync();
            return _manager.Settlements.GetAll().ToList();
        }

        // GET: api/Settlements/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Settlement>> GetSettlement(string id)
        //{
        //    var settlement = await _context.Settlements.FindAsync(id);

        //    if (settlement == null)
        //    {
        //        return NotFound();
        //    }

        //    return settlement;
        //}

        // PUT: api/Settlements/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSettlement(string id, Settlement settlement)
        //{
        //    if (id != settlement.Te)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(settlement).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SettlementExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Settlements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Settlement>> PostSettlement(Settlement settlement)
        //{
        //    _context.Settlements.Add(settlement);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (SettlementExists(settlement.Te))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetSettlement", new { id = settlement.Te }, settlement);
        //}

        // DELETE: api/Settlements/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Settlement>> DeleteSettlement(string id)
        //{
        //    var settlement = await _context.Settlements.FindAsync(id);
        //    if (settlement == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Settlements.Remove(settlement);
        //    await _context.SaveChangesAsync();

        //    return settlement;
        //}

        //private bool SettlementExists(string id)
        //{
        //    return _context.Settlements.Any(e => e.Te == id);
        //}
    }
}
