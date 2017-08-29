using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hki.web.Models;
using hki.web.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace hki.web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;

        }
        
        
        [HttpGet] 
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Usuario, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Produccion");
                }
                else
                {
                    ModelState.AddModelError(String.Empty,"Los datos ingresados son incorrectos");
                }
            }

            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid) return View();
            
            var user = new ApplicationUser{UserName = model.UserName};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                  await _userManager.AddToRoleAsync(user, model.Rol.ToString());
                return View();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);

            }

            return View();

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}