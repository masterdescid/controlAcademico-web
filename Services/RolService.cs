using controlAcademico_web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace controlAcademico_web.Services
{
    public interface IRolService
    {
        Task<List<RolModel>> Lista();
        Task<RolModel> RegistroPorCodigo(int codigoRegistro);
        Task<string> Guardar(RolModel guardarModelo);
        Task<string> Editar(RolModel guardarModelo);
    }
    public class RolService : IRolService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string? _usuario;
        private static string? _clave;
        private static string? _baseUrl;
        private static string? _token;
        private static string? _endpoint = "/api/Rol";
        private static string? _endpointLogin = "/api/Login";

        public RolService(IHttpContextAccessor httpContextAccessor)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _usuario = builder.GetSection("ApiSettings:usuario").Value;
            _clave = builder.GetSection("ApiSettings:clave").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Editar(RolModel guardarModelo)
        {
            string guardado = string.Empty;

            try
            {

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var content = new StringContent(JsonConvert.SerializeObject(guardarModelo), Encoding.UTF8, "application/json");
                var response = await cliente.PutAsync($"{_endpoint}/{guardarModelo.codigoRol}", content);

                if (response.IsSuccessStatusCode)
                {
                    guardado = "OK";
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (responseBody.Contains("Violation of UNIQUE KEY constraint"))
                        guardado = "El registro que está intentando guardar ya existe en el sistema.";
                    else
                        guardado = "ALERTA: No se guardó la información";
                }

            }
            catch (Exception ex)
            {
                guardado = "ERROR: " + ex.Message;
            }

            return guardado;
        }

        public async Task<string> Guardar(RolModel guardarModelo)
        {
            string guardado = string.Empty;

            try
            {

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var content = new StringContent(JsonConvert.SerializeObject(guardarModelo), Encoding.UTF8, "application/json");
                var response = await cliente.PostAsync(_endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    guardado = "OK";
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (responseBody.Contains("Violation of UNIQUE KEY constraint"))
                        guardado = "El registro que está intentando guardar ya existe en el sistema.";
                    else
                        guardado = "ALERTA: No se guardó la información";
                }

            }
            catch (Exception ex)
            {
                guardado = "ERROR: " + ex.Message;
            }

            return guardado;
        }


        public async Task<List<RolModel>> Lista()
        {
            List<RolModel>? listaModelo = new List<RolModel>();
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var response = await cliente.GetAsync(_endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<RolModel>>(json_respuesta);
                    listaModelo = resultado;
                }
            }
            catch (Exception ex)
            {
                listaModelo = new List<RolModel>();
            }
            return listaModelo!;
        }

        public async Task<RolModel> RegistroPorCodigo(int codigoRegistro)
        {
            RolModel? respuestaModelo = new RolModel();

            try
            {

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var response = await cliente.GetAsync($"{_endpoint}/{codigoRegistro}");

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<RolModel>(json_respuesta);
                    respuestaModelo = resultado;
                }

            }
            catch (Exception ex)
            {
                respuestaModelo = new RolModel();
            }

            return respuestaModelo!;
        }
    }
}
