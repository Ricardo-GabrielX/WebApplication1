using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Celular
    {
        public int Id { get; set; }

       

        [Required(ErrorMessage = "O número é obrigatório.")]
        [RegularExpression(@"^[\d\-\(\)\s]{11,15}$", ErrorMessage = "Número inválido. Digite 11 dígitos.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "A marca é obrigatório.")]
        public string Marca { get; set; }

        public bool Novo { get; set; }

        [Required(ErrorMessage = "A data de compra é obrigatória.")]
        [Display(Name = "Data de compra")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataCompra { get; set; }

        public string NumeroFormatado
        {
            get
            {
                if (string.IsNullOrEmpty(Numero))
                    return Numero;

                return Convert.ToInt64(Numero).ToString("(00) 00000-0000");
            }
        }

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
            lista.Add(new Celular { Id = 1, Numero = "15992312312", Marca = "Motorola", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 2, Numero = "15992111113", Marca = "Samsung", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 3, Numero = "15993213214", Marca = "Iphone", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 4, Numero = "15988776655", Marca = "Xiaomi", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 5, Numero = "15977665544", Marca = "LG", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 6, Numero = "15966554433", Marca = "Asus", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 7, Numero = "15955443322", Marca = "Nokia", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 8, Numero = "15944332211", Marca = "Sony", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 9, Numero = "15933221100", Marca = "Huawei", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 10, Numero = "15922110099", Marca = "Google", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 11, Numero = "15911009988", Marca = "OnePlus", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 12, Numero = "15900998877", Marca = "Realme", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 13, Numero = "15987654321", Marca = "Motorola", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 14, Numero = "15976543210", Marca = "Samsung", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 15, Numero = "15965432109", Marca = "Iphone", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 16, Numero = "15954321098", Marca = "Xiaomi", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 17, Numero = "15943210987", Marca = "LG", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 18, Numero = "15932109876", Marca = "Asus", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 19, Numero = "15921098765", Marca = "Nokia", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 20, Numero = "15910987654", Marca = "Sony", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 21, Numero = "15909876543", Marca = "Huawei", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 22, Numero = "15998765432", Marca = "Google", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 23, Numero = "15987654320", Marca = "OnePlus", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 24, Numero = "15976543209", Marca = "Realme", Novo = true, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 25, Numero = "15965432098", Marca = "Motorola", Novo = false, DataCompra = DateTime.Now });
            lista.Add(new Celular { Id = 26, Numero = "15954320987", Marca = "Samsung", Novo = true, DataCompra = DateTime.Now });

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
            var lista = (List<Celular>)session["ListaCelular"];
            lista.RemoveAll(a => a.Id == this.Id);
            session["ListaCelular"] = lista;
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