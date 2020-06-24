using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Vets.Data;

namespace Vets.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// variavel que contem os dados do ambiente do servidor
        /// Em particular, onde estão os ficheiros guardados, no disco rigido do servidor
        /// </summary>
        private readonly IWebHostEnvironment _ambiente;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment ambiente)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _ambiente = ambiente;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres, e no maximo {1} caracteres tamanho.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "A password e a sua confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Atributo para recolher o nome do utilizador que se regista
            /// </summary>
            [Required(ErrorMessage = "O Nome é de preenchimento obrigatório.")]
            [StringLength(40, ErrorMessage = "=O {0} não pode ter mais de {1} caracteres.")]
            [RegularExpression("[A-ZÂÓÍÉ][a-záéíóúàèìòùâêîôûãôûäëïöüçñ]+(( | d[oa](s)? | (d)?e |-|'| d')[A-ZÂÓÍÉ][a-záéíóúàèìòùâêîôûãôûäëïöüçñ]+){1,3}",
             ErrorMessage = "Só são aceites letras.<br />A primeira letra de cada nome é uma Maiúscula seguida de minúsculas.<br />Deve escrever entre 2 e 4 nomes.")]
            public string Nome { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// este metodo é assionado quando os dados do formulário são enviados em modo post
        /// </summary>
        /// <param name="fotoUser">Fotografia do novo utilizador</param>
        /// <param name="returnUrl">caso exista, define para onde se reencaminha a ação do programa apos registo</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(IFormFile fotoUser, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            //não estamos a trabalhar com ferramentas externas para o Registo de um novo Utilizador
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)//refere-se ao InputModel (linhas 44)
            {
                //processar o ficheiro da Fotografica
                string caminhoCompleto = "";
                string nomeFoto = "";
                bool haimagem = false;
                //será que há ficheiro?
                if (fotoUser == null)
                {
                    //não há ficheiro!
                    //   - devolver o controlo para a View, informando que é necessário escolher uma fotografia
                    //       ModelState.AddModelError("", "Não se esqueça de adicionar uma fotografia do Veterinário");
                    //       return View(veterinario);
                    //   - adicionar uma fotografia 'por defeito'
                    nomeFoto = "noFoto.png";
                }
                else
                {
                    //Há ficheiro
                    //será que é uma imagem
                    if (fotoUser.ContentType == "image/jpeg" || fotoUser.ContentType == "image/png")
                    {
                        // temos imagem. Ótimo!
                        // temos de gerar um nome para o ficheiro
                        Guid g;
                        g = Guid.NewGuid();
                        // identificar a Extensão do ficheiro
                        string extensao = Path.GetExtension(fotoUser.FileName).ToLower();
                        // nome do ficheiro
                        string nome = g.ToString() + extensao;
                        // preparar o ficheiro para ser guardado, mas não o vamos guardar já
                        // precisamos de identificar o caminho onde o ficheiro vai ser guardado
                        caminhoCompleto = Path.Combine(_ambiente.WebRootPath, "Imagens\\Users", nome);
                        // associar o nome da fotografia ao Veterinário 
                        nomeFoto = nome;
                        // assinalar que existe imagem
                        haimagem = true;
                    }
                    else
                    {
                        // há ficheiro, MAS não é uma imagem
                        // o que vai ser feito?
                        //   - devolver o controlo para a View, informando que é necessário escolher uma fotografia
                        //       ModelState.AddModelError("", "Não se esqueça de adicionar uma fotografia do Veterinário");
                        //       return View(veterinario);
                        //   - adicionar uma fotografia 'por defeito'
                        nomeFoto = "noFoto.png";
                    }
                }
                //criação de um novo utilizador
                var user = new ApplicationUser { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    Nome=Input.Nome, 
                    Fotografia=nomeFoto,
                    Timestamp = DateTime.Now 
                };
                //vou escrever esses dados na BD
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //se cheguei aqui, é porque houve sucesso na escrita na BD

                    //vou agora guardar a imagem no disco rigido do servidor
                    //se ha imagem vou guarda-la no disco rigido
                    if (haimagem)
                    {
                        using var stream = new FileStream(caminhoCompleto, FileMode.Create);
                        await fotoUser.CopyToAsync(stream);
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
