using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entites;
using Infrastructure;
using web.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Core.Intefaces;

namespace web.Controllers
{
    public class PortfolioItemsController : Controller
    {

        private readonly IUnitOfWork<PortfolioItem> _portfolioUnitOfWork;
        private readonly IHostingEnvironment _hosting;

        public PortfolioItemsController(IUnitOfWork<PortfolioItem> portfolioUnitOfWork, IHostingEnvironment hosting)
        {

            _portfolioUnitOfWork = portfolioUnitOfWork;
            _hosting = hosting;
        }

        // GET: PortfolioItems
        public IActionResult Index()
        {
            return View(_portfolioUnitOfWork.Entity.GetAll());
        }

        // GET: PortfolioItems/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _portfolioUnitOfWork.Entity.GetByID(id);
             
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }

        // GET: PortfolioItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PortfolioItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.File != null)
                {
                    var upload = Path.Combine(_hosting.WebRootPath, @"img/portfolio");
                    var fullPath = Path.Combine(upload, model.File.FileName);
                    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                var portfolioItem = new PortfolioItem
                {
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    ProjectName = model.ProjectName
                };
                _portfolioUnitOfWork.Entity.Insert(portfolioItem);
                _portfolioUnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PortfolioItems/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _portfolioUnitOfWork.Entity.GetByID(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }
            var portfolioItemModel = new PortfolioItem
            {
                Description = portfolioItem.Description,
                Id = portfolioItem.Id,
                ImageUrl = portfolioItem.ImageUrl,
                ProjectName = portfolioItem.ProjectName
            };
            return View(portfolioItemModel);
        }

        // POST: PortfolioItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, PortfolioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var portfolioItem = new PortfolioItem
                    {
                        Id = model.Id,
                        Description = model.Description,
                        ImageUrl = model.ImageUrl,
                        ProjectName = model.ProjectName
                    };
                    _portfolioUnitOfWork.Entity.Update(portfolioItem);
                    _portfolioUnitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioItemExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PortfolioItems/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem =  _portfolioUnitOfWork.Entity.GetByID(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }

        // POST: PortfolioItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _portfolioUnitOfWork.Entity.Delete(id);
            _portfolioUnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool PortfolioItemExists(Guid id)
        {
            return _portfolioUnitOfWork.Entity.GetAll().Any(e => e.Id == id);
        }
    }
}
