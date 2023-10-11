# Introduction
This project uses C#, ASP.NET Core with Entity Framework, and PostgreSQL to create the backend for a personal to-do application that requires users to be logged in before they can call the APIs. One user can sign in, sign up, change their password; create, update, delete, and read the user's to-do lists.

## Table of content
- Technologies
- Project structure
- Getting started

## Technologies 
- C#
- ASP.NET Core
- PostgreSQL

## Project structure
```
.
└───Todolistv1_Csharp
    └───todolistv1
        └───Controllers
            └───LoginController.cs
            └───PasswordController.cs
            └───TodoesController.cs
            └───UsersController.cs
        └───Data
            └───todolistv1Context.cs
        └───Migrations
        └───Models
            └───Todo.cs
            └───User.cs
        └───Properties
            └───launchSettings.json
            └───serviceDependencies.json
            └───serviceDependencies.local.json
        └───Repository
            └───ITodo.cs
            └───TodoRepository.cs
        └───Program.cs
        └───appsettings.Development.json
        └───appsettings.json
        └───todolistv1.csproj
    └───.gitattributes
    └───.gitignore
    └───README.md
    └───todolistv1.sln
```
## Getting started
Clone the respository from github: ```git clone```

<!--- # Todolistv1_Csharp
## Install Required Nuget Packages
+ Microsoft.EntityFrameworkCore
+ Microsoft.EntityFrameworkCore.Tools
+ Microsoft.EntityFrameworkCore.Design
+ Npgsql.EntityFrameworkCore.PostgreSQL
+ Microsoft.AspNetCore.Authentication.JwtBearer

## Adding the models to the Application 
First , I created models folder then User.cs and Todo.cs.
This is User.cs :

``` Csharp
namespace todolistv1.Models
{
    public class User
    {
        public User()
        {
            CreatedDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<Todo>? Todos { get; set; }

    }
}
```

Todo.cs :

``` Csharp
namespace todolistv1.Models
{
    public class Todo
    {
        public Todo()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
        public int Id { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public enum status
        {
            NotStarted = 0 , Ongoing =  1, Completed = 2
        }
        public status Status { get; set; }

    }
}
```

## Adding Data Access Layer to the Application:
I created the folder Data then creating the "todolistv1Context.cs" to define database connection :

``` Csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todolistv1.Models;

namespace todolistv1.Data
{
    public class todolistv1Context : DbContext
    {
        public todolistv1Context()
        {
        }
        public todolistv1Context (DbContextOptions<todolistv1Context> options)
            : base(options)
        {
        }

        public DbSet<todolistv1.Models.User> User { get; set; }

        public DbSet<todolistv1.Models.Todo> Todo { get; set; }
    }
}
```
## Adding the connection strings to the appsettings.json:

``` Csharp
"ConnectionStrings": {
    "todolistv1": "Server=localhost;Port=5432;Database=todolist;Username=postgres;Password=anhnguyen;"
 }
```

## Adding the ITodo interface and TodoRepository which will inherit the Itodo interface , these two will handle the database-related operations :
Creating the folder named Repository then create two classes Itodo.cs and TodoRepository.cs :

Itodo.cs :

``` Csharp
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
```

TodoRepository.cs :

``` Csharp
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

```

## Adding Controller to the Application:
First , I add TodoController.cs , because this need authorization before handle data so I put [Authorize] to the file:

``` Csharp
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

        // POST: api/v1/todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _Itodo.AddTodo(todo);
            return await Task.FromResult(todo);

        }

        // DELETE: api/v1/Todo/5
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
```

Then I added LoginController.cs. This controller action method accepts email and password as input. If the email and password are valid then it will return the access token , otherwise it will return a bad request error:

``` Csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todolistv1.Data;
using todolistv1.Models;
using Microsoft.EntityFrameworkCore;

namespace todolistv1.Controllers
{
    [Route("api/v1/signin")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly todolistv1Context _context;

        public LoginController(IConfiguration config, todolistv1Context context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email)
                        
                  
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
```

Then I added the PasswordController.cs for changing password , this action also need authorization: 

``` Csharp
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
    [Route("api/v1/changePassword")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
       
            private readonly todolistv1Context _context;

            public PasswordController(todolistv1Context context)
            {
                _context = context;
            }

        // PUT: api/v1/changePassword/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
            public async Task<IActionResult> PutUser(int id, User user)

            {
             if (id != user.Id)
              {
                 return BadRequest();

              }
             user.UpdateDate = DateTime.UtcNow;
             _context.Entry(user).State = EntityState.Modified;

            try
                {
                     await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

        private bool UserExists(int id)
            {
                return _context.User.Any(e => e.Id == id);
            }
        }
    }

```

