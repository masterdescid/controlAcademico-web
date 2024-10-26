using Microsoft.AspNetCore.Mvc;
using controlAcademico_web.Services;
using controlAcademico_web.Models;

namespace controlAcademico_web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService? _objetoService;
        private readonly IRolService? _rolService;

        public UsuarioController(IUsuarioService? objetoService,IRolService rolService)
        {
            _objetoService = objetoService;
            _rolService = rolService;
        }

        //Pantalla del listado de todos los registros
        public async Task<IActionResult> Index()
        {
            List<UsuarioModel> listaModelo = new List<UsuarioModel>();
            listaModelo = await _objetoService!.Lista();
            return View(listaModelo);
        }

        //Pantalla del formulario crear vacío
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            List<RolModel> listadoRoles = new List<RolModel>();
            listadoRoles = await _rolService!.Lista();
            ViewBag.listadoRoles = listadoRoles;
            return View();
        }

        //Evento para guardar un registro nuevo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(UsuarioModel guardarModelo)
        {

            if (ModelState.IsValid)
            {
                //TODO: Quemado mientras se hace lo de la autenticación

                string respuestaModelo = string.Empty;

                respuestaModelo = await _objetoService!.Guardar(guardarModelo);

                if (respuestaModelo.Equals("OK"))
                {
                    ViewBag.javaScript = "ejecutarToast();";

                    return View();
                }
                else
                {
                    ViewBag.mensajeError = respuestaModelo;
                    ViewBag.javaScript = "ejecutarToastWarning();";
                    return View();
                }
            }
            ViewBag.mensajeError = "Validaciones erroneas.";
            ViewBag.javaScript = "ejecutarToastWarning();";
            return View();
        }

        //Pantalla del formulario editar lleno
        [HttpGet]
        public async Task<IActionResult> Editar(int codigoRegistro)
        {
            UsuarioModel respuestaModelo = new UsuarioModel();
            respuestaModelo = await _objetoService!.RegistroPorCodigo(codigoRegistro);

            List<RolModel> listadoRoles = new List<RolModel>();
            listadoRoles = await _rolService!.Lista();

            if (respuestaModelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            ViewBag.listadoRoles = listadoRoles;
            return View(respuestaModelo);
        }

        //Evento para modificar un registro existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuarioModel guardarModelo)
        {
            if (ModelState.IsValid)

            {
                string respuestaModelo = string.Empty;

                respuestaModelo = await _objetoService!.Editar(guardarModelo);

                if (respuestaModelo.Equals("OK"))
                {
                    ViewBag.javaScript = "ejecutarToast();";
                    return View();
                }
                else
                {
                    ViewBag.mensajeError = respuestaModelo;
                    ViewBag.javaScript = "ejecutarToastWarning();";
                    return View();
                }
            }
            ViewBag.mensajeError = "Validaciones erroneas.";
            ViewBag.javaScript = "ejecutarToastWarning();";
            return View();
        }

        [HttpGet]
        public async Task<string> EditarEliminar(int codigoRegistro)
        {

            string respuestaModelo = string.Empty;
            UsuarioModel guardarModelo = new UsuarioModel();
            guardarModelo = await _objetoService!.RegistroPorCodigo(codigoRegistro);

            if (guardarModelo.codigoUsuario > 0)
            {
                //TODO: Quemado mientras se hace lo de la autenticación
                guardarModelo.estatus = 3;
                guardarModelo.fechaRegistro = DateTime.Now;
                guardarModelo.correo = "eliminado" + guardarModelo.fechaRegistro.ToString();

                respuestaModelo = await _objetoService!.Editar(guardarModelo);

                if (respuestaModelo.Equals("OK"))
                {
                    return "{'codigo': 0, 'texto': 'Proceso realizado exitosamente'}";
                    //return RedirectToAction("Index", "Agencia");
                }
                else
                {
                    return "{'codigo': 1, 'texto': 'Ocurrió un error'}";
                    //return NoContent();
                }
            }
            else
            {
                return "{'codigo': 2, 'texto': 'No se encontró el registro.'}";
            }


        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            string respuestaModelo = await _objetoService!.Login(correo, clave);

            if (respuestaModelo.Equals("OK"))
            {

                // Aquí puedes redirigir al usuario a la página principal o a la página deseada después de iniciar sesión.
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.mensajeError = respuestaModelo; // Mostrar mensaje de error.
                return View();
            }
        }
    }
}
