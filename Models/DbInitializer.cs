namespace ProjetoOS.Models;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Usuarios.Any())
        {
            db.Usuarios.AddRange(
                CriarUsuario("Juan Administrador Geral", "geral@projeto.local", "000.000.000-00", "Administrador"),
                CriarUsuario("Joao Colaborador", "joao@projeto.local", "111.111.111-11", "Colaborador"),
                CriarUsuario("Maria Colaboradora", "maria@projeto.local", "222.222.222-22", "Colaborador"),
                CriarUsuario("OPP Administrador", "admin@projeto.local", "333.333.333-33", "Administrador")
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

    private static Usuario CriarUsuario(string nome, string email, string cpf, string perfil)
    {
        var senhaInicial = PasswordHelper.GerarSenhaInicial(nome, cpf);
        return new Usuario
        {
            Nome = nome,
            Email = email,
            CPF = cpf,
            Senha = PasswordHelper.CriarHash(senhaInicial),
            Perfil = perfil
        };
    }
}
