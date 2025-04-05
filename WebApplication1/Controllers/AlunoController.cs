using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI.WebControls;



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
    }
}