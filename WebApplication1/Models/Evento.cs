using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O local do evento é obrigatório.")]
        public string Local { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        [Display(Name = "Data do evento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                if (((List<Evento>)session["ListaEvento"]).Count > 0)
                {
                    return;
                }

            }
            var lista = new List<Evento>();
            lista.Add(new Evento { Id = 1, Local = "Jd Araraquara", Data = DateTime.Now });
            lista.Add(new Evento { Id = 2, Local = "Jd Dos Anjos", Data = DateTime.Now });
            lista.Add(new Evento { Id = 3, Local = "Jd Lá Tem", Data = DateTime.Now });
            lista.Add(new Evento { Id = 4, Local = "Centro", Data = DateTime.Now });
            lista.Add(new Evento { Id = 5, Local = "Vila Hortência", Data = DateTime.Now });
            lista.Add(new Evento { Id = 6, Local = "Mangal", Data = DateTime.Now });
            lista.Add(new Evento { Id = 7, Local = "Campolim", Data = DateTime.Now });
            lista.Add(new Evento { Id = 8, Local = "Wanel Ville", Data = DateTime.Now });
            lista.Add(new Evento { Id = 9, Local = "Éden", Data = DateTime.Now });
            lista.Add(new Evento { Id = 10, Local = "Santa Rosália", Data = DateTime.Now });
            lista.Add(new Evento { Id = 11, Local = "Vila Leão", Data = DateTime.Now });
            lista.Add(new Evento { Id = 12, Local = "Jd Europa", Data = DateTime.Now });
            lista.Add(new Evento { Id = 13, Local = "Alto da Boa Vista", Data = DateTime.Now });
            lista.Add(new Evento { Id = 14, Local = "Jd Paulistano", Data = DateTime.Now });
            lista.Add(new Evento { Id = 15, Local = "Parque das Laranjeiras", Data = DateTime.Now });
            lista.Add(new Evento { Id = 16, Local = "Jd Maria Eugênia", Data = DateTime.Now });
            lista.Add(new Evento { Id = 17, Local = "Vila Progresso", Data = DateTime.Now });
            lista.Add(new Evento { Id = 18, Local = "Jd Simus", Data = DateTime.Now });
            lista.Add(new Evento { Id = 19, Local = "Jd Astro", Data = DateTime.Now });
            lista.Add(new Evento { Id = 20, Local = "Jd Zulmira", Data = DateTime.Now });
            lista.Add(new Evento { Id = 21, Local = "Jd Gutierres", Data = DateTime.Now });
            lista.Add(new Evento { Id = 22, Local = "Jd Tatiana", Data = DateTime.Now });
            lista.Add(new Evento { Id = 23, Local = "Jd Sandra", Data = DateTime.Now });
            lista.Add(new Evento { Id = 24, Local = "Jd Brasil", Data = DateTime.Now });
            lista.Add(new Evento { Id = 25, Local = "Jd Iguatemi", Data = DateTime.Now });
            lista.Add(new Evento { Id = 26, Local = "Jd Santa Bárbara", Data = DateTime.Now });

            session.Remove("ListaEvento");
            session.Add("ListaEvento", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                (session["ListaEvento"] as List<Evento>).Add(this);
            }
        }

        public static Evento Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                return (session["ListaEvento"] as List<Evento>).ElementAt(id);
            }

            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            var lista = (List<Evento>)session["ListaEvento"];
            lista.RemoveAll(a => a.Id == this.Id);
            session["ListaEvento"] = lista;
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                var evento = Evento.Procurar(session, id);
                evento.Local = this.Local;
                evento.Data = this.Data;
            }
        }
    }
}