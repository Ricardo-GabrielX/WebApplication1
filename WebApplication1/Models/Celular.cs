using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Celular
    {
        public int Numero { get; set; }

        public string Marca { get; set; }

        public bool Novo { get; set; }


        public static List<Celular> GerarLista()
        {

            var lista = new List<Celular>();
            lista.Add(new Celular { Numero = 1599231231, Marca = "Motorola", Novo = true });
            lista.Add(new Celular { Numero = 1599211111, Marca = "Samsung", Novo = true });
            lista.Add(new Celular { Numero = 1599321321, Marca = "Iphone", Novo = false });
            


            return lista;
        }
    }
}