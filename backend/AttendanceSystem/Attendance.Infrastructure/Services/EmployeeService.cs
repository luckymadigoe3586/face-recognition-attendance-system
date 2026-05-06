using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Core.DTOs;
using Attendance.Core.Entities;
using Attendance.Core.Interfaces;
using Attendance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDto?> UploadFaceImageAsync(int employeeId, string imagePath)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                return null;

            employee.FaceImagePath = imagePath;

            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                RegistrationNumber = employee.RegistrationNumber,
                FaceImagePath = employee.FaceImagePath,
                CreatedAt = employee.CreatedAt
            };
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var emailExists = await _context.Employees.AnyAsync(e => e.Email == dto.Email);
            if (emailExists)
                throw new InvalidOperationException("An employee with this email already exists.");

            var registrationExists = await _context.Employees.AnyAsync(e => e.RegistrationNumber == dto.RegistrationNumber);
            if (registrationExists)
                throw new InvalidOperationException("An employee with this registration number already exists.");

            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Department = dto.Department,
                RegistrationNumber = dto.RegistrationNumber,
                

            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                FaceImagePath = employee.FaceImagePath,
                RegistrationNumber = employee.RegistrationNumber,

                CreatedAt = employee.CreatedAt
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeDto
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Email = e.Email,
                    Department = e.Department,
                    RegistrationNumber = e.RegistrationNumber,
                    FaceImagePath = e.FaceImagePath,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                RegistrationNumber = employee.RegistrationNumber,
                FaceImagePath = employee.FaceImagePath,
                CreatedAt = employee.CreatedAt
            };
        }

        public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return null;

            employee.FullName = dto.FullName;
            employee.Email = dto.Email;
            employee.Department = dto.Department;
            employee.RegistrationNumber = dto.RegistrationNumber;

            await _context.SaveChangesAsync();

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Email = employee.Email,
                Department = employee.Department,
                RegistrationNumber = employee.RegistrationNumber,
                FaceImagePath = employee.FaceImagePath,
                CreatedAt = employee.CreatedAt
            };
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
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
