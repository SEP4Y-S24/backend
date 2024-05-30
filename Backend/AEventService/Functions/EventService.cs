using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Shared;
using Shared.dtos;
using Shared.DTOs;


namespace AEventService.Functions
{
    public class EventService
    {
        private readonly ILogger<EventService> _logger;

        public EventService(ILogger<EventService> logger)
        {
            _logger = logger;
        }

        [Function("ClockService")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("GetAllEvents")]
        public async Task<IActionResult> GetAllEvents(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "events/users/{userId}")] HttpRequest req, Guid userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var eventService = ServiceFactory.GetEventService();
            IEnumerable<Event> events = await eventService.GetAllByUserIdAsync(userId);

            EventsDto eventsDto = new EventsDto();
            eventsDto.Events = new List<EventDTO>();

            foreach (var ev in events)
            {
                EventDTO eventDto = new EventDTO()
                {
                    Id = ev.Id,
                    Name = ev.Name,  // Mapping the existing properties
                    Description = ev.Description,
                    Start = ev.StartDate,
                    End = ev.EndDate,
                    Status = ev.Status,
                    UserId = ev.UserId
                };
                eventDto.tags = new List<TagDto>();
                foreach (var tag in ev.Categories)
                {
                    TagDto tagDto = new TagDto()
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Colour = tag.Colour,
                        UserId = tag.UserId
                    };
                    eventDto.tags.Add(tagDto);
                }

                eventsDto.Events.Add(eventDto);
            }
            return new OkObjectResult(eventsDto);
        }


        [Function("CreateEvent")]
        public async Task<IActionResult> CreateEvent(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "events")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateEventDto ev = JsonConvert.DeserializeObject<CreateEventDto>(requestBody);
                if (ev.Start == DateTime.MinValue)
                {
                    throw new Exception("Start date is required!");
                }
                if (ev.End == DateTime.MinValue)
                {
                    throw new Exception("End date is required!");
                }
                var eventService = ServiceFactory.GetEventService();
                Event eventToCreate = new Event
                {
                    Id = Guid.NewGuid(),
                    Name = ev.Name,
                    Description = ev.Description,
                    StartDate = ev.Start,
                    EndDate = ev.End,
                    Status = ev.Status,
                    UserId = ev.UserId
                };
                Event created = await eventService.CreateAsync(eventToCreate);
                EventDTO eventDto = new EventDTO
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Start = created.StartDate,
                    End = created.EndDate,
                    Status = created.Status,
                    UserId = created.UserId
                };
                eventDto.tags = new List<TagDto>();
                foreach (var tag in created.Categories)
                {
                    TagDto tagDto = new TagDto()
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Colour = tag.Colour,
                        UserId = tag.UserId
                    };
                    eventDto.tags.Add(tagDto);
                }

                return new OkObjectResult(eventDto);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestObjectResult(e.Message);
            }
        }

        [Function("GetEvent")]
        public async Task<IActionResult> GetEvent(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "events/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var eventService = ServiceFactory.GetEventService();
            Event ev = await eventService.GetByIdAsync(id);

            EventDTO eventDto = new EventDTO
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                Start = ev.StartDate,
                End = ev.EndDate,
                Status = ev.Status,
                UserId = ev.UserId
            };
            eventDto.tags = new List<TagDto>();
            foreach (var tag in ev.Categories)
            {
                TagDto tagDto = new TagDto()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Colour = tag.Colour,
                    UserId = tag.UserId
                };
                eventDto.tags.Add(tagDto);
            }
            return new OkObjectResult(eventDto);
        }

        [Function("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "events/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EventDTO eventDto = JsonConvert.DeserializeObject<EventDTO>(requestBody);

            var eventService = ServiceFactory.GetEventService();
            var existingEvent = await eventService.GetByIdAsync(id);
            if (existingEvent == null)
            {
                return new NotFoundResult();
            }
            existingEvent.Name = eventDto.Name;
            existingEvent.Id = eventDto.Id;
            existingEvent.Description = eventDto.Description;
            existingEvent.StartDate = eventDto.Start;
            existingEvent.EndDate = eventDto.End;
            existingEvent.Status = eventDto.Status;
            existingEvent.UserId = eventDto.UserId;

            await eventService.UpdateAsync(existingEvent);

            return new OkObjectResult(eventDto);
        }

        [Function("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "events/{id}")] HttpRequest req, Guid id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var eventService = ServiceFactory.GetEventService();
            await eventService.DeleteAsync(id);
            return new OkResult();
        }

        [Function("UpdateTagsToEvent")]
        public async Task<IActionResult> UpdateTagsToEvent(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "events/{id}")]
            HttpRequest req, Guid id)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Tags tags = JsonConvert.DeserializeObject<Tags>(requestBody);
                var tagService = ServiceFactory.GetTagService();

                    await tagService.addEventToTagAsync(tags.tags, id);
                return new OkObjectResult("Added");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
    }

}
