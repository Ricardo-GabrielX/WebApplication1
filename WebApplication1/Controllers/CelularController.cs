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

        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var celulares = Session["ListaCelular"] as List<Celular>;
            var celular = celulares?.FirstOrDefault(a => a.Id == id);

            if (celular == null)
                return Json(new { sucesso = false, mensagem = "Celular não encontrado" });

            celulares.Remove(celular);
            Session["ListaCelular"] = celulares;
            return Json(new { sucesso = true });
        }

        public ActionResult Create()
        {
            return View(new Celular());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Celular celular)
        {
            if (!ModelState.IsValid)
            {
                return View(celular);
            }

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

        public ActionResult GerarExcel()
        {
            var lista = Session["ListaCelular"] as List<Celular>;

            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            ExcelPackage.License.SetNonCommercialOrganization("ETEC Fernando Prestes extensão Fatec");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Celulares");

                // Cabeçalho
                planilha.Cells[1, 1].Value = "Numero de telefone";
                planilha.Cells[1, 2].Value = "Marca";
                planilha.Cells[1, 3].Value = "Data de Fabricação";

                using (var faixaCabecalho = planilha.Cells[1, 1, 1, 3])
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
                    var celular = lista[i];
                    int linha = i + 2;

                    planilha.Cells[linha, 1].Value = celular.NumeroFormatado;
                    planilha.Cells[linha, 2].Value = celular.Marca;
                    planilha.Cells[linha, 3].Value = celular.DataCompra.ToString("dd/MM/yyyy");
                }

                planilha.Cells.AutoFitColumns();

                // Bordas em todas as células com dados
                var faixaDados = planilha.Cells[1, 1, lista.Count + 1, 3];
                faixaDados.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                return File(
                    pacote.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Celulares.xlsx"
                );
            }
        }
    }
}