using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signManager;
        public UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;
        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signmanager, UserManager<StoreUser> userManager,IConfiguration config)
		{
			_logger = logger;
			_signManager = signmanager;
            _userManager = userManager;
            _config = config;
        }
		public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","App");  
            }
            return View();
        }
        [HttpPost]  
		public async Task<IActionResult> Login(LoginViewModel login)
		{
			if(ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(login.Username,login.Password,login.Rememberme,false);
                if(result.Succeeded)
                {
                    if (Request.Query.ContainsKey("ReturnUrl"))
                    {
                        Redirect(Request.Query["ReturnUrl"].First());
                    }
                    RedirectToAction("Contact", "App");
                }
            }
            ModelState.AddModelError("", "Erreur de connexion !");
            return View();
		}
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signManager?.SignOutAsync(); 
            return RedirectToAction("Index", "App");
        }
        [HttpPost]
        [Route("auth/api/login")]
        public async  Task<IActionResult> CreateToken([FromBody] LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.Username);
                if(user != null)
                {
                var result = await _signManager.CheckPasswordSignInAsync(user,login.Password,false);
                    if(result.Succeeded) 
                    {
                        // create Token
                        try
                        {
                        var token = GetToken(user);
                        return Created($"", 
                        new {
                            Token = token,
                            ExpireIn = DateTime.UtcNow.AddMinutes(1)
                        }
                        );
                        }catch(Exception ex)
                        {
                            ModelState.AddModelError("Login", "Erreur creation token");
                            return NotFound(ModelState);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "Erreur de connexion user");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("Login", "Erreur de connexion : Utilisateur introuvable");
                    return NotFound(ModelState);
                }
            }
            ModelState.AddModelError("Login", "Erreur de connexion : paramètres non reconnus");
            return BadRequest(ModelState);
        }
        public string GetToken(StoreUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["token:key"]));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Token:Host_Client"],
                _config["Token:Host_App"],
                claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
         }
	}
}
