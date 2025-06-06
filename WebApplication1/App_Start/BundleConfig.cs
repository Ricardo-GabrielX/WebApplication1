﻿using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender com ela. Após isso, quando você estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Aluno").Include(
                "~/Content/styles/Aluno/Listar.css"
             ));
            
            bundles.Add(new StyleBundle("~/Content/Carro").Include(
                "~/Content/styles/Carro/Listar.css"
             ));

            bundles.Add(new StyleBundle("~/Content/Celular").Include(
                "~/Content/styles/Celular/Listar.css"
             ));

            bundles.Add(new StyleBundle("~/Content/Evento").Include(
                "~/Content/styles/Evento/Listar.css"
             ));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                "~/Content/styles/Create.css",
                "~/Content/styles/Exibir.css",
                "~/Content/styles/Editar.css",
                "~/Content/styles/buttons.css"
                ));
        }
    }
}
