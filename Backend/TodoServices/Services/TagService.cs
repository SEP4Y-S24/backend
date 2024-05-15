using TodoServices.IDAO;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Services;

public class TagService: ITagService
{
    private readonly ITodoDao _toDoDao;
    private readonly ITagDao _tagDao;

    public TagService(ITodoDao toDoDao, ITagDao tagDao)
    {
        _toDoDao = toDoDao;
        _tagDao = tagDao;
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
        Tag created = await _tagDao.CreateAsync(tag);
        return created;
    }

    public async Task UpdateAsync(Tag tag)
    {
        try
        {
            await _tagDao.UpdateAsync(tag);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Tag?> GetByIdAsync(Guid tagId)
    {
        try
        {
            return await _tagDao.GetByIdAsync(tagId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task DeleteAsync(Guid tagId)
    {
        try
        {
            await _tagDao.DeleteAsync(tagId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        try
        {
            return await _tagDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task addTaskToTagAsync(Guid tagId, Guid todoId)
    {
        Todo t = await _toDoDao.GetByIdAsync(todoId);
        if (t == null)
        {
            throw new Exception($"Todo with id {t.Id} does not exist!");
        }
        Tag tag = await _tagDao.GetByIdAsync(tagId);
        if (tag == null)
        {
            throw new Exception($"Tag with id {tag.Id} does not exist!");
        }
        t.Tags.Add(tag);
        await _toDoDao.UpdateAsync(t);
        tag.Todos.Add(t);
        await _tagDao.UpdateAsync(tag);
    }
}