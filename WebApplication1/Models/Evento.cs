using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Evento
    {
        public string Local { get; set; }

        public DateTime Data { get; set; }


        public static List<Evento> GerarLista()
        {

            var lista = new List<Evento>();
            lista.Add(new Evento { Local= "Jd blabla", Data = DateTime.Now });
            lista.Add(new Evento { Local= "Jd blabla", Data = DateTime.Now });
            lista.Add(new Evento { Local= "Jd blabla", Data = DateTime.Now });
          

            return lista;
        }
    }
}