Creating UsersController.cs for sign up actions:

``` Csharp
namespace todolistv1.Controllers
{
    [Route("api/v1/signup")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly todolistv1Context _context;

        public UsersController(todolistv1Context context)
        {
            _context = context;
        }

        // POST: api/v1/signup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}

```
## Adding token setting to the appsetting.json:
``` Csharp
"Jwt": {
    "Key": "61babdf7222a67aa284a04705c9a02fe8563dd4ea766f51b307e14f72921913a",
    "Issuer": "JWTAuthenticationServer",
    "Audience": "JWTServicePostmanClient",
    "Subject": "JWTServiceAccessToken"
  }
```

## Adding the "todolistv1Context" , "Itodo" , "TodoRepository", JWT authentication settings reference to the "Program.cs" 
I opened the program.cs and add these commands :

``` Csharp
using todolistv1.Data;
using todolistv1.Repository;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<todolistv1Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("todolistv1") ?? throw new InvalidOperationException("Connection string 'todolistv1Context' not found.")));

// Add services to the container.
builder.Services.AddTransient<ITodo, TodoRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


app.UseAuthorization();
```

In the AddAuthentication code , I configured authorization middleware in the startup. Then it is possible to pass the security key when creating the token and enabled validation of Issuer and Audience. I also set “SaveToken” to true, which stores the bearer token in HTTP Context so I can use the token later in the controller.

## Create table in Postgre database:
Go to the Package Manager Console then write two commands :
+ Add-Migration InitialCreate
+ Update-Database

## Running the code: 
I opened the postman and start to test the code:
#### POST /api/v1/signup: Sign up as an user of the system, using email & password :

![signup](https://user-images.githubusercontent.com/101724167/200093720-6e0e5bdb-4f89-4c80-b46d-75cffb97db8f.png)

In Postgre database :

![post1](https://user-images.githubusercontent.com/101724167/200093972-088d9081-81a3-4150-920a-fe6602765539.png)

#### POST /api/v1/signin: Sign in using email & password. The system will return the JWT token that can be used to call the APIs that follow

![siginin](https://user-images.githubusercontent.com/101724167/200094034-dd04a95b-c5a3-40ce-affd-6c80b0232789.png)

Then I go to the authorization -> choose Type : "Bearer Token" -> Then paste the token :

![token](https://user-images.githubusercontent.com/101724167/200094116-ba4346d8-d1d2-496b-8a07-1093d8ed1a25.png)

#### PUT /api/v1/changePassword: Change user’s password :

![changepassword](https://user-images.githubusercontent.com/101724167/200094178-a138d556-96ea-4435-a18f-ffa32149e639.png)

In Postgre database:

![post2](https://user-images.githubusercontent.com/101724167/200094216-6acf7258-27b6-4b77-b711-34611feb2b05.png)

#### POST /api/v1/todos: Create a new todo item : 

![createtodo](https://user-images.githubusercontent.com/101724167/200094313-9025c86f-3c0e-4dd0-b77a-99958af89ed8.png)

In Postgre database:

![post3](https://user-images.githubusercontent.com/101724167/200094330-43daf692-078b-443b-b769-6ae98f53e9fd.png)

#### GET /api/v1/todos?status=[status]: Get a list of todo items. Optionally, a status query param can be included to return only items of specific status. If not present, return all items :

![gettodo](https://user-images.githubusercontent.com/101724167/200094389-37964b15-1845-47f8-8dd2-1339bcffb201.png)

#### PUT /api/v1/todos/:id: Update a todo item :

![puttodo](https://user-images.githubusercontent.com/101724167/200094454-1da792aa-66ac-4400-92f4-fe73f6ca00b6.png)

In Postgre database: 

![post4](https://user-images.githubusercontent.com/101724167/200094500-759df204-69c4-4ef3-a14b-df82557681cf.png)

#### DELETE /api/v1/todos/:id: Delete a todo item :

![deletetodo](https://user-images.githubusercontent.com/101724167/200094604-9193e4e4-eda8-4e10-a7bd-c78dcc5f90fb.png)

In postgre database: 

![image](https://user-images.githubusercontent.com/101724167/200094634-84aae83b-f986-4a8f-bc99-8374ce78cdd0.png)



--->

