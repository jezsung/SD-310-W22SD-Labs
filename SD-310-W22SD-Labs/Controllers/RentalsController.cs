using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD_310_W22SD_Labs.Models;

namespace SD_310_W22SD_Labs.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ShopContext _context;

        public RentalsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public IActionResult Index()
        {
            var rentals = _context.Rentals.Include("Customer").Include("Equipment").Where(rental => rental.IsCurrent).ToList();
            var orderedRentals = rentals.OrderBy(r => r.Customer.Name);
            return View(orderedRentals);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["Customers"] =  _context.Customers.ToList();
            ViewData["Equipments"] =  _context.Equipments.ToList();
            return View();
        }

        // GET: Rentals/BuyRentalHours
        public IActionResult BuyRentalHours()
        {
            ViewData["Customers"] = _context.Customers.ToList();
            return View();
        }

        // GET: Rentals/Expired
        public IActionResult Expired()
        {
            var rentals = _context.Rentals.Include("Customer").Include("Equipment").Where(rental => !rental.IsCurrent).ToList();
            var orderedRentals = rentals.OrderBy(r => r.Customer.Name);
            return View(rentals);
        }

        [HttpPost]
        public IActionResult Create([Bind("CustomerId,EquipmentId,RentalHours")] Rental rental)
        {
            rental.RentedAt = DateTime.Now;
            rental.IsCurrent = true;

            var customer = _context.Customers.Find(rental.CustomerId);
            if(customer == null)
            {
                return NotFound();
            }

            var currentRentalHours = _context.Rentals.Where(rental => rental.IsCurrent).Sum(rental => rental.RentalHours);
            var maxRentalHours = customer.RentalHours;

            if(currentRentalHours > maxRentalHours)
            {
                return BadRequest();
            }

            _context.Add(rental);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult EndRental(int rentalId)
        {
            var rental = _context.Rentals.Find(rentalId);

            if (rental == null)
            {
                return NotFound();
            }

            rental.IsCurrent = false;

            _context.Update(rental);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult BuyRentalHours([Bind("Id", "RentalHours")] Customer input)
        {
            var customer = _context.Customers.Find(input.Id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.RentalHours += input.RentalHours;

            _context.Customers.Update(customer);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
