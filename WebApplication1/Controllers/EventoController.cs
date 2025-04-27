using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using OfficeOpenXml;

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

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var eventos = Session["ListaEvento"] as List<Evento>;
            var evento = eventos?.FirstOrDefault(a => a.Id == id);

            if (evento == null)
                return Json(new { sucesso = false, mensagem = "Aluno não encontrado" });

            eventos.Remove(evento);
            Session["ListaAluno"] = eventos;
            return Json(new { sucesso = true });
        }

        public ActionResult Create()
        {
            return View(new Evento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return View(evento);
            }

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

        public ActionResult GerarExcel()
        {
            var lista = Session["ListaEvento"] as List<Evento>;

            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            ExcelPackage.License.SetNonCommercialOrganization("ETEC Fernando Prestes extensão Fatec");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Celulares");

                // Cabeçalho
                planilha.Cells[1, 1].Value = "Local";
                planilha.Cells[1, 2].Value = "Data";

                using (var faixaCabecalho = planilha.Cells[1, 1, 1, 2])
                {
                    faixaCabecalho.Style.Font.Bold = true;
                    faixaCabecalho.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    faixaCabecalho.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    faixaCabecalho.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    faixaCabecalho.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Dados
                for (int i = 0; i < lista.Count; i++)
                {
                    var evento = lista[i];
                    int linha = i + 2;

                    planilha.Cells[linha, 1].Value = evento.Local;
                    planilha.Cells[linha, 2].Value = evento.Data.ToString("dd/MM/yyyy");
                }

                planilha.Cells.AutoFitColumns();

                // Bordas em todas as células com dados
                var faixaDados = planilha.Cells[1, 1, lista.Count + 1, 2];
                faixaDados.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                return File(
                    pacote.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Eventos.xlsx"
                );
            }
        }
    }
}