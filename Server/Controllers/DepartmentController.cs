using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Server.Controllers

{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public DepartmentController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // GET: api/departments/
        [HttpGet("")]
        async public Task<IActionResult> GetDepartments() 
        {
            var departments = await _context.Departments.AsNoTracking().OrderBy((d) => d.DepartmentName).ToListAsync();

            if (departments != null && departments.Count() > 0)
            {
                return new JsonResult(departments);
            }

            return NotFound();
        }  

       // GET: api/departments/1
        [HttpGet("{departmentId}")]
        async public Task<IActionResult> GetDepartment(int departmentId) 
        {
            var department = await _context.Departments.Where((d) => d.DepartmentId == departmentId).AsNoTracking().FirstOrDefaultAsync();

            if (department != null)
            {
                return new JsonResult(department);
            }

            return NotFound();
        }   
    }


}