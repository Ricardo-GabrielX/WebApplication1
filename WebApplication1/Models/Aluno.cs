using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebApplication1.Models
{
    public class Aluno
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O RA é obrigatório. ")]
        public string RA { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                if (((List<Aluno>)session["ListaAluno"]).Count > 0)
                {
                    return;
                }
            }

            var lista = new List<Aluno>();
            lista.Add(new Aluno { Id = 1, Nome = "Lucas Martins", RA = "78945612301", Data = new DateTime(2025, 03, 01) });
            lista.Add(new Aluno { Id = 2, Nome = "Mariana Souza", RA = "12378945602", Data = new DateTime(2025, 03, 02) });
            lista.Add(new Aluno { Id = 3, Nome = "Carlos Eduardo", RA = "45612378903", Data = new DateTime(2025, 03, 03) });
            lista.Add(new Aluno { Id = 4, Nome = "Fernanda Lima", RA = "98765432104", Data = new DateTime(2025, 03, 04) });
            lista.Add(new Aluno { Id = 5, Nome = "João Pedro", RA = "32198765405", Data = new DateTime(2025, 03, 05) });
            lista.Add(new Aluno { Id = 6, Nome = "Ana Clara", RA = "65432198706", Data = new DateTime(2025, 03, 06) });
            lista.Add(new Aluno { Id = 7, Nome = "Rafael Silva", RA = "25836914707", Data = new DateTime(2025, 03, 07) });
            lista.Add(new Aluno { Id = 8, Nome = "Beatriz Costa", RA = "14725836908", Data = new DateTime(2025, 03, 08) });
            lista.Add(new Aluno { Id = 9, Nome = "Henrique Oliveira", RA = "36914725809", Data = new DateTime(2025, 03, 09) });
            lista.Add(new Aluno { Id = 10, Nome = "Camila Ribeiro", RA = "85274196310", Data = new DateTime(2025, 03, 10) });

            lista.Add(new Aluno { Id = 11, Nome = "Mateus Rocha", RA = "74185296311", Data = new DateTime(2025, 03, 11) });
            lista.Add(new Aluno { Id = 12, Nome = "Juliana Freitas", RA = "96374185212", Data = new DateTime(2025, 03, 12) });
            lista.Add(new Aluno { Id = 13, Nome = "Thiago Fernandes", RA = "25896314713", Data = new DateTime(2025, 03, 13) });
            lista.Add(new Aluno { Id = 14, Nome = "Patrícia Mendes", RA = "14785236914", Data = new DateTime(2025, 03, 14) });
            lista.Add(new Aluno { Id = 15, Nome = "Diego Barros", RA = "36974125815", Data = new DateTime(2025, 03, 15) });
            lista.Add(new Aluno { Id = 16, Nome = "Larissa Teixeira", RA = "85214796316", Data = new DateTime(2025, 03, 16) });
            lista.Add(new Aluno { Id = 17, Nome = "André Almeida", RA = "74196385217", Data = new DateTime(2025, 03, 17) });
            lista.Add(new Aluno { Id = 18, Nome = "Natália Castro", RA = "96325874118", Data = new DateTime(2025, 03, 18) });
            lista.Add(new Aluno { Id = 19, Nome = "Vinícius Gomes", RA = "25814796319", Data = new DateTime(2025, 03, 19) });
            lista.Add(new Aluno { Id = 20, Nome = "Gabriela Dias", RA = "14736985220", Data = new DateTime(2025, 03, 20) });

            lista.Add(new Aluno { Id = 21, Nome = "Fábio Nogueira", RA = "36985274121", Data = new DateTime(2025, 03, 21) });
            lista.Add(new Aluno { Id = 22, Nome = "Débora Batista", RA = "85274196322", Data = new DateTime(2025, 03, 22) });
            lista.Add(new Aluno { Id = 23, Nome = "Leonardo Pinto", RA = "74196385223", Data = new DateTime(2025, 03, 23) });
            lista.Add(new Aluno { Id = 24, Nome = "Isabela Martins", RA = "96325874124", Data = new DateTime(2025, 03, 24) });
            lista.Add(new Aluno { Id = 25, Nome = "Pedro Henrique", RA = "25814796325", Data = new DateTime(2025, 03, 25) });

            session.Remove("ListaAluno");
            session.Add("ListaAluno", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
		{
			if (session["ListaAluno"] != null)
			{
				(session["ListaAluno"] as List<Aluno>).Add(this);
            }
 		}

        public static Aluno Procurar(HttpSessionStateBase session, int id)
		{
            if (session["ListaAluno"] != null)
            {
                return (session["ListaAluno"] as List<Aluno>).ElementAt(id);
            }

            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            var lista = (List<Aluno>)session["ListaAluno"];
            lista.RemoveAll(a => a.Id == this.Id);
            session["ListaAluno"] = lista;
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                var aluno = Aluno.Procurar(session, id);
                aluno.Nome = this.Nome;
                aluno.RA = this.RA;
                aluno.Data = this.Data;
            }
        }
    }
}