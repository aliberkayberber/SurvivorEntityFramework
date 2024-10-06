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
    public class CategoriesController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CategoriesController(SurvivorDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_context.Categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) 
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryEntity>> Create(CreateCategoryDto category)
        {

            using var transcation = await _context.Database.BeginTransactionAsync();

            try
            {
                var newCategory = new CategoryEntity
                {
                    Name = category.Name
                };

                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                await transcation.CommitAsync();

                return CreatedAtAction(nameof(GetById),new {id = newCategory.Id} ,category);
            }
            catch (Exception ex)
            {   
                transcation.Rollback();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById(int id , UpdateCategoryDto updateCategory)
        {
            using var transcation = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _context.Categories.FirstOrDefaultAsync (x => x.Id == id);

                if(result == null) return NotFound();

                result.Name = updateCategory.Name;
                result.ModifiedDate = DateTime.Now;
                
                await _context.SaveChangesAsync();
                await transcation.CommitAsync();

                return Ok(result);

            }
            catch (Exception exception)
            {
                transcation.Rollback();
                throw;
            }



        //   var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        //
        //    if (result is null)
        //        return NotFound();
        //
        //
        //    result.Name = category.Name;
        //    result.ModifiedDate = DateTime.Now;
        //
        //    _context.SaveChangesAsync();
        //
        //    return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(result is null)
                return NotFound();

            result.IsDeleted = true;
            result.ModifiedDate= DateTime.Now;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
