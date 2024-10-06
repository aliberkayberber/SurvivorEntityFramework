using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurvivorEntityFramework.Context;
using SurvivorEntityFramework.DTOs;
using SurvivorEntityFramework.Entities;

namespace SurvivorEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorsController(SurvivorDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_context.Competitors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           
          var result = await _context.Competitors.FirstOrDefaultAsync(x => x.Id == id);

            
            if (result is null) 
                return NotFound();

            return Ok(result);
        }

        [HttpGet("categories/{CategoryId}")]
        public async Task<IActionResult> GetByCategoryId(int CategoryId)
        {
            //var result = await _context.Competitors.FirstOrDefaultAsync(x => x.CategoryId == CategoryId);

            var result = await _context.Competitors.Where(x => x.CategoryId == CategoryId).ToListAsync();

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompetitorDto competitorEntity)
        {
            using var transcation = await _context.Database.BeginTransactionAsync();

            try
            {
                var newCompetitor = new CompetitorEntity
                {
                    FirstName = competitorEntity.FirstName,
                    LastName = competitorEntity.LastName,
                    CategoryId = competitorEntity.CategoryId,
                };

                await _context.AddAsync(newCompetitor);
                await _context.SaveChangesAsync();
                transcation.Commit();

                return CreatedAtAction(nameof(GetById),new {id = newCompetitor.Id}, newCompetitor);
            }
            catch (Exception ex) 
            {
                transcation.Rollback();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateCompetitorDto competitorEntity)
        {
            using var transcation = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _context.Competitors.FirstOrDefaultAsync(c => c.Id == id);

                if (result == null) return NotFound();

                result.FirstName = competitorEntity.FirstName;
                result.LastName = competitorEntity.LastName;
                result.CategoryId = competitorEntity.CategoryId;
                result.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                await transcation.CommitAsync();

                return Ok(result);

            }
            catch (Exception ex)
            {
                transcation.Rollback();
                throw;
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _context.Competitors.FirstOrDefaultAsync(x =>x.Id == id);

            if (result is null)
                return NotFound();

            result.ModifiedDate = DateTime.Now;
            result.IsDeleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
