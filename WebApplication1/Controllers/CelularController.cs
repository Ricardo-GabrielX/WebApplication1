using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CelularController : Controller
    {
        // GET: Celular
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Celular.GerarLista(Session);

            return View(Session["ListaCelular"] as List<Celular>);
        }

        public ActionResult Exibir(int id)
        {
            ViewBag.Id = id;
            var celular = (Session["ListaCelular"] as List<Celular>).ElementAt(id);
            return View(celular);
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaCelular"] as List<Celular>).ElementAt(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id, Celular celular)
        {
            Celular.Procurar(Session, id)?.Excluir(Session);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Celular());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            celular.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Editar(int id)
        {
            return View(Celular.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Celular celular)
        {
            celular.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult GerarPdf()
        {
            List<Celular> ListaCelulares = Session["ListaCelular"] as List<Celular>;

            if (ListaCelulares == null || !ListaCelulares.Any())
            {
                return Content("Nenhum celular encontrado para gerar o PDF");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 50, 50, 25, 25);

                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Configuração de fontes
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Título
                    document.Add(new Paragraph("Lista de Celular", titleFont));
                    document.Add(Chunk.NEWLINE);

                    // Tabela
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;

                    // Cabeçalhos
                    table.AddCell(new Phrase("Numero", headerFont));
                    table.AddCell(new Phrase("Marca", headerFont));
                    table.AddCell(new Phrase("Novo", headerFont));
                    table.AddCell(new Phrase("Data de Compra", headerFont));

                    // Dados
                    foreach (var celular in ListaCelulares)
                    {
                        table.AddCell(new Phrase(celular.Numero.ToString(), normalFont));
                        table.AddCell(new Phrase(celular.Marca ?? "", normalFont));
                        table.AddCell(new Phrase(celular.Novo ? "Sim" : "Não", normalFont));

                        string dataFormatada = celular.DataCompra.ToString("dd/MM/yyyy");
                        table.AddCell(new Phrase(dataFormatada, normalFont));
                    }

                    document.Add(table);
                }
                finally
                {
                    if (document.IsOpen())
                    {
                        document.Close();
                    }
                }

                return File(memoryStream.ToArray(), "application/pdf", "ListaCelular.pdf");
            }
        }
    }
}