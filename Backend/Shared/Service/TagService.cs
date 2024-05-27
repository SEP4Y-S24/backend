using Models;
using Shared.dtos;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class TagService: ITagService
{
    private readonly ITodoDAO _todoDao;
    private readonly ITagDao _tagDao;

    public TagService(ITodoDAO todoDao, ITagDao tagDao)
    {
        _todoDao = todoDao;
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
        }    
    }
    public async Task<IEnumerable<Tag>> GetAllByUserIdAsync(Guid userId)
    {
        try
        {
            return await _tagDao.GetAllByUserIdAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


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

    public async Task addTaskToTagAsync(List<TagDto> tagDTOs, Guid todoId)
    {
        Todo? t = await _todoDao.GetByIdAsync(todoId);
        if (t == null)
        {
            throw new Exception($"Todo with id {t.Id} does not exist!");
        }

        t.Tags = new List<Tag>();

        foreach (var tagDTO in tagDTOs)
        {
            Tag? tag = await _tagDao.GetByIdAsync(tagDTO.UserId);
            if (tag == null)
            {
                throw new Exception($"Tag with id {tag.Id} does not exist!");
            }
            t.Tags.Add(tag);
        }
        
        await _todoDao.UpdateAsync(t);
    }
}