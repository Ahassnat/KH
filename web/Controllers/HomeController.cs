using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;
using Core.Intefaces;
using Microsoft.AspNetCore.Mvc;
using web.ViewModels;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<Owner> _ownerUnitOfWork;
        private readonly IUnitOfWork<PortfolioItem> _portfolioUnitOfWork;

        public HomeController(IUnitOfWork<Owner> ownerUnitOfWork,
                              IUnitOfWork<PortfolioItem> portfolioUnitOfWork)
        {
            _ownerUnitOfWork = ownerUnitOfWork;
            _portfolioUnitOfWork = portfolioUnitOfWork;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                Owner = _ownerUnitOfWork.Entity.GetAll().First(),
                PortfolioItems = _portfolioUnitOfWork.Entity.GetAll().ToList()
            };
            return View(homeViewModel);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
