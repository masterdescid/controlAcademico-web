using controlAcademico_web.Models;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace controlAcademico_web.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioModel>> Lista();
        Task<UsuarioModel> RegistroPorCodigo(int codigoRegistro);
        Task<string> Guardar(UsuarioModel guardarModelo);
        Task<string> Editar(UsuarioModel guardarModelo);
        Task<string> Login(string email, string password);

    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static string? _usuario;
        private static string? _clave;
        private static string? _baseUrl;
        private static string? _token;
        private static string? _endpoint = "/api/Usuario";
        private static string? _endpointLogin = "/api/Usuario/login";

        public UsuarioService(IHttpContextAccessor httpContextAccessor)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _usuario = builder.GetSection("ApiSettings:usuario").Value;
            _clave = builder.GetSection("ApiSettings:clave").Value;
            _baseUrl = builder.GetSection("ApiSettings:baseUrl").Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Editar(UsuarioModel guardarModelo)
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
                var response = await cliente.PutAsync($"{_endpoint}/{guardarModelo.codigoUsuario}", content);

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

        public async Task<string> Guardar(UsuarioModel guardarModelo)
        {
            string guardado = string.Empty;

            try
            {

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                // Recuperar el token de la sesión
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                // Establecer el token en los encabezados de autorización
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


        public async Task<List<UsuarioModel>> Lista()
        {
            List<UsuarioModel>? listaModelo = new List<UsuarioModel>();
            try
            {

                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                // Recuperar el token de la sesión
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                // Establecer el token en los encabezados de autorización
                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var response = await cliente.GetAsync(_endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<List<UsuarioModel>>(json_respuesta);
                    listaModelo = resultado;
                }
            }
            catch (Exception ex)
            {
                listaModelo = new List<UsuarioModel>();
            }
            return listaModelo!;
        }

        public async Task<UsuarioModel> RegistroPorCodigo(int codigoRegistro)
        {
            UsuarioModel? respuestaModelo = new UsuarioModel();

            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                // Recuperar el token de la sesión
                var _token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

                // Establecer el token en los encabezados de autorización
                if (!string.IsNullOrEmpty(_token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }
                var response = await cliente.GetAsync($"{_endpoint}/{codigoRegistro}");

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<UsuarioModel>(json_respuesta);
                    respuestaModelo = resultado;
                }

            }
            catch (Exception ex)
            {
                respuestaModelo = new UsuarioModel();
            }

            return respuestaModelo!;
        }
        public async Task<string> Login(string correo, string clave)
        {
            string resultado = "ERROR";
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(_baseUrl!);
                var credenciales = new { correo, clave };
                var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
                var response = await cliente.PostAsync(_endpointLogin, content);

                if (response.IsSuccessStatusCode)
                {
                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    var resultadoLogin = JsonConvert.DeserializeObject<TokenResultadoModel>(json_respuesta); 
                    _token = resultadoLogin?.token;
                    _httpContextAccessor.HttpContext?.Session.SetString("Token", _token!);
                    resultado = "OK";
                }
                else
                {
                    resultado = "Credenciales inválidas.";
                }
            }
            catch (Exception ex)
            {
                resultado = "ERROR: " + ex.Message;
            }

            return resultado;
        }

    }
}
