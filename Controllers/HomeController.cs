using centralHalcon.Models;
using centralHalcon.Models.ViewModels;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace centralHalcon.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient = new();

    [HttpGet("{query}")]
    public async Task<IActionResult> Index([FromRoute] string query)
    {
        var result = await _httpClient.GetAsync($"https://idtec.websitos256.com/api/Qr/Validar/{query}");
        var json = await result.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(json);

        var datos = JsonConvert.DeserializeObject<List<DatoValor>>(apiResponse.Resp);

        var viewModel = new IndexViewModel
        {
            NumControl = apiResponse.NumControl,
            NombreAlumno = datos.FirstOrDefault(d => d.Dato.Contains("NOMBRE DEL ALUMNO"))?.Valor ?? string.Empty,
            Carrera = datos.FirstOrDefault(d => d.Dato.Contains("CARRERA"))?.Valor ?? string.Empty,
            Vigencia = datos.FirstOrDefault(d => d.Dato.Contains("VIGENCIA"))?.Valor ?? string.Empty,
            Periodo = datos.FirstOrDefault(d => d.Dato.Contains("PERIODO ACTUAL O ULTIMO"))?.Valor ?? string.Empty,
            Curp = datos.FirstOrDefault(d => d.Dato.Contains("CLAVE CURP"))?.Valor ?? string.Empty,
        };

        return View(viewModel);
    }
}