using System;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Queue.Data;
using Queue.Models;

namespace Queue.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly QueueDbContext _dbcontext;

    public HomeController(ILogger<HomeController> logger,QueueDbContext context)
    {
        _logger = logger;
        _dbcontext=context;
    }

    public IActionResult Index()
    {
        var queues= _dbcontext.queues.ToList();
        return View(queues);
    }

    public IActionResult AdminPage()
    {
        var queues=_dbcontext.queues;
        return View(queues);
    }

    [HttpGet]
    public IActionResult TakeQueue()
    {
        return View();
    }
    [HttpPost]
    public IActionResult TakeQueue([FromForm] QueueModel model)
    {
        var user=new QueueModel();
        user.ID=model.ID;
        user.CustomerName=model.CustomerName;
        user.CreatedAt=model.CreatedAt=DateTimeOffset.UtcNow.ToLocalTime();
        user.Phone=model.Phone;
        user.QueueTime=model.CreatedAt.AddMinutes(45);
        try{
            _dbcontext.queues.Add(user);
            _dbcontext.SaveChanges();
            }
        catch(ArgumentNullException)
        {
            System.Console.WriteLine("Null Keldi");
        }
        return RedirectToAction("ShowQueue",user);
    }
    
    [HttpGet]
    public IActionResult ShowQueue([FromRoute]QueueModel model)
    {
        var client =_dbcontext.queues.FirstOrDefault(u=>u.ID==model.ID);
        return View(client);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
