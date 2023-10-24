using EFCoreMVC.Data;
using EFCoreMVC.Models.SchoolViewModels;
using EFCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;

namespace EFCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolContext _context;

        public HomeController(ILogger<HomeController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> About()
        {
            // Codigo antiguo antes que se aplicara la herencia con la clase Person
            //// La instrucción LINQ agrupa las entidades de alumnos por fecha de inscripción, calcula la cantidad de entidades que se incluyen en cada grupo y almacena los resultados en una colección de objetos de modelo de la vista EnrollmentDateGroup.
            //IQueryable<EnrollmentDateGroup> data = from student in _context.Students
            //                                       group student by student.EnrollmentDate into dateGroup
            //                                       select new EnrollmentDateGroup()
            //                                       {
            //                                           EnrollmentDate = dateGroup.Key,
            //                                           StudentCount = dateGroup.Count()
            //                                       };

            //return View(await data.AsNoTracking().ToListAsync());

            // Codigo actualizado
            {
                List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();
                var conn = _context.Database.GetDbConnection();

                try
                {
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                            + "FROM Person "
                            + "WHERE Discriminator = 'Student' "
                            + "GROUP BY EnrollmentDate";
                        command.CommandText = query;
                        DbDataReader reader = await command.ExecuteReaderAsync();

                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new EnrollmentDateGroup { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
                                groups.Add(row);
                            }
                        }
                        reader.Dispose();
                    }
                }
                finally
                {
                    conn.Close();
                }
                return View(groups);
            }
        }

    }
}
