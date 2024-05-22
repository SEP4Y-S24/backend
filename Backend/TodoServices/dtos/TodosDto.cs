namespace TodoServices.dtos;

public class TodosDto
{
    public List<TodoDto> Todos { get; set; }
    public TodosDto()
    {
        Todos = new List<TodoDto>();
    }
}