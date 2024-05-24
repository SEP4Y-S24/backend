using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace TodoServices.DAOImplementation;

public class CategoryDao: ICategoryDao
{
    private readonly ClockContext _context;
    
    public CategoryDao(ClockContext dbContext)
    {
        _context = dbContext;
    }


    public async Task<Category> CreateAsync(Category category)
    {
        List<Category> ta= await _context.Tags.Where(t => t.Name.Equals(category.Name)).ToListAsync();
        if(ta.Count>0)
        {
            throw new ArgumentException("Tag already exists");
        }
        EntityEntry<Category> added = await _context.Tags.AddAsync(category);
        await _context.SaveChangesAsync();
        return added.Entity;

    }

    public async Task UpdateAsync(Category category)
    {
        Category? dbEntity = await GetByIdAsync(category.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        _context.Entry(dbEntity).CurrentValues.SetValues(category);

        _context.Tags.Update(dbEntity);

        await _context.SaveChangesAsync();

    }

    public async Task<Category?> GetByIdAsync(Guid tagId)
    {
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Tag ID is null");
        }
        
        Category? existing = await _context.Tags.Include(t=>t.Todos).FirstOrDefaultAsync(todo => todo.Id == tagId);
        return existing;
    }

    public async Task DeleteAsync(Guid tagId)
    {
        
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Tag ID is null");
        }
        
        Category? toDelete = await GetByIdAsync(tagId);
        if (toDelete == null)
        {
            throw new ArgumentNullException("Tag object is not found in the database");
        }
        _context.Tags.Remove(toDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Set<Category>().Include(t=>t.Todos).ToListAsync();
    }
}