using controlAcademico_web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace controlAcademico_web.Services
{
    public interface IAlumnoService
    {
        Task<List<AlumnoModel>> Lista();
        Task<AlumnoModel> RegistroPorCodigo(int codigoRegistro);
        Task<string> Guardar(AlumnoModel guardarModelo);
        Task<string> Editar(AlumnoModel guardarModelo);
    }
    public class AlumnoService : IAlumnoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string? _usuario;
        private static string? _clave;
        private static string? _baseUrl;
        private static string? _token;
        private static string? _endpoint = "/api/Alumno";
        private static string? _endpointLogin = "/api/Login";

        public AlumnoService(IHttpContextAccessor httpContextAccessor)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _usuario = builder.GetSection("ApiSettings:usuario").Value;
            _clave = builder.GetSection("ApiSettings:clave").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Editar(AlumnoModel guardarModelo)
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
                var response = await cliente.PutAsync($"{_endpoint}/{guardarModelo.codigoAlumno}", content);

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

        public async Task<string> Guardar(AlumnoModel guardarModelo)
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


        public async Task<List<AlumnoModel>> Lista()
        {
            List<AlumnoModel>? listaModelo = new List<AlumnoModel>();
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
                    var resultado = JsonConvert.DeserializeObject<List<AlumnoModel>>(json_respuesta);
                    listaModelo = resultado;
                }
            }
            catch (Exception ex)
            {
                listaModelo = new List<AlumnoModel>();
            }
            return listaModelo!;
        }

        public async Task<AlumnoModel> RegistroPorCodigo(int codigoRegistro)
        {
            AlumnoModel? respuestaModelo = new AlumnoModel();

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
                    var resultado = JsonConvert.DeserializeObject<AlumnoModel>(json_respuesta);
                    respuestaModelo = resultado;
                }

            }
            catch (Exception ex)
            {
                respuestaModelo = new AlumnoModel();
            }

            return respuestaModelo!;
        }
    }
}
