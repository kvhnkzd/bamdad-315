using App.Domain.Core.Products.AppServices;
using App.Domain.Core.Products.DTOs;
using App.EndPoints.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using ExchangeProxy;
namespace App.EndPoints.MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductAppService _productAppService;
    private readonly IExchageProxy _exchageProxy;

    public HomeController(ILogger<HomeController> logger , IProductAppService productAppService ,IExchageProxy exchageProxy)
    {
        _logger = logger;
        _productAppService = productAppService;
        _exchageProxy = exchageProxy;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(int? id, CancellationToken cancellationToken)
    {
        if (id != null)
        {
            var products = await _productAppService.GetProducts(id??0, cancellationToken);

            var c = id;
            return View(products);
        }
        else
        {
            try
            {
                var products = await _productAppService.GetProducts(cancellationToken);
                return View(products);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }


    }
    [Authorize(Roles = "Admin")]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    public async  Task<IActionResult> Students(int? id, CancellationToken cancellationToken)
    
    {
        var students= await _exchageProxy.GetAllStudents(cancellationToken);
        //HttpClient client = new HttpClient();
        //client.DefaultRequestHeaders.Add("apikey", "ABCabc%40123");
        //var response = await client.GetAsync("https://localhost:7004/api/v1/Student/GetAll");
        //var result = await response.Content.ReadAsStringAsync(cancellationToken);
        //var resultObj = JsonConvert.DeserializeObject<List<Student>>(result);

        await _exchageProxy.SetStudent(students.First(), cancellationToken);

        return View(students);

    }


}
