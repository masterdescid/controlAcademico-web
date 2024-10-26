﻿using controlAcademico_web.Models;
using controlAcademico_web.Services;
using Microsoft.AspNetCore.Mvc;

namespace controlAcademico_web.Controllers
{
    public class MaestroController : Controller
    {
        private readonly IMaestroService? _objetoService;

        public MaestroController(IMaestroService? objetoService)
        {
            _objetoService = objetoService;
        }

        //Pantalla del listado de todos los registros
        public async Task<IActionResult> Index()
        {
            List<MaestroModel> listaModelo = new List<MaestroModel>();
            listaModelo = await _objetoService!.Lista();
            return View(listaModelo);
        }

        //Pantalla del formulario crear vacío
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        //Evento para guardar un registro nuevo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MaestroModel guardarModelo)
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
            MaestroModel respuestaModelo = new MaestroModel();
            respuestaModelo = await _objetoService!.RegistroPorCodigo(codigoRegistro);

            if (respuestaModelo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(respuestaModelo);
        }

        //Evento para modificar un registro existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(MaestroModel guardarModelo)
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
            MaestroModel guardarModelo = new MaestroModel();


            guardarModelo = await _objetoService!.RegistroPorCodigo(codigoRegistro);

            if (guardarModelo.codigoMaestro > 0)
            {
                //TODO: Quemado mientras se hace lo de la autenticación
                guardarModelo.nombreMaestro = "eliminado" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                guardarModelo.estatus = 3;

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
    }
}