using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.dtos;
using Shared.DTOs;

namespace ATodoService.Functions;

public class TagService
{
    private readonly ILogger _logger;

    public TagService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TagService>();
    }


    [Function("CreateTag")]
    public async Task<IActionResult> CreateTag(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "tag")]
        HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        CreateTagDto dto = JsonConvert.DeserializeObject<CreateTagDto>(requestBody);
        var service = ServiceFactory.GetTagService();
        Tag tag = new Tag()
        {
            Name = dto.Name,
            UserId = dto.UserId,
            Colour = dto.Colour,
            Id = Guid.NewGuid()
        };
        Tag t = await service.CreateAsync(tag);
        TagDto respone = new TagDto()
        {
            Name = t.Name,
            UserId = t.UserId,
            Colour = t.Colour,
            Id = t.Id
        };
        return new OkObjectResult(respone);
    }
    [Function("GetAllTags")]
    public async Task<IActionResult> GetAllTags(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "tag/users/{userId}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var tagService = ServiceFactory.GetTagService();
        IEnumerable<Tag> tags = await tagService.GetAllByUserIdAsync(userId);

        Tags tagsDto = new Tags();
        tagsDto.tags = new List<TagDto>();

        foreach (var tag in tags)
        {
            TagDto t = new TagDto()
            {
                Name = tag.Name,  // Mapping the existing properties
                UserId = tag.UserId,
                Colour = tag.Colour,
                Id = tag.Id
            };
            tagsDto.tags.Add(t);
        }
        return new OkObjectResult(tagsDto);
    }
    [Function("DeleteTag")]
    public async Task<IActionResult> DeleteTag(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "tag/{id}")] HttpRequest req, Guid id)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var tagService = ServiceFactory.GetTagService();
        await tagService.DeleteAsync(id);
        return new OkObjectResult("Deleted successfully!");
    }

}