using DAO.DBConnection;
using DTO.Intra.Menu.Enum;
using DTO.Intra.Menu.Output;
using System.Collections.Generic;

namespace Business.API.Intra.Menu
{
    public class BlIntraMenu
    {
        public BlIntraMenu(XDataDatabaseSettings _) { }

        public static List<MenuOutput> GetIntraMenu()
        {
            var menus = new List<MenuOutput>();
            var registers = new MenuOutput("Cadastros", new("fas fa-edit", IconTypeEnum.FontAwesome), new List<MenuOutput>
            {
                new("Usuários", new("fas fa-user-cog", IconTypeEnum.FontAwesome), "users"),
                new("Pessoas", new("fas fa-users", IconTypeEnum.FontAwesome), "person"),
                new("Parceiros", new("fa-solid fa-industry", IconTypeEnum.FontAwesome), "companies"),
                new("Situação", new("fa-solid fa-industry", IconTypeEnum.FontAwesome), "situation"),
                new("Entrevista", new("fa-solid fa-industry", IconTypeEnum.FontAwesome), "interview"),
                new("Frequência", new("fa-solid fa-industry", IconTypeEnum.FontAwesome), "frequency")
            });

            menus.Add(new("Dashboard", new("dashboard", IconTypeEnum.Material), "home"));
            menus.Add(registers);

            return menus;
        }
    }
}
