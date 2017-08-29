using System;
using System.Linq;
using System.Threading.Tasks;
using hki.web.Migrations;
using hki.web.Models;
using hki.web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace hki.web.Controllers
{
    [Authorize(Roles = "Produccion")]
    public class ProduccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public ProduccionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        
        // GET
        public async Task <IActionResult> Index()
        {
            var modelo = new IndexViewModel
            {
                Acabado = _context.Ordenes.Count(u => u.Ubicacion == Ubicaciones.Acabado),
                Calidad = _context.Ordenes.Count(u => u.Ubicacion == Ubicaciones.Calidad),
                Electricos = _context.Ordenes.Count(u => u.Ubicacion == Ubicaciones.Electricos),
                Limpieza = _context.Ordenes.Count(u => u.Ubicacion == Ubicaciones.Limpieza),
                Soldadura = _context.Ordenes.Count(u => u.Ubicacion == Ubicaciones.Soldadura),
                Total = _context.Ordenes.Count(),
                Ordenes = await _context.Ordenes.OrderByDescending(o => o.Levantamiento).Take(5).ToListAsync()
            };



            return View(modelo);
        }

        public IActionResult NewMo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewMo([Bind("Dia, Descripcion, ProductoId, ValorHrs, Cantidad, Finalizadas,TotalHrs,Asignado,Ubicacion,FechaReq")] Orden model)
        {
            
            model.Asignado = Roles.Produccion;
            model.Finalizadas = 0;
            model.TotalHrs = model.ValorHrs * model.Cantidad;
            model.Levantamiento = DateTime.Now;

            if (!ModelState.IsValid) return View(model);
               
            _context.Add(model);
            await _context.SaveChangesAsync();
                
               return RedirectToAction("Index");
               
        }

        public  IActionResult Mos()
        {
            var modelo =  _context.Ordenes.ToList();

            return View(modelo);
        }

       
    }
}