using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagedList;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class ItemController : Controller    
    {

        private readonly ApplicationDbContext _db;


        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }


        //public IActionResult Index()
        //{
        //    IEnumerable<Item> objList = _db.Items;

        //    return View(objList);

        //}

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {//sorting
            ViewData["CurrentSort"] = sortOrder;
            ViewData["BorrowerSortParm"] = sortOrder== "Borrower"? "Borrower_desc" : "Borrower";

            ViewData["ItemSortParm"] = sortOrder == "ItemName" ? "ItemName_desc" : "ItemName";
            ViewData["LeaderSortParm"]= sortOrder == "Leader" ? "Leader_desc" : "Leader";
            var items = from s in _db.Items
                           select s;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
           
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Borrower.Contains(searchString)
                                       || s.ItemName.Contains(searchString)
                                       || s.Lender.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Borrower":
                    items = items.OrderBy(s => s.Borrower);
                    break;
                case "Borrower_desc":
                    items = items.OrderByDescending(s => s.Borrower);
                    break;
                case "ItemName":
                    items = items.OrderBy(s => s.ItemName);
                    break;
                case "ItemName_desc":
                    items = items.OrderByDescending(s => s.ItemName);
                    break;
                case "Leader":
                    items = items.OrderBy(s => s.ItemName);
                    break;
                case "Leader_desc":
                    items = items.OrderByDescending(s => s.ItemName);
                    break;
                default:
                    items = items.OrderBy(s => s.Lender);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), page ?? 1, pageSize));
        }


        public IActionResult Create()
        {  
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item obj)
        {
            if (ModelState.IsValid)
            {
                _db.Items.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        // GET Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        // POST Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Items.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }


        // GET Update
        public IActionResult Update(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Items.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        // POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Item obj)
        {

            if (ModelState.IsValid)
            {
                _db.Items.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}