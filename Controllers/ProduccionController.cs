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
    [Authorize(Roles = "Produccion, Administrador")]
    public class ProduccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public ProduccionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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
            [Bind("Id,Dia, Descripcion, ProductoId, ValorHrs, Cantidad, Finalizadas,TotalHrs,Asignado,Ubicacion,FechaReq, Validada")]
            Orden model)
        {
            
            var pieza = new Piezas();

            model.Asignado = Roles.Produccion;
            model.Finalizadas = 0;
            model.TotalHrs = model.ValorHrs * model.Cantidad;
            model.Levantamiento = DateTime.Now;
            model.UltModificacion = Roles.Programacion;

            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                
                for (var i = 0; i < model.Cantidad; i++)
                {
                    
                    pieza.Comentarios = "";
                    pieza.Estatus = EstatusP.Null;
                    pieza.Id = i < 9 ? $"{model.Id}-0{ i + 1}" : $"{model.Id}-{i + 1}";
                    pieza.Levantamiento = model.Levantamiento.ToString();
                    pieza.Orden = model.Id ;
                    pieza.Terminado = false;
                    pieza.UltimaModificacion = DateTime.Now.ToString();
                    
                    _context.Add(pieza);
                    
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            

            return RedirectToAction("Mos");

        }

        public IActionResult Mos()
        {
            var modelo = _context.Ordenes.OrderByDescending(o => o.Levantamiento).ToList();

            return View(modelo);
        }

        public async Task<IActionResult> Details(string id)
        {

            var viewModel = new DetailsViewModel
            {
                Orden = await _context.Ordenes
                    .SingleOrDefaultAsync(o => o.Id == id),
                Piezas = await _context.Piezas
                    .Where(o => o.Orden == id ).ToListAsync()
            };


            if (viewModel.Orden == null)
            {
                return NotFound();
            }

            return View(viewModel);
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
            ordenToUpdate.UltModificacion = Roles.Produccion;
            if (await TryUpdateModelAsync<Orden>(
                ordenToUpdate,
                "",
                o => o.Terminado, o => o.Ubicacion, o => o.Estatus2))
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
        
        public async Task<IActionResult> EditProd(string id)
        {
            var pieza = await _context.Piezas.FirstOrDefaultAsync(p => p.Id == id);
            return View(pieza);
        }
        
        [HttpPost, ActionName("EditProd")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProdPost(string id)
        {
            var ordenToUpdate = await _context.Piezas.SingleOrDefaultAsync(o => o.Id == id);
                ordenToUpdate.Surtir = DateTime.Now;
            
            if (await TryUpdateModelAsync<Piezas>(
                ordenToUpdate,
                "",
                o => o.Surtir, o => o.Comentarios, o => o.Terminado, o => o.Ubicacion, o => o.Estatus))
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
       
       
    }
}