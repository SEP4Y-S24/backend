using Microsoft.AspNetCore.Mvc;
using TodoServices.dtos;
using TodoServices.IServices;
using TodoServices.Model;

namespace TodoServices.Controllers;
[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost]
    public async Task<ActionResult<Tag>> CreateAsync(TagDto tag)
    {
        try
        {
            Tag tagToCreate = new Tag()
            {
                Id = Guid.NewGuid(),
                name = tag.Name,
                Todos = new List<Todo>()
            };
            Tag created = await _tagService.CreateAsync(tagToCreate);
            return Ok(200);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("")]
    public async Task<ActionResult<Todo>> GetAllAsync()
    {
        try
        {
            IEnumerable<Tag>? tags = await _tagService.GetAllAsync();
            if (tags == null)
            {
                return NotFound();
            }

            Tags tagsDto = new Tags();
            tagsDto.tags = new List<TagDto>();
            foreach (var tag in tags)
            {
                tagsDto.tags.Add(new TagDto()
                {
                    Name = tag.name
                });
            }
            return Ok(tagsDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("/{tagId}/todos/{todoId}")]
    public async Task<ActionResult<Tag>> AddTagToTodo(Guid tagId, Guid todoId)
    {
        try
        {
            await _tagService.addTaskToTagAsync(tagId, todoId);
            return Ok(200);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}