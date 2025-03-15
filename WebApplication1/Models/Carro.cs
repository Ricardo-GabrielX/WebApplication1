using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Carro
    {
        public string Placa { get; set; }

        public string Ano { get; set; }

        public string Cor { get; set; }


        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                if (((List<Carro>)session["ListaCarro"]).Count > 0)
                {
                    return;
                }

            }
            var lista = new List<Carro>();
            lista.Add(new Carro { Placa = "BRA123", Ano = "1234" , Cor="Vermelho" });
            lista.Add(new Carro { Placa = "EUA123", Ano = "1234" , Cor="Azul" });
            lista.Add(new Carro { Placa = "ARG123", Ano = "1234" , Cor="Prata" });


            session.Remove("ListaCarro");
            session.Add("ListaCarro", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Add(this);
            }
        }

        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                return (session["ListaCarro"] as List<Carro>).ElementAt(id);
            }

            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Remove(this);
            }
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                var carro = Carro.Procurar(session, id);
                carro.Placa = this.Placa;
                carro.Ano = this.Ano;
                carro.Cor = this.Cor;
            }
        }
    }
}