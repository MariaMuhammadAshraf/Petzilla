using System.Net;
using System.Net.Mail;
using AptechVisionPetZilla.Data;
using AptechVisionPetZilla.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace AptechVisionPetZilla.Controllers
{
    public class AdminController : Controller
    {


        private readonly PetzillaContext db;
        private readonly IHttpContextAccessor CONTX;

        public AdminController(PetzillaContext db, IHttpContextAccessor cONTX)
        {
            this.db = db;
            this.CONTX = cONTX;
        }

        public IActionResult Dashboard()
        {
            var userrole = CONTX.HttpContext.Session.GetString("Userrole");

            // Redirect if no role (unauthorized)
            if (userrole.IsNullOrEmpty())
            {
                return RedirectToAction("Index", "Home");
            }


            var totalPets = db.Pets.Count();
            var totalRequests = db.AdoptionRequestsHomes.Count();
            var approved = db.AdoptionRequestsHomes.Count(r => r.Status == "Approved");
            var pending = db.AdoptionRequestsHomes.Count(r => r.Status == "Pending");
            var rejected = db.AdoptionRequestsHomes.Count(r => r.Status == "Rejected"); // ✅ Add this line
            // contact work
            var contactCount = db.ContactMessages.Count();
            // Reviews
            var reviewCount = db.Reviews.Count();
            // NGO work
            var totalNgos = db.Ngos.Count();

            // Available NGOs
            var availableNgos = db.Ngos.Count(n => n.AvailabilityStatus);

            // Not available NGOs
            var unavailableNgos = db.Ngos.Count(n => !n.AvailabilityStatus);

            // Pending NGOs
            var pendingNgos = db.Ngos.Count(n => n.Status.ToLower() == "pending");

            // Approved NGOs
            var approvedNgos = db.Ngos.Count(n => n.Status.ToLower() == "approved");


            var monthlyCounts = db.AdoptionRequestsHomes
                .Where(r => r.RequestedOn.HasValue)
                .GroupBy(r => r.RequestedOn.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToList();

            var monthLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var data = new int[12];
            foreach (var item in monthlyCounts)
            {
                data[item.Month - 1] = item.Count;
            }

            ViewBag.TotalPets = totalPets;
            ViewBag.TotalRequests = totalRequests;
            ViewBag.ApprovedRequests = approved;
            ViewBag.PendingRequests = pending;
            ViewBag.RejectedRequests = rejected;
            //contact work
            ViewBag.ContactCount = contactCount;
            ViewBag.ReviewCount = reviewCount;
            // NGO ✅
            // Send data to ViewBag
            ViewBag.TotalNgos = totalNgos;
            ViewBag.AvailableNgos = availableNgos;
            ViewBag.UnavailableNgos = unavailableNgos;
            ViewBag.PendingNgos = pendingNgos;
            ViewBag.ApprovedNgos = approvedNgos;




            // ✅ Pass to View

            ViewBag.MonthLabels = JsonConvert.SerializeObject(monthLabels);
            ViewBag.MonthlyRequestCounts = JsonConvert.SerializeObject(data);

            return View();

        }




        public IActionResult ManagePets()
        {
            var userrole = CONTX.HttpContext.Session.GetString("Userrole");
            if (userrole.IsNullOrEmpty())
                return RedirectToAction("Index", "Home");

            var pets = db?.Pets?.ToList() ?? new List<Pet>();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(pets);
        }

        public IActionResult StrayPets()
        {
            var strayPets = db.PetsStrays.ToList(); // Fetch all stray pets
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(strayPets);
        }



        // Show Add Pet Form
        public IActionResult AddPet()

        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }





        [HttpPost]
        public IActionResult AddPet(Pet newPet, IFormFile? petImage)
        {
            if (ModelState.IsValid)
            {
                if (petImage != null && petImage.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(petImage.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pets", imageName);

                    // 🛠️ Create folder if it doesn't exist
                    var folder = Path.GetDirectoryName(imagePath);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder!);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        petImage.CopyTo(stream);
                    }

                    // ✅ Save image name in ImageUrl (not ImagePath)
                    newPet.ImageUrl = "/images/pets/" + imageName;
                }

                db.Pets.Add(newPet);
                db.SaveChanges();
                return RedirectToAction("ManagePets");
            }

            return View(newPet);
        }


        public IActionResult AddPetsStray()
        {
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult AddPetsStray(PetsStray newPet, IFormFile? petImage)
        {
            if (ModelState.IsValid)
            {
                if (petImage != null && petImage.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(petImage.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pets", imageName);

                    // 🛠️ Create folder if it doesn't exist
                    var folder = Path.GetDirectoryName(imagePath);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder!);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        petImage.CopyTo(stream);
                    }

                    // ✅ Save image name in ImageUrl (not ImagePath)
                    newPet.ImageUrl = "/images/pets/" + imageName;
                }

                db.PetsStrays.Add(newPet);
                db.SaveChanges();
                return RedirectToAction("StrayPets");
            }

            return View(newPet);
        }


        // GET: Edit Pet Form
        // GET: Show Edit Form
        // GET: Edit Pet
        public IActionResult Edit(int id)
        {
            var pet = db.Pets.FirstOrDefault(p => p.PetId == id);
            if (pet == null)
                return NotFound();

            return View(pet);
        }

        // POST: Handle Edit Submission
        [HttpPost]
        public IActionResult Edit(Pet updatedPet, IFormFile? petImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedPet);
            }

            var existingPet = db.Pets.FirstOrDefault(p => p.PetId == updatedPet.PetId);
            if (existingPet == null)
                return NotFound();

            // Update basic fields
            existingPet.PetName = updatedPet.PetName;
            existingPet.Category = updatedPet.Category;
            existingPet.Age = updatedPet.Age;
            existingPet.Description = updatedPet.Description;
            existingPet.IsAvailable = updatedPet.IsAvailable;

            // If new image is uploaded
            if (petImage != null && petImage.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(petImage.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pets", imageName);

                // Create folder if not exists
                var folderPath = Path.GetDirectoryName(imagePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath!);

                // Save the new image
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    petImage.CopyTo(stream);
                }

                // Set new image URL
                existingPet.ImageUrl = "/images/pets/" + imageName;
            }

            db.SaveChanges();
            return RedirectToAction("ManagePets");
        }

        //________________________________________________________________________________________________
        // GET: Edit Stray Pet
        // GET: Edit Stray Pet
        public IActionResult EditStray(int id)
        {
            var pet = db.PetsStrays.FirstOrDefault(p => p.PetId == id);
            if (pet == null)
                return NotFound();

            return View(pet);
        }

        // POST: Edit Stray Pet
        [HttpPost]
        public IActionResult EditStray(PetsStray updatedPet, IFormFile? petImage)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedPet);
            }

            var existingPet = db.PetsStrays.FirstOrDefault(p => p.PetId == updatedPet.PetId);
            if (existingPet == null)
                return NotFound();

            // Update fields
            existingPet.Category = updatedPet.Category;
            existingPet.Description = updatedPet.Description;
            existingPet.IsAvailable = updatedPet.IsAvailable;

            // Update image if new one is uploaded
            if (petImage != null && petImage.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(petImage.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pets", imageName);

                var folderPath = Path.GetDirectoryName(imagePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath!);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    petImage.CopyTo(stream);
                }

                existingPet.ImageUrl = "/images/pets/" + imageName;
            }

            db.SaveChanges();

            return RedirectToAction("StrayPets");
        }


        //________________________________________________________________________________________________

        // GET: Confirm Delete View (optional)



        [HttpPost]
        public IActionResult DeletePets(int id)
        {
            var pet = db.Pets.FirstOrDefault(p => p.PetId == id);
            if (pet == null) return NotFound();

            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("ManagePets");
        }

        [HttpPost]

        public IActionResult DeleteStrayPets(int petId)
        {
            var pet = db.PetsStrays.FirstOrDefault(p => p.PetId == petId);
            if (pet == null)
                return NotFound();

            db.PetsStrays.Remove(pet);
            db.SaveChanges();

            return RedirectToAction("StrayPets");
        }



        public async Task<IActionResult> ViewAdoptionRequests()
        {
            var requests = await db.AdoptionRequestsHomes
                .Include(r => r.Pet)
                .ToListAsync();

            return View(requests);
        }




        public IActionResult ApproveRequest(int id)
        {
            var request = db.AdoptionRequestsHomes
                            .Include(r => r.Pet) // include Pet relation
                            .FirstOrDefault(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "approved"; // use consistent casing

            if (request.Pet != null)
            {
                request.Pet.IsAvailable = false; // 🔴 Mark pet as unavailable
            }

            db.SaveChanges();

            return RedirectToAction("ViewAdoptionRequests");
        }


        public IActionResult RejectRequest(int id)
        {
            var request = db.AdoptionRequestsHomes.FirstOrDefault(r => r.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "rejected";
            db.SaveChanges();

            return RedirectToAction("ViewAdoptionRequests");
        }



        public IActionResult Analytics()
        {
            var totalPets = db.Pets.Count();
            var totalRequests = db.AdoptionRequestsHomes.Count();
            var approved = db.AdoptionRequestsHomes.Count(r => r.Status == "Approved");
            var pending = db.AdoptionRequestsHomes.Count(r => r.Status == "Pending");
            var rejected = db.AdoptionRequestsHomes.Count(r => r.Status == "Rejected"); // ✅ Add this line
            // contact work
            var contactCount = db.ContactMessages.Count();

            // Reviews
            var reviewCount = db.Reviews.Count();

            // NGO work
            var totalNgos = db.Ngos.Count();

            // Available NGOs
            var availableNgos = db.Ngos.Count(n => n.AvailabilityStatus);

            // Not available NGOs
            var unavailableNgos = db.Ngos.Count(n => !n.AvailabilityStatus);

            // Pending NGOs
            var pendingNgos = db.Ngos.Count(n => n.Status.ToLower() == "pending");

            // Approved NGOs
            var approvedNgos = db.Ngos.Count(n => n.Status.ToLower() == "approved");


            var monthlyCounts = db.AdoptionRequestsHomes
                .Where(r => r.RequestedOn.HasValue)
                .GroupBy(r => r.RequestedOn.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToList();

            var monthLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var data = new int[12];
            foreach (var item in monthlyCounts)
            {
                data[item.Month - 1] = item.Count;
            }

            ViewBag.TotalPets = totalPets;
            ViewBag.TotalRequests = totalRequests;
            ViewBag.ApprovedRequests = approved;
            ViewBag.PendingRequests = pending;
            ViewBag.RejectedRequests = rejected;
            //contact work
            ViewBag.ContactCount = contactCount;
            ViewBag.ReviewCount = reviewCount;

            // NGO ✅
            // Send data to ViewBag
            ViewBag.TotalNgos = totalNgos;
            ViewBag.AvailableNgos = availableNgos;
            ViewBag.UnavailableNgos = unavailableNgos;
            ViewBag.PendingNgos = pendingNgos;
            ViewBag.ApprovedNgos = approvedNgos;




            // ✅ Pass to View

            ViewBag.MonthLabels = JsonConvert.SerializeObject(monthLabels);
            ViewBag.MonthlyRequestCounts = JsonConvert.SerializeObject(data);

            return View();
        }
        // admin see contact list contact work
        public IActionResult ContactMessages()
        {
            var userrole = CONTX.HttpContext.Session.GetString("Userrole");
            if (userrole.IsNullOrEmpty())
                return RedirectToAction("Index", "Home");

            var messages = db.ContactMessages
                            .OrderByDescending(c => c.SubmittedAt)
                            .ToList();

            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(messages);
        }
        // Dynamic about us
        public IActionResult ManageAbout()
        {
            var sections = db.AboutSections.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();

            return View(sections);
        }

        [HttpGet]
        public IActionResult AddAbout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAbout(AboutSection model, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                model.ImagePath = "/images/" + fileName;
            }
            db.AboutSections.Add(model);
            db.SaveChanges();
            return RedirectToAction("ManageAbout");
        }

        public IActionResult DeleteAbout(int id)
        {
            var about = db.AboutSections.Find(id);
            if (about != null)
            {
                db.AboutSections.Remove(about);
                db.SaveChanges();
            }
            return RedirectToAction("ManageAbout");
        }
        [HttpGet]
        public IActionResult EditAbout(int id)
        {
            var about = db.AboutSections.Find(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        [HttpPost]
        public IActionResult EditAbout(AboutSection model, IFormFile? ImageFile)
        {
            var existing = db.AboutSections.Find(model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = model.Title;
            existing.Description = model.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                existing.ImagePath = "/images/" + fileName;
            }

            db.SaveChanges();
            return RedirectToAction("ManageAbout");
        }
        //Faqs

        // Admin side
        public IActionResult ManageFaq()
        {
            var faqs = db.Faqs.ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(faqs);
        }

        [HttpPost]
        public IActionResult AddFaq(Faq model)
        {
            if (ModelState.IsValid)
            {
                db.Faqs.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("ManageFaq");
        }

        public IActionResult DeleteFaq(int id)
        {
            var faq = db.Faqs.Find(id);
            if (faq != null)
            {
                db.Faqs.Remove(faq);
                db.SaveChanges();
            }
            return RedirectToAction("ManageFaq");
        }
        // GET: Edit FAQ
        public IActionResult EditFaq(int id)
        {
            var faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Edit FAQ
        [HttpPost]
        public IActionResult EditFaq(Faq model)
        {
            if (ModelState.IsValid)
            {
                db.Faqs.Update(model);
                db.SaveChanges();
                return RedirectToAction("ManageFaq");
            }
            return View(model);
        }




     


        public async Task<IActionResult> ViewAdoptionRequestsStray()
        {
            var requests = await db.AdoptionRequestsStrays
                .Include(r => r.Pet)
                .ToListAsync();

            return View(requests);
        }




        public IActionResult ApproveRequestStray(int id)
        {
            var request = db.AdoptionRequestsStrays
                            .Include(r => r.Pet)
                            .FirstOrDefault(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "approved";

            if (request.Pet != null)
            {
                request.Pet.IsAvailable = false; // 🔴 Mark stray pet unavailable
            }

            db.SaveChanges();

            return RedirectToAction("ViewAdoptionRequestsStray");
        }


        public IActionResult RejectRequestStray(int id)
        {
            var request = db.AdoptionRequestsStrays.FirstOrDefault(r => r.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = "rejected";
            db.SaveChanges();

            return RedirectToAction("ViewAdoptionRequestsStray");
        }



        //_____________________________________________________________________________

        // Add other admin actions below

        public IActionResult logout()
        {
            // Clear session, authentication, etc.
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }


        //dropdown dynamic category work categories

        public IActionResult Categories()
        {
            var categories = db.PetCategories.ToList();
            return View(categories);
        }

        // Add new category - GET
        public IActionResult AddCategory()
        {
            return View();
        }

        // Add new category - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(PetCategory category)
        {
            if (ModelState.IsValid)
            {
                db.PetCategories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(category);
        }

        // Edit category - GET
        public IActionResult EditCategory(int id)
        {
            var category = db.PetCategories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // Edit category - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(PetCategory category)
        {
            if (ModelState.IsValid)
            {
                db.PetCategories.Update(category);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(category);
        }

        // Delete category
        public IActionResult DeleteCategory(int id)
        {
            var category = db.PetCategories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                db.PetCategories.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Categories");
        }

        ///-----------------Mnanage NGOs-----------------------------
        ///
        // GET: Manage NGOs
        // GET: Manage NGOs
        [HttpGet]
        public IActionResult ManageNGO()
        {
            var ngos = db.Ngos.OrderByDescending(n => n.CreatedAt).ToList();
            ViewBag.PetCategories = db.PetCategories.ToList();
            return View(ngos);
        }

        // GET: Edit NGO
        [HttpGet]
        public IActionResult EditNGO(int id)
        {
            var ngo = db.Ngos.Find(id);
            if (ngo == null)
            {
                TempData["error"] = "NGO not found!";
                return RedirectToAction("ManageNGO");
            }
            return View(ngo);
        }

        // POST: Edit NGO
        [HttpPost]
        public IActionResult EditNGO(Ngo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ngo = db.Ngos.Find(model.NgoId);
            if (ngo == null)
            {
                TempData["error"] = "NGO not found!";
                return RedirectToAction("ManageNGO");
            }

            // Update fields
            ngo.NgoName = model.NgoName;
            ngo.Email = model.Email;
            ngo.PhoneNumber = model.PhoneNumber;
            ngo.Address = model.Address;
            ngo.Branches = model.Branches;
            ngo.AvailabilityStatus = model.AvailabilityStatus;
            ngo.UpdatedAt = DateTime.Now;

            db.SaveChanges();
            TempData["success"] = $"NGO '{ngo.NgoName}' updated successfully!";
            return RedirectToAction("ManageNGO");
        }

        // POST: Approve NGO
        [HttpPost]
        public IActionResult ApproveNGO(int id)
        {
            var ngo = db.Ngos.Find(id);
            if (ngo != null)
            {
                ngo.Status = "Approved";
                ngo.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                TempData["success"] = $"NGO '{ngo.NgoName}' approved successfully!";
            }

            return RedirectToAction("ManageNGO");
        }

        // POST: Delete NGO
        [HttpPost]
        public IActionResult DeleteNGO(int id)
        {
            var ngo = db.Ngos.Find(id);
            if (ngo != null)
            {
                db.Ngos.Remove(ngo);
                db.SaveChanges();
                TempData["success"] = $"NGO '{ngo.NgoName}' deleted successfully!";
            }

            return RedirectToAction("ManageNGO");
        }
    }

}










