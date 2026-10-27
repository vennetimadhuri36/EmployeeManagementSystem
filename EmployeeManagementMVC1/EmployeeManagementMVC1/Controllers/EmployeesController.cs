using EmployeeManagementMVC1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EmployeeManagementMVC1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public EmployeesController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = configuration["ApiSettings:BaseUrl"] + "Employees";
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            var data = await response.Content.ReadAsStringAsync();

            var employees = JsonSerializer.Deserialize<List<Employee>>(data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var jsonData = JsonSerializer.Serialize(employee);

            var content = new StringContent(
                jsonData,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(_baseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var data = await response.Content.ReadAsStringAsync();

            var employee = JsonSerializer.Deserialize<Employee>(data,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(employee);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var jsonData = JsonSerializer.Serialize(employee);

            var content = new StringContent(
                jsonData,
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync(
                $"{_baseUrl}/{employee.Id}",
                content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public async Task<IActionResult> Search(string name)
        {
            var response =
                await _httpClient.GetAsync(
                $"{_baseUrl}/search?name={name}");

            var data =
                await response.Content.ReadAsStringAsync();

            var employees =
                JsonSerializer.Deserialize<List<Employee>>(
                    data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View("Index", employees);
        }

    }
}