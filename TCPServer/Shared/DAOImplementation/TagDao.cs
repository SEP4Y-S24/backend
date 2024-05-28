using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace TodoServices.DAOImplementation;

public class TagDao: ITagDao
{
    private readonly ClockContext _context;
    
    public TagDao(ClockContext dbContext)
    {
        _context = dbContext;
    }


    public async Task<Tag> CreateAsync(Tag tag)
    {
        List<Tag> ta= await _context.Tags.Where(t => t.Name.Equals(tag.Name)).ToListAsync();
        if(ta.Count>0)
        {
            throw new ArgumentException("Tag already exists");
        }
        EntityEntry<Tag> added = await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        return added.Entity;

    }

    public async Task UpdateAsync(Tag tag)
    {
        Tag? dbEntity = await GetByIdAsync(tag.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        _context.Entry(dbEntity).CurrentValues.SetValues(tag);

        _context.Tags.Update(dbEntity);

        await _context.SaveChangesAsync();

    }

    public async Task<Tag?> GetByIdAsync(Guid tagId)
    {
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Tag ID is null");
        }
        
        Tag? existing = await _context.Tags.Include(t=>t.Todos).FirstOrDefaultAsync(todo => todo.Id == tagId);
        return existing;
    }

    public async Task DeleteAsync(Guid tagId)
    {
        
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Tag ID is null");
        }
        
        Tag? toDelete = await GetByIdAsync(tagId);
        if (toDelete == null)
        {
            throw new ArgumentNullException("Tag object is not found in the database");
        }
        _context.Tags.Remove(toDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Set<Tag>().Include(t=>t.Todos).ToListAsync();
    }
}