using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<List<Employee>> SearchAsync(string name);

        Task AddAsync(Employee employee);

        Task<bool> UpdateAsync(int id, Employee employee);

        Task<bool> DeleteAsync(int id);
    }
}
