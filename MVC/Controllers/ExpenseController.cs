using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MVC.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;

        private Budget budget1;

        public ExpenseController(ApplicationDbContext db)
        {
            _db = db;
            budget1 = db.Budgets.Find(1);
        }


        //public IActionResult Index()
        //{
        //    IEnumerable<Expense> objList = _db.Expenses;

        //    return View(objList);


        //}
        public IActionResult Index(string sortOrder, string searchString)
        {//sorting
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ExpenseNameSortParm"] = sortOrder == "ExpenseName" ? "ExpenseName_desc" : "ExpenseName";

            //ViewData["AmountSortParm"] = sortOrder == "Amount" ? "Amount_desc" : "Amount";

            var expenses = from s in _db.Expenses
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                expenses = expenses.Where(s => s.ExpenseName.Contains(searchString));
                //   || s.Amount.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "ExpenseName":
                    expenses = expenses.OrderBy(s => s.ExpenseName);
                    break;
                case "ExpenseName_desc":
                    expenses = expenses.OrderByDescending(s => s.ExpenseName);
                    break;
                //case "Amount":
                //    incomes = incomes.OrderBy(s => s.Amount);
                //    break;
                //case "Amount_desc":
                //    incomes = incomes.OrderByDescending(s => s.Amount);
                //    break;

                default:
                    expenses = expenses.OrderBy(s => s.ExpenseName);
                    break;
            }

            return View(expenses);
        }

        //Get
        public IActionResult Create()
        {
            return View();

        }
        //Post-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Amount < budget1.Amount)
                {
                    budget1.Amount -= obj.Amount;
                    _db.Budgets.Update(budget1);
                    _db.Expenses.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError("Amount", "you dont have enough money, you have only : " + budget1.Amount);
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
            var obj = _db.Expenses.Find(id);
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
            var obj = _db.Expenses.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            budget1.Amount += _db.Expenses.Find(obj.Id).Amount;
            _db.Budgets.Update(budget1);
            _db.Expenses.Remove(obj);
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
            var obj = _db.Expenses.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        // POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Expense obj )
        {
           

            if (ModelState.IsValid)
            {
                if (obj.Amount < budget1.Amount)
                {
                          budget1.Amount += _db.Expenses.Find(obj.Id).Amount;
                          budget1.Amount -= obj.Amount;
                        _db.Budgets.Update(budget1);
                        _db.Expenses.Update(obj);
                        _db.SaveChanges();
                        return RedirectToAction("Index");
                   
                }
                else ModelState.AddModelError("Amount", "you dont have enough money, you have only : " +budget1.Amount);
            }
            return View(obj);
        }
    }
}