using AptechVisionPetZilla.Data;
using AptechVisionPetZilla.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineHelpDesk.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace AptechVisionPetZilla.Controllers
{
    public class HomeController : Controller
    {
        private readonly PetzillaContext db;
        private readonly IHttpContextAccessor CONTX;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _groqApiKey = "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"; // ⚠️ Move to appsettings.json later

        public HomeController(PetzillaContext db, IHttpContextAccessor contx, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.CONTX = contx;
            _webHostEnvironment = webHostEnvironment;


        }

        // GET: Show NGO Registration Form
        [HttpGet]
        public IActionResult RegisterNGO()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }

        // POST: Handle form submission
        [HttpPost]
        public IActionResult RegisterNGO(Ngo model)
        {
            if (!ModelState.IsValid)
            {
                // Show validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                ViewBag.Errors = errors;
                return View(model);
            }

            try
            {
                // Set default values
                model.Status = "Pending";
                model.CreatedAt = DateTime.Now;

                // Add to database
                db.Ngos.Add(model);
                db.SaveChanges();

                TempData["success"] = "NGO registered successfully! Awaiting admin approval.";
                return RedirectToAction("RegisterNGO");
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new List<string> { "Database error: " + ex.Message };
                return View(model);
            }
        }


        // GET: NGOs Overview
        [HttpGet]
        public IActionResult NGOsOverview()
        {
            // Users see only approved NGOs
            var ngos = db.Ngos
                         .Where(n => n.Status == "Approved")
                         .OrderByDescending(n => n.CreatedAt)
                         .ToList();

            return View(ngos);
        }
        [HttpGet]
        public IActionResult NgoDetails(int id)
        {
            var ngo = db.Ngos.FirstOrDefault(x => x.NgoId == id);
            if (ngo == null)
            {
                return NotFound();
            }
            return View(ngo);
        }



        //chatbot Ai work

 public IActionResult Chat()
 {
     return View();
 }

 public class ChatRequest
 {
     public string Message { get; set; }
 }

 [HttpPost("chat")]
 public async Task<IActionResult> Chat([FromBody] ChatRequest input)
 {
     if (string.IsNullOrWhiteSpace(input?.Message))
         return Json(new { response = "⚠️ Please enter a message." });

     try
     {
         using (var client = new HttpClient())
         {
             client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("Bearer", _groqApiKey);

             // ✅ Updated request body with system prompt for PetZilla
             var requestBody = new
             {
                 model = "llama-3.3-70b-versatile",
                 messages = new[]
                 {
             new { role = "system", content = "You are PetZilla, a professional and friendly pet assistant on a website. Always answer questions about pets clearly and politely, give advice on pet care, suggest pet names, and engage users in a helpful manner. Never say you are a computer program or mention a store."  },
             new { role = "user", content = input.Message }
         }
             };

             string json = JsonConvert.SerializeObject(requestBody);
             var content = new StringContent(json, Encoding.UTF8, "application/json");

             var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
             var responseString = await response.Content.ReadAsStringAsync();

             if (!response.IsSuccessStatusCode)
                 return Json(new { response = $"❌ API Error: {response.StatusCode} - {responseString}" });

             // ✅ Parse OpenAI-compatible chat response
             dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
             string reply = jsonResponse?.choices?[0]?.message?.content ?? "No reply from AI.";

             return Json(new { response = reply });
         }
     }
     catch (Exception ex)
     {
         // Always return JSON even if something goes wrong
         return Json(new { response = "❌ Exception: " + ex.Message });
     }
 }

        // Keep all your other methods as-is below 👇

        //public IActionResult Index() => View();
        public IActionResult Index()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }

        //changepassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid data.";
                return View(model);
            }

            string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            var user = db.UserRegistrations.FirstOrDefault(u => u.UserEmail == userEmail);

            if (user == null || user.UserPassword != model.CurrentPassword)
            {
                TempData["error"] = "Current password is incorrect.";
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["error"] = "New passwords do not match.";
                return View(model);
            }

            user.UserPassword = model.NewPassword;
            db.SaveChanges();

            TempData["success"] = "Password changed successfully.";
            return RedirectToAction("ChangePassword");
        }

        // GET: Show user account details
 [HttpGet]
 public IActionResult MyAccount()
 {
     // ? Correct: Fetching session value
     string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

     // ? Correct: Handling if session is expired or missing
     if (string.IsNullOrEmpty(userEmail))
     {
         TempData["error"] = "Session expired. Please login again.";
         return RedirectToAction("Login");
     }

     // ? Correct: Querying the user from the database using email
     var user = db.UserRegistrations.FirstOrDefault(u => u.UserEmail == userEmail);

     // ? Correct: Passing the user model to the view
     ViewBag.PetCategories = db.PetCategories.ToList();
     return View(user);
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public IActionResult MyAccount(UserRegistration model)
 {
     if (!ModelState.IsValid)
     {
         // ? Log model state errors
         foreach (var state in ModelState)
         {
             foreach (var error in state.Value.Errors)
             {
                 Console.WriteLine($"{state.Key}: {error.ErrorMessage}");
             }
         }

         TempData["error"] = "Invalid data submitted.";
         return View(model);
     }

     var user = db.UserRegistrations.FirstOrDefault(u => u.UserId == model.UserId);
     if (user == null)
     {
         TempData["error"] = "User not found.";
         return RedirectToAction("Login");
     }

     // ? Update fields
     user.UserName = model.UserName;
     user.UserEmail = model.UserEmail;

     db.SaveChanges();

     TempData["success"] = "Account updated successfully!";
     return RedirectToAction("MyAccount");
 }



        //about work
        public IActionResult About()
        {
            var abouts = db.AboutSections.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();

            return View(abouts);
        }

        //public IActionResult Contact() => View(new ContactMessage());

        //contact us
        public IActionResult Contact()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(new ContactMessage());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactMessage contact)
        {
            if (ModelState.IsValid)
            {
                contact.SubmittedAt = DateTime.Now;
                db.ContactMessages.Add(contact);
                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["success"] = "Form submitted successfully!";
                    return RedirectToAction("Contact");
                }
                else
                {
                    TempData["error"] = "There was an issue submitting your form.";
                  
                    return View(contact);
                }
            }

            return View(contact);
        }


        ///FAQS   

        // User side
        public IActionResult Faq()
        {
            var faqs = db.Faqs.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(faqs);
        }

        //review
        public ActionResult Reviews()
        {
            var reviews = db.Reviews.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(reviews);
        }

        // submit review
        // GET: Home/SubmitReview
        [HttpGet]
        public IActionResult SubmitReview()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitReview(Review review, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Unique filename banayein
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                    // wwwroot/UploadedImages folder ka path nikalain
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedImages");

                    // Folder create karo agar nahi hai
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var filePath = Path.Combine(uploads, fileName);

                    // File ko save karo
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }

                    // Relative URL DB me store karo
                    review.ImageUrl = "/UploadedImages/" + fileName;
                }

                db.Reviews.Add(review);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thank you! Your review has been submitted.";
                return RedirectToAction("SubmitReview");
            }

            return View(review);
        }


        //login work

       //public IActionResult Login() => View();
        public IActionResult Login()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserRegistration USERVALIDATION)
        {
            if (!ModelState.IsValid)
            {
                var uservalidity = db.UserRegistrations
                    .Where(user => user.UserEmail == USERVALIDATION.UserEmail &&
                                   user.UserPassword == USERVALIDATION.UserPassword)
                    .ToList();

                if (uservalidity.Any())
                {
                    CONTX.HttpContext.Session.SetString("UserName", uservalidity[0].UserName);
                    CONTX.HttpContext.Session.SetString("UserEmail", uservalidity[0].UserEmail);
                    CONTX.HttpContext.Session.SetString("Userrole", uservalidity[0].UserRole);


                    ViewBag.LoginSuccess = true; // ✅ success message bhej diya

                    return uservalidity[0].UserRole == "customer"
                        ? RedirectToAction("Index", "Home")
                        : RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ViewBag.LoginError = "Invalid Email or Password!"; // ❌ agar user nahi mila
                }
            }
            else
            {
                ViewBag.LoginError = "Invalid input. Please try again!";
            }

            return View();
        }
        

       //register work

 [HttpGet]
 //public IActionResult Register() => View();

 public IActionResult Register()
 {
     ViewBag.PetCategories = db.PetCategories.ToList();
     return View();
 }

 [HttpPost]
 public IActionResult Register(UserRegistration newSTD)
 {
     if (ModelState.IsValid)
     {
         // Check if email already exists
         bool emailExists = db.UserRegistrations
                             .Any(u => u.UserEmail == newSTD.UserEmail);

         if (emailExists)
         {
             // Error message for duplicate email
             ModelState.AddModelError("UserEmail", "This email is already registered.");
             ViewBag.PetCategories = db.PetCategories.ToList();
             return View(newSTD);
         }

         // If email is unique, save new record
         newSTD.UserRole = "customer";
         db.UserRegistrations.Add(newSTD);
         db.SaveChanges();

         ViewBag.RegistrationSuccess = true;
         return RedirectToAction("Login"); // Optional: redirect to login page after success
     }

     ViewBag.PetCategories = db.PetCategories.ToList();
     return View(newSTD);
 }






        public IActionResult Report() => View();


        //pet details work
        public IActionResult PetDetails()
        {
            var pets = db.Pets.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(pets);
        }


        //straypet details
        public IActionResult StrayPetDetails(int id)
        {
            var petsStray = db.PetsStrays.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(petsStray);
        }

        // GET: Show the form to request adoption
        // GET: Show the adoption request form for a specific pet
        public IActionResult RequestForHome(int id)
        {
            var userRole = HttpContext.Session.GetString("Userrole");

            if (string.IsNullOrEmpty(userRole))
            {
                TempData["LoginMessage"] = "Please log in to request adoption.";
                return RedirectToAction("Login", "Home");
            }

            if (!userRole.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                TempData["LoginMessage"] = "Only customers are allowed to make adoption requests.";
                return RedirectToAction("Login", "Home");
            }

            // Use Find for primary key lookup, faster and cleaner
            var pet = db.Pets.Find(id);
            if (pet == null) return NotFound();

            ViewBag.Pet = pet;

            return View("RequestForHome", new AdoptionRequestsHome
            {
                PetId = id,
                RequestedOn = DateTime.Now
            });
        }

        // POST: Submit the adoption request form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestForHome(AdoptionRequestsHome adoptionRequest)
        {
            var userRole = HttpContext.Session.GetString("Userrole");

            if (string.IsNullOrEmpty(userRole))
            {
                TempData["LoginMessage"] = "Please log in to request adoption.";
                return RedirectToAction("Login", "Home");
            }

            if (!userRole.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                TempData["LoginMessage"] = "Only customers are allowed to make adoption requests.";
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                adoptionRequest.Status = "Pending";
                adoptionRequest.RequestedOn = DateTime.Now;

                db.AdoptionRequestsHomes.Add(adoptionRequest);
                db.SaveChanges();

                TempData["Success"] = "Your adoption request has been submitted!";
                return RedirectToAction("RequestForHome", new { id = adoptionRequest.PetId });
            }

            // If model is invalid, reload the pet info to show on form
            var pet = db.Pets.Find(adoptionRequest.PetId);
            if (pet == null) return NotFound();

            ViewBag.Pet = pet;

            return View("RequestForHome", adoptionRequest);
        }



        // GET: Show adoption form for stray pet
        public IActionResult RequestForStray(int id)
        {
            var userRole = HttpContext.Session.GetString("Userrole");

            if (string.IsNullOrEmpty(userRole))
            {
                TempData["LoginMessage"] = "Please log in to request adoption.";
                return RedirectToAction("Login", "Home");
            }

            if (!userRole.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                TempData["LoginMessage"] = "Only customers are allowed to make adoption requests.";
                return RedirectToAction("Login", "Home");
            }

            var pet = db.PetsStrays.Find(id);
            if (pet == null) return NotFound();

            ViewBag.Pet = pet;

            return View("RequestForStray", new AdoptionRequestsStray
            {
                PetId = id,
                RequestedOn = DateTime.Now
            });
        }



        // POST: Submit stray pet adoption request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestForStray(AdoptionRequestsStray adoptionRequestStray)
        {
            var userRole = HttpContext.Session.GetString("Userrole");

            if (string.IsNullOrEmpty(userRole))
            {
                TempData["LoginMessage"] = "Please log in to request adoption.";
                return RedirectToAction("Login", "Home");
            }

            if (!userRole.Equals("customer", StringComparison.OrdinalIgnoreCase))
            {
                TempData["LoginMessage"] = "Only customers are allowed to make adoption requests.";
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                adoptionRequestStray.Status = "Pending";
                adoptionRequestStray.RequestedOn = DateTime.Now;

                db.AdoptionRequestsStrays.Add(adoptionRequestStray);
                db.SaveChanges();

                TempData["Success"] = "Your adoption request has been submitted!";
                return RedirectToAction("RequestForStray", new { id = adoptionRequestStray.PetId });
            }

            // Reload pet info if validation fails
            var strayPet = db.PetsStrays.FirstOrDefault(p => p.PetId == adoptionRequestStray.PetId);
            ViewBag.Pet = strayPet;

            return View("RequestForStray", adoptionRequestStray);
        }



        public IActionResult MyRequestsStray()
        {
            string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            // Fetch requests for logged-in user (including Pet info for images)
            var requests = db.AdoptionRequestsStrays
                             .Include(r => r.Pet)
                             .Where(r => r.RequesterEmail == userEmail)
                             .ToList();

            return View(requests);
        }

       public IActionResult MyRequestsHome()
        {
            string userEmail = CONTX.HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["error"] = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            // Fetch requests for logged-in user (including Pet info for images)
            var requests = db.AdoptionRequestsHomes
                             .Include(r => r.Pet)
                             .Where(r => r.RequesterEmail == userEmail)
                             .ToList();

            return View(requests);
        }

        // logout work
      public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
 


    }
}
