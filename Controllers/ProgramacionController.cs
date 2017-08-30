using System;
using System.Linq;
using System.Threading.Tasks;
using hki.web.Models;
using hki.web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace hki.web.Controllers
{
    [Authorize(Roles = "Programacion, Administrador")]
    public class ProgramacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public ProgramacionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        // GET
        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> NewMo(
            [Bind("Dia, Descripcion, ProductoId, ValorHrs, Cantidad, Finalizadas,TotalHrs,Asignado,Ubicacion,FechaReq")]
            Orden model)
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

        public IActionResult Mos()
        {
            var modelo = _context.Ordenes.OrderByDescending(o => o.Levantamiento).ToList();

            return View(modelo);
        }

        public async Task<IActionResult> Details(string id)
        {

            var orden = await _context.Ordenes
                .SingleOrDefaultAsync(o => o.Id == id);


            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        public async Task<IActionResult> Logut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var orden = await _context.Ordenes.SingleOrDefaultAsync(o => o.Id == id);

            return View(orden);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOst(string id)
        {
            var ordenToUpdate = await _context.Ordenes.SingleOrDefaultAsync(o => o.Id == id);
            if (await TryUpdateModelAsync<Orden>(
                ordenToUpdate,
                "",
                o => o.Dia, o => o.Ubicacion, o => o.Estatus2, o => o.Estatus3))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Mos));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }


            }
            return View();
        }

        public IActionResult Importar()
        {
            return View();
        }
    }
}
