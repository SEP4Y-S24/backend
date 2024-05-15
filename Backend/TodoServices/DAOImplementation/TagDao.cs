using ClockServices.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoServices.IDAO;
using TodoServices.Model;

namespace TodoServices.DAOImplementation;

public class TagDao: ITagDao
{
    private readonly ClockContext context;
    
    public TagDao(ClockContext dbContext)
    {
        context = dbContext;
    }


    public async Task<Tag> CreateAsync(Tag tag)
    {
        List<Tag> ta= context.Tags.Where(t => t.name.Equals(tag.name)).ToList();
        if(ta.Count>0)
        {
            throw new ArgumentException("Tag already exists");
        }
        EntityEntry<Tag> added = await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
        return added.Entity;

    }

    public async Task UpdateAsync(Tag tag)
    {
        Tag? dbEntity = await GetByIdAsync(tag.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        context.Entry(dbEntity).CurrentValues.SetValues(tag);

        context.Tags.Update(dbEntity);

        await context.SaveChangesAsync();

    }

    public Task<Tag?> GetByIdAsync(Guid tagId)
    {
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Tag ID is null");
        }
        
        Tag? existing = context.Tags.Include(t=>t.Todos).FirstOrDefault(todo => todo.Id == tagId);
        return Task.FromResult(existing);    }

    public async Task DeleteAsync(Guid tagId)
    {
        
        if (tagId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Tag ID is null");
        }
        
        Tag? toDelete = await GetByIdAsync(tagId);
        context.Tags.Remove(toDelete);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await context.Set<Tag>().Include(t=>t.Todos).ToListAsync();
    }
}