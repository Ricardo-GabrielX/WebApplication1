using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Carro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatório.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "A cor é obrigatório.")]
        public string Cor { get; set; }

        //[Required(ErrorMessage = "O ano do carro é obrigatório.")]
        //[Range(1900, 9999, ErrorMessage = "O ano deve ser um número válido entre 1900 e 9999.")]
        //public string Ano { get; set; }

        [Required(ErrorMessage = "A data de fabricação é obrigatória.")]
        [Display(Name = "Data de fabricação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFabricacao { get; set; }
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
            lista.Add(new Carro { Placa = "ABC1D23", DataFabricacao = DateTime.Now, Cor = "Vermelho" });
            lista.Add(new Carro { Placa = "DEF4G56", DataFabricacao = DateTime.Now, Cor = "Azul" });
            lista.Add(new Carro { Placa = "GHI7J89", DataFabricacao = DateTime.Now, Cor = "Prata" });
            lista.Add(new Carro { Placa = "JKL0M12", DataFabricacao = DateTime.Now, Cor = "Verde" });
            lista.Add(new Carro { Placa = "MNO3P45", DataFabricacao = DateTime.Now, Cor = "Amarelo" });
            lista.Add(new Carro { Placa = "QRS6T78", DataFabricacao = DateTime.Now, Cor = "Preto" });
            lista.Add(new Carro { Placa = "TUV9W01", DataFabricacao = DateTime.Now, Cor = "Branco" });
            lista.Add(new Carro { Placa = "XYZ2A34", DataFabricacao = DateTime.Now, Cor = "Laranja" });
            lista.Add(new Carro { Placa = "BCD5E67", DataFabricacao = DateTime.Now, Cor = "Roxo" });
            lista.Add(new Carro { Placa = "EFG8H90", DataFabricacao = DateTime.Now, Cor = "Cinza" });
            lista.Add(new Carro { Placa = "HIJ1K23", DataFabricacao = DateTime.Now, Cor = "Marrom" });
            lista.Add(new Carro { Placa = "KLM4N56", DataFabricacao = DateTime.Now, Cor = "Bege" });
            lista.Add(new Carro { Placa = "NOP7Q89", DataFabricacao = DateTime.Now, Cor = "Vinho" });
            lista.Add(new Carro { Placa = "RST0U12", DataFabricacao = DateTime.Now, Cor = "Rosa" });
            lista.Add(new Carro { Placa = "UVW3X45", DataFabricacao = DateTime.Now, Cor = "Dourado" });
            lista.Add(new Carro { Placa = "WXY6Z78", DataFabricacao = DateTime.Now, Cor = "Prata Fosco" });
            lista.Add(new Carro { Placa = "YZA9B01", DataFabricacao = DateTime.Now, Cor = "Azul Marinho" });
            lista.Add(new Carro { Placa = "CAB2D34", DataFabricacao = DateTime.Now, Cor = "Verde Musgo" });
            lista.Add(new Carro { Placa = "BCE5F67", DataFabricacao = DateTime.Now, Cor = "Vermelho Ferrari" });
            lista.Add(new Carro { Placa = "CDF8G90", DataFabricacao = DateTime.Now, Cor = "Amarelo Canário" });
            lista.Add(new Carro { Placa = "DFG1H23", DataFabricacao = DateTime.Now, Cor = "Preto Onix" });
            lista.Add(new Carro { Placa = "FGH4I56", DataFabricacao = DateTime.Now, Cor = "Branco Pérola" });
            lista.Add(new Carro { Placa = "GHI7J89", DataFabricacao = DateTime.Now, Cor = "Laranja Cítrico" });
            lista.Add(new Carro { Placa = "ARG901", DataFabricacao = DateTime.Now, Cor = "Preto Onix" });
            lista.Add(new Carro { Placa = "BRA234", DataFabricacao = DateTime.Now, Cor = "Branco Pérola" });
            lista.Add(new Carro { Placa = "EUA234", DataFabricacao = DateTime.Now, Cor = "Laranja Cítrico" });

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
            var lista = (List<Carro>)session["ListaCarro"];
            lista.RemoveAll(a => a.Id == this.Id);
            session["ListaCarro"] = lista;
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                var carro = Carro.Procurar(session, id);
                carro.Placa = this.Placa;
                carro.Cor = this.Cor;
                carro.DataFabricacao = this.DataFabricacao;
            }
        }
    }
}