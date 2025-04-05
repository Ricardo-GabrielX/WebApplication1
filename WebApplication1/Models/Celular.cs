using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Celular
    {
        public int Numero { get; set; }

        public string Marca { get; set; }

        public bool Novo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCompra { get; set; }
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                if (((List<Celular>)session["ListaCelular"]).Count > 0)
                {
                    return;
                }

            }
            var lista = new List<Celular>();
            lista.Add(new Celular { Numero = 1599231231, Marca = "Motorola", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Numero = 1599211111, Marca = "Samsung", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Numero = 1599321321, Marca = "Iphone", Novo = false , DataCompra = DateTime.Now });

            session.Remove("ListaCelular");
            session.Add("ListaCelular", lista);
        }

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                (session["ListaCelular"] as List<Celular>).Add(this);
            }
        }

        public static Celular Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                return (session["ListaCelular"] as List<Celular>).ElementAt(id);
            }

            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                (session["ListaCelular"] as List<Celular>).Remove(this);
            }
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                var celular = Celular.Procurar(session, id);
                celular.Numero = this.Numero;
                celular.Marca = this.Marca;
                celular.Novo = this.Novo;
                celular.DataCompra = this.DataCompra;
            }
        }
    }
}