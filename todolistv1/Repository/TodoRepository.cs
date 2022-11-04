using todolistv1.Models;
using Microsoft.EntityFrameworkCore;
using todolistv1.Repository;
using todolistv1.Data;

namespace todolistv1.Repository
{
    public class TodoRepository : ITodo
    {
        readonly todolistv1Context _dbContext = new();

        public TodoRepository(todolistv1Context dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Todo> GetTodoDetails()
        {
            try
            {
                return _dbContext.Todo.ToList();
            }
            catch
            {
                throw;
            }
        }

        public Todo GetTodoDetails(int id)
        {
            try
            {
                Todo? todo = _dbContext.Todo.Find(id);
                if (todo != null)
                {
                    return todo;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }
      

        public void AddTodo(Todo todo)
        {
            try
            {
                _dbContext.Todo.Add(todo);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTodo(Todo todo)
        {
            try
            {
                _dbContext.Entry(todo).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Todo DeleteTodo(int id)
        {
            try
            {
                Todo? todo = _dbContext.Todo.Find(id);

                if (todo != null)
                {
                    _dbContext.Todo.Remove(todo);
                    _dbContext.SaveChanges();
                    return todo;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckTodo(int id)
        {
            return _dbContext.Todo.Any(e => e.Id == id);
        }
    

}
}
