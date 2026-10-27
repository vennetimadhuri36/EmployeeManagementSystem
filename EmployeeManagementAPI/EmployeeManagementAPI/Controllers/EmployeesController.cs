using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Interfaces;
using EmployeeManagementAPI.Repositories;



namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
       
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }
     
      // public EmployeesController(EmployeeDbContext context)
       // {
       //     _context = context;
       // }

        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            return employee;
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            await _repository.AddAsync(employee);

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.Id },
                employee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(
     int id, Employee employee)
        {
            var success = await _repository.UpdateAsync(id, employee);

            if (!success)
                return NotFound("Employee not found");

            var updatedEmployee = await _repository.GetByIdAsync(id);

            return Ok(updatedEmployee);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _repository.DeleteAsync(id);

            if (!success)
                return NotFound("Employee not found");

            return Ok("Employee deleted successfully");
        }


        [HttpGet("search")]
        public async Task<ActionResult<List<Employee>>> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Please provide an employee name.");

            return await _repository.SearchAsync(name);
        }
    }
}