﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pascu_Bianca_Lab2.Data;
using Pascu_Bianca_Lab2.Models;
using Newtonsoft.Json;
using System.Text;

namespace Pascu_Bianca_Lab2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly LibraryContext _context;
        private string _baseUrl = "https://localhost:7184/api/Customers";

        public CustomersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);

            if (response.IsSuccessStatusCode)
            {
                var customers = JsonConvert.DeserializeObject<List<Customer>>(await response.Content.ReadAsStringAsync());
                return View(customers);
            }
            return NotFound();

        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                return View(customer);
            }
            return NotFound();
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewBag.CityID = new SelectList(_context.Cities, "ID", "CityName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("CustomerID,Name,Adress,BirthDate")] Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(customer);
                var response = await client.PostAsync(_baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message} ");
            }
            return View(customer);
        }


        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.CityID = new SelectList(_context.Cities, "ID", "CityName");
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                return View(customer);
            }
            return new NotFoundResult();
        }


        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("CustomerID,Name,Adress,BirthDate")] Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(customer);
            var response = await client.PutAsync($"{_baseUrl}/{customer.CustomerID}",
            new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }


        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var customer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
                return View(customer);
            }
            return new NotFoundResult();
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind("CustomerID")] Customer customer)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{customer.CustomerID}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message} ");
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'LibraryContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
