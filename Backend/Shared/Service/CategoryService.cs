using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class CategoryService: ICategoryervice
{
    private readonly ITodoDAO _todoDao;
    private readonly ICategoryDao _categoryDao;

    public CategoryService(ITodoDAO todoDao, ICategoryDao categoryDao)
    {
        _todoDao = todoDao;
        _categoryDao = categoryDao;
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
        Tag created = await _categoryDao.CreateAsync(tag);
        return created;
    }

    public async Task UpdateAsync(Tag tag)
    {
        try
        {
            await _categoryDao.UpdateAsync(tag);
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
            return await _categoryDao.GetByIdAsync(tagId);
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
            await _categoryDao.DeleteAsync(tagId);
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
            return await _categoryDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task addTaskToTagAsync(Guid tagId, Guid todoId)
    {
        Todo? t = await _todoDao.GetByIdAsync(todoId);
        if (t == null)
        {
            throw new Exception($"Todo with id {t.Id} does not exist!");
        }
        Tag? tag = await _categoryDao.GetByIdAsync(tagId);
        if (tag == null)
        {
            throw new Exception($"Tag with id {tag.Id} does not exist!");
        }
        t.Tags.Add(tag);
        await _todoDao.UpdateAsync(t);
        tag.Todos.Add(t);
        await _categoryDao.UpdateAsync(tag);
    }
}