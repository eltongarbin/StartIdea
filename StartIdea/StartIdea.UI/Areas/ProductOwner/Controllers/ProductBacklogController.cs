﻿using PagedList;
using StartIdea.DataAccess;
using StartIdea.Model.ScrumArtefatos;
using StartIdea.UI.Areas.ProductOwner.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StartIdea.UI.Areas.ProductOwner.Controllers
{
    public class ProductBacklogController : Controller
    {
        private StartIdeaDBContext dbContext = new StartIdeaDBContext();

        public ActionResult Index(string contextoBusca, string filtroAtual, int? pagina)
        {
            var productBacklogVM = new ProductBacklogVM();

            if (contextoBusca != null)
                pagina = 1;
            else
                contextoBusca = filtroAtual;

            ViewBag.Pagina      = pagina;
            ViewBag.FiltroAtual = contextoBusca;

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            if (!string.IsNullOrEmpty(contextoBusca))
            {
                productBacklogVM.productBacklogs = dbContext.ProductBacklogs
                                                   .Where(productBacklog => productBacklog.UserStory.ToUpper().Contains(contextoBusca.ToUpper()))
                                                   .ToList()
                                                   .OrderBy(backlog => backlog.Prioridade)
                                                   .ToPagedList(pageNumber, pageSize);
            }
            else
            {
                productBacklogVM.productBacklogs = dbContext.ProductBacklogs
                                                   .ToList()
                                                   .OrderBy(backlog => backlog.Prioridade)
                                                   .ToPagedList(pageNumber, pageSize);
            }

            return View(productBacklogVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserStory,Prioridade")] ProductBacklog productBacklog, string filtroAtual, int? paginaAtual)
        {
            if (ModelState.IsValid)
            {
                productBacklog.ProductOwnerId = 1; // Remover

                dbContext.ProductBacklogs.Add(productBacklog);
                dbContext.SaveChanges();

                return RedirectToAction("Index", "ProductBacklog", new
                {
                    filtroAtual = filtroAtual,
                    pagina = paginaAtual
                });
            }

            return RedirectToAction("Index", "ProductBacklog", new {
                filtroAtual = filtroAtual,
                pagina = paginaAtual
            });
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            return View("Index", productBacklog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserStory,StoryPoint,Prioridade,DataInclusao,ProductOwnerId")] ProductBacklog productBacklog)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(productBacklog).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(productBacklog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductBacklog productBacklog = dbContext.ProductBacklogs.Find(id);
            if (productBacklog == null)
                return HttpNotFound();

            dbContext.ProductBacklogs.Remove(productBacklog);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}