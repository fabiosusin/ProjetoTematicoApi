using DAO.DBConnection;
using DTO.Intra.Menu.Enum;
using DTO.Intra.Menu.Output;
using System.Collections.Generic;

namespace Business.API.Intra.Menu
{
    public class BlIntraMenu
    {
        public BlIntraMenu(XDataDatabaseSettings _) { }

        public static List<MenuOutput> GetIntraMenu(MenuSystemTypeEnum type)
        {
            var menus = new List<MenuOutput>();
            var registers = new MenuOutput("Cadastros", new("fas fa-edit", IconTypeEnum.FontAwesome), new List<MenuOutput>
            {
                new("Usuários", new("fas fa-user-cog", IconTypeEnum.FontAwesome), "users"),
                new("Pessoas", new("fas fa-users", IconTypeEnum.FontAwesome), "person"),
                new("Frequência", new("fas fa-clock", IconTypeEnum.FontAwesome), "frequency")
            });

            if (type == MenuSystemTypeEnum.Ciap)
                registers.Children.AddRange(new List<MenuOutput>
                {
                    new("Parceiros", new("fa-solid fa-handshake", IconTypeEnum.FontAwesome), "companies"),
                    new("Situação", new("far fa-chart-bar", IconTypeEnum.FontAwesome), "situation"),
                    new("Entrevista", new("fa-solid fa-clipboard-question", IconTypeEnum.FontAwesome), "interview")
                });

            menus.Add(new("Dashboard", new("dashboard", IconTypeEnum.Material), "home"));
            menus.Add(registers);

            return menus;
        }
    }
}
