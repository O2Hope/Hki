﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
            if (!_signInManager.IsSignedIn(User)) return View();
            
            if (User.IsInRole(Roles.Almacen.ToString()))
                return RedirectToAction("Index", "Almacen");

            if (User.IsInRole(Roles.Produccion.ToString()))
                return RedirectToAction("Index", "Produccion");
                
            if (User.IsInRole(Roles.Calidad.ToString()))               
                return RedirectToAction("Index", "Calidad");

            if (User.IsInRole(Roles.Programacion.ToString()))
                return RedirectToAction("Index", "Programacion");
            
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

                    if (User.IsInRole(Roles.Almacen.ToString()))
                        return RedirectToAction("Index", "Almacen");

                    if (User.IsInRole(Roles.Produccion.ToString()))
                        return RedirectToAction("Index", "Produccion");
                
                    if (User.IsInRole(Roles.Calidad.ToString()))               
                        return RedirectToAction("Index", "Calidad");

                    if (User.IsInRole(Roles.Programacion.ToString()))
                        return RedirectToAction("Index", "Programacion");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Los datos ingresados son incorrectos");
                }
            }

            return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Administrador, Programacion")]
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