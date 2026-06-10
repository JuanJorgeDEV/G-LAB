using Microsoft.EntityFrameworkCore;

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
            db.SaveChanges();
        }

        if (!db.Equipamentos.Any())
        {
            SeedSala(db, "Sala 610", 1248381, 2000001, 3000001, 4000001, 5000001);
            SeedSala(db, "Sala 615", 1248501, 2000101, 3000101, 4000101, 5000101);
            SeedSala(db, "Sala 603", 1248601, 2000201, 3000201, 4000201, 5000201);
            SeedSala(db, "Sala 604", 1248701, 2000301, 3000301, 4000301, 5000301);
            SeedSala(db, "Sala 601", 1248801, 2000401, 3000401, 4000401, 5000401);
            SeedSala(db, "Sala 602", 1248901, 2000501, 3000501, 4000501, 5000501);
            SeedSala(db, "Sala 605", 1249001, 2000601, 3000601, 4000601, 5000601);
            SeedSala(db, "Sala 612", 1249101, 2000701, 3000701, 4000701, 5000701);
            db.SaveChanges();
        }

        if (!db.OrdensServico.Any())
        {
            var joao = db.Usuarios.FirstOrDefault(u => u.Email == "joao@projeto.local");
            var maria = db.Usuarios.FirstOrDefault(u => u.Email == "maria@projeto.local");
            var admin = db.Usuarios.FirstOrDefault(u => u.Email == "admin@projeto.local");

            if (joao != null && maria != null && admin != null)
            {
                // Helper to create and return OS
                OrdemServico AddOS(string ni, Usuario solicitante, Usuario? responsavel, string problema, string tipo, string status, int daysAgo, int? closeHoursAfter = null)
                {
                    var equip = db.Equipamentos.FirstOrDefault(e => e.NI == ni);
                    if (equip == null) throw new Exception($"NI {ni} not found during seed");
                    
                    var os = new OrdemServico
                    {
                        EquipamentoId = equip.Id,
                        SolicitanteId = solicitante.Id,
                        ResponsavelId = responsavel?.Id,
                        DescricaoProblema = problema,
                        TipoProblema = tipo,
                        Status = status,
                        DataAbertura = DateTime.UtcNow.AddDays(-daysAgo),
                        DataConclusao = closeHoursAfter.HasValue ? DateTime.UtcNow.AddDays(-daysAgo).AddHours(closeHoursAfter.Value) : null
                    };
                    db.OrdensServico.Add(os);
                    db.SaveChanges(); // to get Id

                    // History - registered
                    db.HistoricosOS.Add(new HistoricoOS { OrdemServicoId = os.Id, UsuarioId = solicitante.Id, Acao = "OS registrada no sistema", DataHora = os.DataAbertura });
                    
                    if (responsavel != null)
                    {
                        db.HistoricosOS.Add(new HistoricoOS { OrdemServicoId = os.Id, UsuarioId = admin.Id, Acao = "Responsavel designado", DataHora = os.DataAbertura.AddMinutes(5) });
                        
                        if (status == "Em Execucao")
                        {
                            db.RegistrosTempo.Add(new RegistroTempo { OrdemServicoId = os.Id, ResponsavelId = responsavel.Id, Inicio = os.DataAbertura.AddMinutes(10), Fim = null });
                        }
                        else if (status == "Aguardando")
                        {
                            db.HistoricosOS.Add(new HistoricoOS { OrdemServicoId = os.Id, UsuarioId = responsavel.Id, Acao = "Trabalho pausado", DataHora = os.DataAbertura.AddHours(1), Observacao = "Aguardando peca/liberacao" });
                            db.RegistrosTempo.Add(new RegistroTempo { OrdemServicoId = os.Id, ResponsavelId = responsavel.Id, Inicio = os.DataAbertura.AddMinutes(10), Fim = os.DataAbertura.AddHours(1) });
                        }
                        else if (status == "Concluida")
                        {
                            db.HistoricosOS.Add(new HistoricoOS { OrdemServicoId = os.Id, UsuarioId = responsavel.Id, Acao = "OS concluida", DataHora = os.DataConclusao!.Value });
                            db.RegistrosTempo.Add(new RegistroTempo { OrdemServicoId = os.Id, ResponsavelId = responsavel.Id, Inicio = os.DataAbertura.AddMinutes(10), Fim = os.DataConclusao });
                        }
                    }

                    return os;
                }

                // Seed 15 OSs matching seed-dados-demo.sql exactly:
                AddOS("1248381", joao, null, "Computador não liga, led do power piscando em vermelho.", "Não liga", "Pendente", 1);
                AddOS("5000201", maria, joao, "Lente suja, projeção está muito embaçada.", "Mau funcionamento", "Em Execucao", 0);
                AddOS("4000101", joao, maria, "Não está refrigerando adequadamente.", "Mau funcionamento", "Concluida", 3, 22);
                AddOS("1248382", joao, null, "Monitor piscando sem parar.", "Mau funcionamento", "Pendente", 1);
                AddOS("1248502", joao, maria, "Teclado não está funcionando algumas teclas.", "Peça quebrada", "Em Execucao", 0);
                AddOS("5000201", maria, joao, "Sem sinal de entrada HDMI.", "Cabo/conexão", "Aguardando", 0);
                AddOS("4000301", joao, maria, "Barulho muito alto vindo da evaporadora.", "Mau funcionamento", "Concluida", 4, 20);
                AddOS("1248801", maria, null, "Mouse não responde.", "Mau funcionamento", "Pendente", 0);
                AddOS("1248901", maria, joao, "Computador lento e travando na inicialização.", "Mau funcionamento", "Em Execucao", 0);
                AddOS("5000601", joao, joao, "Suporte do projetor está solto.", "Peça quebrada", "Concluida", 2, 22);
                AddOS("4000701", joao, maria, "Controle remoto quebrado.", "Peça quebrada", "Aguardando", 1);
                AddOS("1248385", maria, joao, "Não conecta à rede sem fio.", "Cabo/conexão", "Concluida", 5, 23);
                AddOS("1248505", maria, null, "Aparecendo tela azul da morte.", "Não liga", "Pendente", 0);
                AddOS("1248605", joao, maria, "Sem áudio nas caixas de som.", "Cabo/conexão", "Em Execucao", 0);
                AddOS("1248705", maria, joao, "Fonte de alimentação queimada após oscilação de energia.", "Não liga", "Concluida", 6, 20);

                db.SaveChanges();
            }
        }
    }

    private static void SeedSala(AppDbContext db, string sala, int compStart, int tecladoStart, int monitorStart, int arStart, int projStart)
    {
        // 40 Computadores
        for (int i = 0; i < 40; i++)
        {
            db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = (compStart + i).ToString(), Nome = "Computador", Descricao = $"Computador da {sala} - Desktop", Localizacao = sala, Vinculo = "Aprendizagem" });
        }
        // 40 Teclados
        for (int i = 0; i < 40; i++)
        {
            db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = (tecladoStart + i).ToString(), Nome = "Teclado", Descricao = "Teclado padrão ABNT2", Localizacao = sala, Vinculo = "Aprendizagem" });
        }
        // 40 Monitores
        for (int i = 0; i < 40; i++)
        {
            db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = (monitorStart + i).ToString(), Nome = "Monitor", Descricao = "Monitor LCD 19 polegadas", Localizacao = sala, Vinculo = "Aprendizagem" });
        }
        // 40 Mouses (sem NI)
        for (int i = 0; i < 40; i++)
        {
            db.Equipamentos.Add(new Equipamento { PossuiNI = false, Nome = "Mouse", Descricao = "Mouse óptico USB", Localizacao = sala, Vinculo = "Aprendizagem" });
        }
        // 2 Ar-condicionados
        db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = arStart.ToString(), Nome = "Ar-condicionado", Descricao = "Ar-condicionado Split", Localizacao = sala, Vinculo = "Aprendizagem" });
        db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = (arStart + 1).ToString(), Nome = "Ar-condicionado", Descricao = "Ar-condicionado Split", Localizacao = sala, Vinculo = "Aprendizagem" });
        // 1 Projetor
        db.Equipamentos.Add(new Equipamento { PossuiNI = true, NI = projStart.ToString(), Nome = "Projetor", Descricao = "Projetor multimídia Epson", Localizacao = sala, Vinculo = "Aprendizagem" });
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
