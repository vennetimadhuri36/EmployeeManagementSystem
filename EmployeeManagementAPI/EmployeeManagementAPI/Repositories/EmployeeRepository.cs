using EmployeeManagementAPI.Interfaces;
using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Data;


namespace EmployeeManagementAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> SearchAsync(string name)
        {
            return await _context.Employees
                .Where(e => e.Name.Contains(name))
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(
            int id, Employee employee)
        {
            var existing = await _context.Employees.FindAsync(id);

            if (existing == null)
                return false;

            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.Salary = employee.Salary;
            existing.Email = employee.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}