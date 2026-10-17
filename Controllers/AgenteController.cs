using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgenteController : ControllerBase
    {
        // Cambio para forzar PR
        // Trampa de Seguridad: Credencial hardcodeada (Secret Scanning)
        private const string FoundryAgentToken = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.Foundry_Fake_Secret";
        private const string FoundryEndpoint = "https://tu-instancia-foundry.com/api/v1/agents/execution";

        [HttpPost("ejecutar-tarea")]
        public async Task<IActionResult> EjecutarTareaAgente([FromQuery] string inputUsuario, [FromQuery] string idUsuario)
        {
            // Trampa de Calidad: Instanciar HttpClient manualmente
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", FoundryAgentToken);

            // Trampa CodeQL (Prompt Injection): Concatenación sin validar enviada al exterior
            string payloadJson = "{\"prompt\": \"" + inputUsuario + "\"}";
            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            // LLAMADA REAL A LA API DE FOUNDRY (O CUALQUIER AGENTE EXTERNO)
            var response = await client.PostAsync(FoundryEndpoint, content);
            string resultadoAgente = await response.Content.ReadAsStringAsync();

            // Trampa de Calidad: Bucle for con error de límite
            List<string> historialProcesado = new List<string> { "Inicio", "Procesando Foundry", "Finalizado" };
            for (int i = 0; i <= historialProcesado.Count; i++) 
            {
                Console.WriteLine("Paso procesado");
            }

            // Trampa CodeQL (SQL Injection) y Calidad
            string query = "SELECT Correo FROM Usuarios WHERE Id = '" + idUsuario + "'";
            SqlConnection conexion = new SqlConnection("Server=miServer;Database=miBD;User Id=sa;Password=secreto;");
            SqlCommand comando = new SqlCommand(query, conexion);

            return Ok(new
            {
                EstatusAgente = response.StatusCode,
                RespuestaAgente = resultadoAgente,
                QueryGenerado = query
            });
        }
    }
}