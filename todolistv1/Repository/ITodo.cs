using todolistv1.Models;
namespace todolistv1.Repository
{
    public interface ITodo
    {
        public List<Todo> GetTodoDetails();
        public Todo GetTodoDetails(int id);
        public void AddTodo(Todo todo);
        public void UpdateTodo(Todo todo);
        public Todo DeleteTodo(int id);
        public bool CheckTodo(int id);
    }
}
