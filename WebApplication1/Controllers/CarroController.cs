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
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            Carro.GerarLista(Session);

            return View(Session["ListaCarro"] as List<Carro>);
        }

        public ActionResult Exibir(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }

        public ActionResult Delete(int id)
        {
            return View((Session["ListaCarro"] as List<Carro>).ElementAt(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Carro carro)
        {
            Carro.Procurar(Session, id)?.Excluir(Session);

            return RedirectToAction("Listar");
        }

        public ActionResult Create()
        {
            return View(new Carro());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Carro carro)
        {
            carro.Adicionar(Session);

            return RedirectToAction("Listar");
        }
        public ActionResult Editar(int id)
        {
            return View(Carro.Procurar(Session, id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Carro carro)
        {
            carro.Editar(Session, id);

            return RedirectToAction("Listar");
        }

        public ActionResult GerarPdf()
        {
            List<Carro> ListaCarros = Session["ListaCarro"] as List<Carro>;

            if (ListaCarros == null || !ListaCarros.Any())
            {
                return Content("Nenhum carro encontrado para gerar o PDF");
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
                    document.Add(new Paragraph("Lista de Carros", titleFont));
                    document.Add(Chunk.NEWLINE);

                    // Tabela
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;

                    // Cabeçalhos
                    table.AddCell(new Phrase("Placa", headerFont));
                    table.AddCell(new Phrase("Cor", headerFont));
                    table.AddCell(new Phrase("Ano", headerFont));
                    table.AddCell(new Phrase("Data de fabricação", headerFont));

                    // Dados
                    foreach (var carro in ListaCarros)
                    {
                        table.AddCell(new Phrase(carro.Placa ?? "", normalFont));
                        table.AddCell(new Phrase(carro.Cor ?? "", normalFont));
                        table.AddCell(new Phrase(carro.Ano ?? "", normalFont));

                        string dataFormatada = carro.DataFabricacao.ToString("dd/MM/yyyy");
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

                return File(memoryStream.ToArray(), "application/pdf", "ListaCarro.pdf");
            }
        }

    }
}