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
            ViewBag.Id = id;
            var carro = (Session["ListaCarro"] as List<Carro>).ElementAt(id);
            return View(carro);
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
            if (!ModelState.IsValid)
            {
                return View(carro);
            }

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

        public ActionResult GerarExcel()
        {
            var lista = Session["ListaCarro"] as List<Carro>;

            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            ExcelPackage.License.SetNonCommercialOrganization("ETEC Fernando Prestes extensão Fatec");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Carros");

                // Cabeçalho
                planilha.Cells[1, 1].Value = "Placa";
                planilha.Cells[1, 2].Value = "Cor";
                planilha.Cells[1, 3].Value = "Ano";
                planilha.Cells[1, 4].Value = "Data de Fabricação";

                using (var faixaCabecalho = planilha.Cells[1, 1, 1, 4]) 
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
                    var carro = lista[i];
                    int linha = i + 2;

                    planilha.Cells[linha, 1].Value = carro.Placa;
                    planilha.Cells[linha, 2].Value = carro.Cor;
                    planilha.Cells[linha, 3].Value = carro.Ano;
                    planilha.Cells[linha, 4].Value = carro.DataFabricacao.ToString("dd/MM/yyyy");
                }

                planilha.Cells.AutoFitColumns();

                // Bordas em todas as células com dados
                var faixaDados = planilha.Cells[1, 1, lista.Count + 1, 4]; 
                faixaDados.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                return File(
                    pacote.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Carros.xlsx"
                );
            }
        }


    }
}