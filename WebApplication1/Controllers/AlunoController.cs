using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            Aluno.GerarLista(Session);

            return View(Session["ListaAluno"] as List<Aluno>);
        }
        
        public ActionResult Exibir(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaAluno"] as List<Aluno>).ElementAt(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id, Aluno aluno)
        {
            Aluno.Procurar(Session, id)?.Excluir(Session);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Aluno());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            aluno.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Editar(int id)
        {
            return View(Aluno.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Aluno aluno)
        {
            aluno.Editar(Session, id);

            return RedirectToAction("Listar");
        }
    }
}