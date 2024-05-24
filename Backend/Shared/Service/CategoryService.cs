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

    public async Task<Category> CreateAsync(Category category)
    {
        Category created = await _categoryDao.CreateAsync(category);
        return created;
    }

    public async Task UpdateAsync(Category category)
    {
        try
        {
            await _categoryDao.UpdateAsync(category);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Category?> GetByIdAsync(Guid tagId)
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

    public async Task<IEnumerable<Category>> GetAllAsync()
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
        Category? tag = await _categoryDao.GetByIdAsync(tagId);
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