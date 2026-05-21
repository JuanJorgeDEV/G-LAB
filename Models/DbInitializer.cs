namespace ProjetoOS.Models;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Usuarios.Any())
        {
            db.Usuarios.AddRange(
                new Usuario { Nome = "Joao Colaborador", Email = "joao@projeto.local", Senha = "123", Perfil = "Colaborador" },
                new Usuario { Nome = "Maria Colaboradora", Email = "maria@projeto.local", Senha = "123", Perfil = "Colaborador" },
                new Usuario { Nome = "OPP Administrador", Email = "admin@projeto.local", Senha = "123", Perfil = "Administrador" }
            );
        }

        if (!db.Equipamentos.Any())
        {
            db.Equipamentos.AddRange(
                new Equipamento { PossuiNI = true, NI = "12345", Nome = "Projetor Epson", Descricao = "Projetor multimidia", Localizacao = "Sala 8", Vinculo = "Aprendizagem" },
                new Equipamento { PossuiNI = true, NI = "56789", Nome = "Impressora 3D", Descricao = "Impressora de polimero", Localizacao = "Laboratorio", Vinculo = "FIC" },
                new Equipamento { PossuiNI = false, Nome = "Cabo HDMI", Descricao = "Cabo de video", Localizacao = "Sala 3" }
            );
        }

        db.SaveChanges();
    }
}
