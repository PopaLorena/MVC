using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace MVC.Controllers
{
    public class IncomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        private Budget budget1;

        public IncomeController (ApplicationDbContext db)
        {
            _db = db;
            budget1 = db.Budgets.Find(1);
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<Income> objList = _db.Incomes;
        //    return View(objList);
        //}

        public IActionResult Index(string sortOrder, string searchString)
        {//sorting
            
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IncomeNameSortParm"] = sortOrder == "IncomeName" ? "IncomeName_desc" : "IncomeName";

            //ViewData["AmountSortParm"] = sortOrder == "Amount" ? "Amount_desc" : "Amount";
            
            var incomes = from s in _db.Incomes
                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                incomes = incomes.Where(s => s.IncomeName.Contains(searchString));
                                    //   || s.Amount.Contains(searchString));
                                       
            }
            switch (sortOrder)
            {
                case "IncomeName":
                    incomes = incomes.OrderBy(s => s.IncomeName);
                    break;
                case "IncomeName_desc":
                    incomes = incomes.OrderByDescending(s => s.IncomeName);
                    break;
                //case "Amount":
                //    incomes = incomes.OrderBy(s => s.Amount);
                //    break;
                //case "Amount_desc":
                //    incomes = incomes.OrderByDescending(s => s.Amount);
                //    break;
              
                default:
                    incomes = incomes.OrderBy(s => s.IncomeName);
                    break;
            }

            return View(incomes);
        }


        //Get 
        public IActionResult Create()
        {
         
            return View();
        }

        //PostCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Income obj)
        {
            if (ModelState.IsValid)
            {
               
                budget1.Amount += obj.Amount;
                obj.budget = budget1;
                _db.Budgets.Update(budget1);
                _db.Incomes.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Get delete
        public IActionResult Delete(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Incomes.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //postDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Incomes.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            budget1.Amount -= obj.Amount;
            _db.Budgets.Update(budget1);
            _db.Incomes.Remove(obj);
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
            var obj = _db.Incomes.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        // POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Income obj)
        {

            if (ModelState.IsValid)
            {
                budget1.Amount -= _db.Expenses.Find(obj.Id).Amount;
                budget1.Amount += obj.Amount;
                _db.Budgets.Update(budget1);

                _db.Incomes.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}