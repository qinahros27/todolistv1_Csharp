using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolistv1.Data;
using todolistv1.Models;
using todolistv1.Repository;

namespace todolistv1.Controllers
{
    [Authorize]
    [Route("api/v1/todo")]
    [ApiController]
    public class TodoesController : ControllerBase
    {
        private readonly ITodo _Itodo;

        public TodoesController(ITodo Itodo)
        {
            _Itodo = Itodo;
        }

        //GET: api/v1/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodo()
        {
            return await Task.FromResult(_Itodo.GetTodoDetails());
        }
        
        // GET: api/v1/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await Task.FromResult(_Itodo.GetTodoDetails(id));

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // PUT: api/v1/todo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> PutTodo(int id, Todo todo)
        {
           if (id != todo.Id) { 
               return BadRequest();
           }

            try
            {
                todo.UpdatedDate = DateTime.Now;
                _Itodo.UpdateTodo(todo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await Task.FromResult(todo);
        }

        // POST: api/todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _Itodo.AddTodo(todo);
            return await Task.FromResult(todo);

        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Todo>> DeleteTodo(int id)
        {
            var todo = _Itodo.DeleteTodo(id);
     


            return await Task.FromResult(todo);
        }

        private bool TodoExists(int id)
        {
            return _Itodo.CheckTodo(id);
        }
    }
}
