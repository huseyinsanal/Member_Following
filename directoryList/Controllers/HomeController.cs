using System.Data.Common;
using System.Diagnostics;
using directoryList.InfraStructure;
using Microsoft.AspNetCore.Mvc;
using directoryList.Models;

namespace directoryList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    
    //Variables
    private static int nextId = 654321;
    
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    
    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult MyDirectory(int page = 1, int pageSize = 3)
    {
        
        // All users
        var allUsers = DataHelper.PersonList;

        // All user count
        var totalUsers = allUsers.Count;

        // Belirli bir sayfayı almak için verileri pageliyoruz
        var pagedUsers = allUsers
            .Skip((page - 1) * pageSize)  // İlk (page - 1) * pageSize kadar kaydır
            .Take(pageSize)               // Sonraki pageSize kadar kullanıcı al
            .ToList();

        // Sayfa sayısı hesaplama
        var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

        // Görünüme verileri gönder
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        return View(pagedUsers);
    }

    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPerson(PersonModel person,string phoneNumber)
    {
        phoneNumber = person.Number;
        
        // Check if the phone number already exist
        var existingPerson = DataHelper.PersonList.FirstOrDefault(p => p.Number == phoneNumber);

        if (person.Number == null)
        {
            ModelState.AddModelError(key: "pn", errorMessage:"Please enter a valid phone number!");
        }
        if ( existingPerson != null )
        {
            // Add an error to ModelState if a match is found
            ModelState.AddModelError(key: "pn", errorMessage:"This phone number already exists! Please write another phone number." );
            return View("Index", DataHelper.PersonList);
        }

        if (person.Surname == null)
        {
            ModelState.AddModelError(key: "surname", errorMessage: "Surname is required!");
            return View("Index", DataHelper.PersonList);
        }

        if (person.Name == null)
        {
            ModelState.AddModelError(key: "name", errorMessage: "Name is required!");
            return View("Index", DataHelper.PersonList);
        }
        
        person.Id = nextId++;
        
        // If no match is found, add the new person
        DataHelper.PersonList.Add(person);
        return RedirectToAction("MyDirectory");
    }

    [HttpPost]
    public IActionResult RemovePerson(int id)
    {
        var personToRemove = DataHelper.PersonList.FirstOrDefault(p => p.Id == id);

        // If the person is found, remove them from the list
        if (personToRemove != null)
        {
            DataHelper.PersonList.Remove(personToRemove);
        }
        return RedirectToAction("MyDirectory");
    }

    [HttpPost]
    public IActionResult Edit(int id)
    {
        var personToEdit = DataHelper.PersonList.FirstOrDefault(p => p.Id == id);

        if (personToEdit != null)
        {
            return View("Edit", personToEdit);
        }
        return View();
    }

    [HttpPost]
    public IActionResult AddNote(int id, string noteText, DateOnly noteDate)
    {
        var person = DataHelper.PersonList.FirstOrDefault(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var newNote = new NoteModel
        {
            Text = noteText,
            Created = noteDate
        };
        
        DataHelper.AddNoteToPerson(id, newNote);
        
        return View("Edit", person);
    }
    
    [HttpPost]
    public IActionResult AddCall(int id, string callText, DateTime callDate)
    {
        var person = DataHelper.PersonList.FirstOrDefault(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var newCall = new CallingDetailModel
        {
            CallText = callText,
            CallTime = callDate
        };
        
        DataHelper.AddCallToPerson(id, newCall);
        
        return View("Edit", person);
    }


}
