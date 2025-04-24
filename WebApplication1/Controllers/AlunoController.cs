using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using OfficeOpenXml;
using System.ComponentModel;
using ClosedXML.Excel;

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
            ViewBag.Id = id;
            var aluno = (Session["ListaAluno"] as List<Aluno>).ElementAt(id);
            return View(aluno);
        }

        [HttpPost]
        public ActionResult DeleteAjax(int id) 
        {
            var alunos = Session["ListaAluno"] as List<Aluno>;
            var aluno = alunos?.FirstOrDefault(a => a.Id == id);

            if (aluno == null)
                return Json(new { sucesso = false, mensagem = "Aluno não encontrado" });

            alunos.Remove(aluno);
            Session["ListaAluno"] = alunos;
            return Json(new { sucesso = true });
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

        public ActionResult GerarPdf()
        {
            List<Aluno> listaAlunos = Session["ListaAluno"] as List<Aluno>;

            if (listaAlunos == null || !listaAlunos.Any())
            {
                return Content("Nenhum aluno encontrado para gerar o PDF");
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
                    document.Add(new Paragraph("Lista de Alunos", titleFont));
                    document.Add(Chunk.NEWLINE);

                    // Tabela
                    PdfPTable table = new PdfPTable(3);
                    table.WidthPercentage = 100;

                    // Cabeçalhos
                    table.AddCell(new Phrase("Nome", headerFont));
                    table.AddCell(new Phrase("RA", headerFont));
                    table.AddCell(new Phrase("Data", headerFont));

                    // Dados
                    foreach (var aluno in listaAlunos)
                    {
                        table.AddCell(new Phrase(aluno.Nome ?? "", normalFont));
                        table.AddCell(new Phrase(aluno.RA ?? "", normalFont));

                        string dataFormatada = aluno.Data.ToString("dd/MM/yyyy");
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

                return File(memoryStream.ToArray(), "application/pdf", "ListaAlunos.pdf");
            }
        }

        public ActionResult GerarExcel()
        {
            var lista = Session["ListaAluno"] as List<Aluno>;

            if (lista == null || !lista.Any())
                return RedirectToAction("Listar");

            ExcelPackage.License.SetNonCommercialOrganization("ETEC Fernando Prestes extensão Fatec");

            using (var pacote = new ExcelPackage())
            {
                var planilha = pacote.Workbook.Worksheets.Add("Alunos");

                // Cabeçalho
                planilha.Cells[1, 1].Value = "Nome";
                planilha.Cells[1, 2].Value = "RA";
                planilha.Cells[1, 3].Value = "Data de Nascimento";

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
                    var aluno = lista[i];
                    planilha.Cells[i + 2, 1].Value = aluno.Nome;
                    planilha.Cells[i + 2, 2].Value = aluno.RA;
                    planilha.Cells[i + 2, 3].Value = aluno.Data.ToString("dd/MM/yyyy");
                }

                planilha.Cells.AutoFitColumns();

                // Bordas nas células preenchidas
                var faixaDados = planilha.Cells[1, 1, lista.Count + 1, 3];
                faixaDados.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                faixaDados.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                return File(
                    pacote.GetAsByteArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Alunos.xlsx"
                );
            }
        }

        //private void GerarExcel(string tabName, string fileName, ICollection<Aluno> lista)
        //{
        //    string filePathName = Path.Combine(Server.MapPath("~/App_Data/"), fileName);

        //    if (System.IO.File.Exists(filePathName))
        //        System.IO.File.Delete(filePathName);

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var planilha = workbook.Worksheets.Add(tabName);

        //        // Cabeçalho
        //        planilha.Cell("A1").Value = "Nome";
        //        planilha.Cell("B1").Value = "RA";
        //        planilha.Cell("C1").Value = "Data de Nascimento";

        //        int linha = 2;

        //        foreach (var aluno in lista)
        //        {
        //            planilha.Cell("A" + linha).Value = aluno.Nome;
        //            planilha.Cell("B" + linha).Value = aluno.RA;
        //            planilha.Cell("C" + linha).Value = aluno.Data.ToString("dd/MM/yyyy");
        //            linha++;
        //        }

        //        planilha.Columns().AdjustToContents(); // Ajusta a largura das colunas

        //        workbook.SaveAs(filePathName);
        //    }
        //}

    }

}
    
