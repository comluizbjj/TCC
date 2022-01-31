using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Identity.Models;

namespace WebApp.Identity.Controllers
{
    
    public class HomeController : Controller
    {

        public HomeController(UserManager<MyUser> userManager )
        {
            _userManager = userManager;
        }
        //luizprivate readonly ILogger<HomeController> _logger;

        private readonly UserManager<MyUser> _userManager;

        /*luizpublic HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var identtity = new ClaimsIdentity("cookies");
                    //identtity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identtity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    identtity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Nome));
                    identtity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    //identtity.AddClaim(new Claim(ClaimTypes.SerialNumber, user.Cpf));
                    identtity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.Telefone));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identtity));
                    
                    return RedirectToAction("About");
                }

                ModelState.AddModelError("", "Usuário ou Senha inválida.");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password.Length < 4)
                {
                    ModelState.AddModelError("", "A senha deve conter no mínimo 4 caracteres.");
                }else
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user == null)
                    {
                        user = new MyUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = model.Username,
                            Email = model.Email,
                            Cpf = model.Cpf,
                            Nome = model.Nome,
                            Telefone = model.Telefone,
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        return View("Sucess");
                    }
                    ModelState.AddModelError("", "Usuário já criado no sistema.");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Sucess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
