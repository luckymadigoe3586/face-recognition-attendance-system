using Attendance.Core.DTOs;
using Attendance.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("{id}/upload-face")]
        public async Task<IActionResult> UploadFaceImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No image file uploaded." });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Only JPG, JPEG, and PNG files are allowed." });

            var fileName = $"employee_{id}_{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "faces");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imagePath = $"/faces/{fileName}";

            var updatedEmployee = await _employeeService.UploadFaceImageAsync(id, imagePath);

            if (updatedEmployee == null)
                return NotFound(new { message = "Employee not found." });

            return Ok(new
            {
                message = "Face image uploaded successfully.",
                employee = updatedEmployee
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                var createdEmployee = await _employeeService.CreateEmployeeAsync(dto);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, dto);

            if (updatedEmployee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);

            if (!deleted)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}