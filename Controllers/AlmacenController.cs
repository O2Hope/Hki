using System.Linq;
using System.Threading.Tasks;
using hki.web.Models;
using hki.web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hki.web.Controllers
{
    [Authorize(Roles = "Almacen, Administrador")]
    public class AlmacenController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public AlmacenController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
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
        
        public async Task<IActionResult> Logut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}