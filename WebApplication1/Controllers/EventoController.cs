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
    public class EventoController : Controller
    {
        // GET: Evento
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar()
        {
            Evento.GerarLista(Session);

            return View(Session["ListaEvento"] as List<Evento>);
        }

        public ActionResult Exibir(int id)
        {
            ViewBag.Id = id;
            var evento = (Session["ListaEvento"] as List<Evento>).ElementAt(id);
            return View(evento);
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaEvento"] as List<Evento>).ElementAt(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id, Evento evento)
        {
            Evento.Procurar(Session, id)?.Excluir(Session);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            evento.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Editar(int id)
        {
            return View(Evento.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Evento evento)
        {
            evento.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult GerarPdf()
        {
            List<Evento> ListaEventos = Session["ListaEvento"] as List<Evento>;

            if (ListaEventos == null || !ListaEventos.Any())
            {
                return Content("Nenhum evento encontrado para gerar o PDF");
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
                    document.Add(new Paragraph("Lista de Eventos", titleFont));
                    document.Add(Chunk.NEWLINE);

                    // Tabela
                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;

                    // Cabeçalhos
                    table.AddCell(new Phrase("Local", headerFont));
                    table.AddCell(new Phrase("Data", headerFont));

                    // Dados
                    foreach (var evento in ListaEventos)
                    {
                        table.AddCell(new Phrase(evento.Local ?? "", normalFont));

                        string dataFormatada = evento.Data.ToString("dd/MM/yyyy");
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

                return File(memoryStream.ToArray(), "application/pdf", "ListaEvento.pdf");
            }
        }
    }
}