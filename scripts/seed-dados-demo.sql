-- =============================================================================
-- ProjetoOS — Seed de demonstração (PostgreSQL / Supabase)
-- =============================================================================
-- Como usar:
--   1. Supabase → SQL Editor → New query → cole este arquivo → Run
--   2. Ou: psql "$CONNECTION_STRING" -f scripts/seed-dados-demo.sql
--
-- ATENÇÃO: apaga TODOS os dados das tabelas da aplicação e recria do zero.
-- Senha de todos os usuários criados aqui: 123 (texto plano)
-- =============================================================================

BEGIN;

TRUNCATE TABLE
    "HistoricosOS",
    "RegistrosTempo",
    "OrdensServico",
    "Equipamentos",
    "Usuarios"
RESTART IDENTITY CASCADE;

-- -----------------------------------------------------------------------------
-- Usuários (4): Administradores e Colaboradores fixos
-- -----------------------------------------------------------------------------
INSERT INTO "Usuarios" ("Nome", "Email", "Senha", "Perfil")
VALUES
    ('Juan Administrador Geral', 'geral@projeto.local', '123', 'Administrador'),
    ('Joao Colaborador',        'joao@projeto.local',  '123', 'Colaborador'),
    ('Maria Colaboradora',      'maria@projeto.local', '123', 'Colaborador'),
    ('OPP Administrador',       'admin@projeto.local', '123', 'Administrador');

-- -----------------------------------------------------------------------------
-- Equipamentos (1304): 163 equipamentos por sala (610, 615, 603, 604, 601, 602, 605, 612)
-- -----------------------------------------------------------------------------

-- SALA 610
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248380 + g)::text, 'Computador', 'Computador da sala 610 - Desktop', 'Sala 610', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000000 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 610', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000000 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 610', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 610', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000001', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 610', 'Aprendizagem', true),
(true, '4000002', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 610', 'Aprendizagem', true),
(true, '5000001', 'Projetor', 'Projetor multimídia Epson', 'Sala 610', 'Aprendizagem', true);


-- SALA 615
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248500 + g)::text, 'Computador', 'Computador da sala 615 - Desktop', 'Sala 615', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000100 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 615', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000100 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 615', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 615', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000101', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 615', 'Aprendizagem', true),
(true, '4000102', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 615', 'Aprendizagem', true),
(true, '5000101', 'Projetor', 'Projetor multimídia Epson', 'Sala 615', 'Aprendizagem', true);


-- SALA 603
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248600 + g)::text, 'Computador', 'Computador da sala 603 - Desktop', 'Sala 603', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000200 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 603', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000200 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 603', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 603', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000201', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 603', 'Aprendizagem', true),
(true, '4000202', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 603', 'Aprendizagem', true),
(true, '5000201', 'Projetor', 'Projetor multimídia Epson', 'Sala 603', 'Aprendizagem', true);


-- SALA 604
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248700 + g)::text, 'Computador', 'Computador da sala 604 - Desktop', 'Sala 604', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000300 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 604', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000300 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 604', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 604', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000301', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 604', 'Aprendizagem', true),
(true, '4000302', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 604', 'Aprendizagem', true),
(true, '5000301', 'Projetor', 'Projetor multimídia Epson', 'Sala 604', 'Aprendizagem', true);


-- SALA 601
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248800 + g)::text, 'Computador', 'Computador da sala 601 - Desktop', 'Sala 601', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000400 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 601', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000400 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 601', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 601', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000401', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 601', 'Aprendizagem', true),
(true, '4000402', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 601', 'Aprendizagem', true),
(true, '5000401', 'Projetor', 'Projetor multimídia Epson', 'Sala 601', 'Aprendizagem', true);


-- SALA 602
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1248900 + g)::text, 'Computador', 'Computador da sala 602 - Desktop', 'Sala 602', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000500 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 602', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000500 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 602', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 602', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000501', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 602', 'Aprendizagem', true),
(true, '4000502', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 602', 'Aprendizagem', true),
(true, '5000501', 'Projetor', 'Projetor multimídia Epson', 'Sala 602', 'Aprendizagem', true);


-- SALA 605
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1249000 + g)::text, 'Computador', 'Computador da sala 605 - Desktop', 'Sala 605', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000600 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 605', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000600 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 605', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 605', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000601', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 605', 'Aprendizagem', true),
(true, '4000602', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 605', 'Aprendizagem', true),
(true, '5000601', 'Projetor', 'Projetor multimídia Epson', 'Sala 605', 'Aprendizagem', true);


-- SALA 612
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (1249100 + g)::text, 'Computador', 'Computador da sala 612 - Desktop', 'Sala 612', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (2000700 + g)::text, 'Teclado', 'Teclado padrão ABNT2', 'Sala 612', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT true, (3000700 + g)::text, 'Monitor', 'Monitor LCD 19 polegadas', 'Sala 612', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT false, NULL, 'Mouse', 'Mouse óptico USB', 'Sala 612', 'Aprendizagem', true
FROM generate_series(1, 40) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
VALUES 
(true, '4000701', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 612', 'Aprendizagem', true),
(true, '4000702', 'Ar-condicionado', 'Ar-condicionado Split', 'Sala 612', 'Aprendizagem', true),
(true, '5000701', 'Projetor', 'Projetor multimídia Epson', 'Sala 612', 'Aprendizagem', true);

-- -----------------------------------------------------------------------------
-- Ordens de serviço realistas (15)
-- -----------------------------------------------------------------------------
INSERT INTO "OrdensServico" (
    "EquipamentoId", "SolicitanteId", "ResponsavelId",
    "DescricaoProblema", "TipoProblema", "Status",
    "DataAbertura", "DataConclusao"
)
VALUES
    -- OS 1
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248381' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        NULL,
        'Computador não liga, led do power piscando em vermelho.',
        'Não liga',
        'Pendente',
        NOW() - INTERVAL '1 day',
        NULL
    ),
    -- OS 2
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '5000201' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Lente suja, projeção está muito embaçada.',
        'Mau funcionamento',
        'Em Execucao',
        NOW() - INTERVAL '2 hours',
        NULL
    ),
    -- OS 3
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '4000101' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        'Não está refrigerando adequadamente.',
        'Mau funcionamento',
        'Concluida',
        NOW() - INTERVAL '3 days',
        NOW() - INTERVAL '2 days 22 hours'
    ),
    -- OS 4
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248382' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        NULL,
        'Monitor piscando sem parar.',
        'Mau funcionamento',
        'Pendente',
        NOW() - INTERVAL '20 hours',
        NULL
    ),
    -- OS 5
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248502' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        'Teclado não está funcionando algumas teclas.',
        'Peça quebrada',
        'Em Execucao',
        NOW() - INTERVAL '5 hours',
        NULL
    ),
    -- OS 6
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '5000201' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Sem sinal de entrada HDMI.',
        'Cabo/conexão',
        'Aguardando',
        NOW() - INTERVAL '12 hours',
        NULL
    ),
    -- OS 7
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '4000301' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        'Barulho muito alto vindo da evaporadora.',
        'Mau funcionamento',
        'Concluida',
        NOW() - INTERVAL '4 days',
        NOW() - INTERVAL '3 days 20 hours'
    ),
    -- OS 8
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248801' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        NULL,
        'Mouse não responde.',
        'Mau funcionamento',
        'Pendente',
        NOW() - INTERVAL '6 hours',
        NULL
    ),
    -- OS 9
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248901' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Computador lento e travando na inicialização.',
        'Mau funcionamento',
        'Em Execucao',
        NOW() - INTERVAL '1 hour',
        NULL
    ),
    -- OS 10
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '5000601' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Suporte do projetor está solto.',
        'Peça quebrada',
        'Concluida',
        NOW() - INTERVAL '2 days',
        NOW() - INTERVAL '1 day 22 hours'
    ),
    -- OS 11
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '4000701' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        'Controle remoto quebrado.',
        'Peça quebrada',
        'Aguardando',
        NOW() - INTERVAL '1 day 5 hours',
        NULL
    ),
    -- OS 12
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248385' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Não conecta à rede sem fio.',
        'Cabo/conexão',
        'Concluida',
        NOW() - INTERVAL '5 days',
        NOW() - INTERVAL '4 days 23 hours'
    ),
    -- OS 13
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248505' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        NULL,
        'Aparecendo tela azul da morte.',
        'Não liga',
        'Pendente',
        NOW() - INTERVAL '8 hours',
        NULL
    ),
    -- OS 14
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248605' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        'Sem áudio nas caixas de som.',
        'Cabo/conexão',
        'Em Execucao',
        NOW() - INTERVAL '3 hours',
        NULL
    ),
    -- OS 15
    (
        (SELECT "Id" FROM "Equipamentos" WHERE "NI" = '1248705' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'maria@projeto.local' LIMIT 1),
        (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'joao@projeto.local' LIMIT 1),
        'Fonte de alimentação queimada após oscilação de energia.',
        'Não liga',
        'Concluida',
        NOW() - INTERVAL '6 days',
        NOW() - INTERVAL '5 days 20 hours'
    );

-- -----------------------------------------------------------------------------
-- Históricos correspondentes
-- -----------------------------------------------------------------------------

-- OS 1
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248381';

-- OS 2
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Em Execucao';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Em Execucao';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Em Execucao';

-- OS 3
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000101';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000101';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'OS concluida', o."DataConclusao", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000101';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataConclusao" FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000101';

-- OS 4
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248382';

-- OS 5
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248502';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248502';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248502';

-- OS 6
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Aguardando';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Aguardando';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'Trabalho pausado', o."DataAbertura" + INTERVAL '1 hour', 'Aguardando novo cabo HDMI' FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Aguardando';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataAbertura" + INTERVAL '1 hour' FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000201' AND o."Status" = 'Aguardando';

-- OS 7
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000301';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000301';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'OS concluida', o."DataConclusao", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000301';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataConclusao" FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000301';

-- OS 8
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248801';

-- OS 9
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248901';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248901';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248901';

-- OS 10
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000601';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000601';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'OS concluida', o."DataConclusao", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000601';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataConclusao" FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '5000601';

-- OS 11
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000701';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000701';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'Trabalho pausado', o."DataAbertura" + INTERVAL '1 hour', 'Aguardando novo controle' FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000701';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataAbertura" + INTERVAL '1 hour' FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '4000701';

-- OS 12
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248385';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248385';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'OS concluida', o."DataConclusao", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248385';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataConclusao" FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248385';

-- OS 13
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248505';

-- OS 14
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248605';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248605';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248605';

-- OS 15
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."SolicitanteId", 'OS registrada no sistema', o."DataAbertura", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248705';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", (SELECT "Id" FROM "Usuarios" WHERE "Email" = 'admin@projeto.local'), 'Responsavel designado', o."DataAbertura" + INTERVAL '5 minutes', NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248705';
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT o."Id", o."ResponsavelId", 'OS concluida', o."DataConclusao", NULL FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248705';
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT o."Id", o."ResponsavelId", o."DataAbertura" + INTERVAL '10 minutes', o."DataConclusao" FROM "OrdensServico" o JOIN "Equipamentos" e ON o."EquipamentoId" = e."Id" WHERE e."NI" = '1248705';


-- Ajusta sequências dos IDs
SELECT setval(pg_get_serial_sequence('"Usuarios"', 'Id'),       COALESCE((SELECT MAX("Id") FROM "Usuarios"), 1));
SELECT setval(pg_get_serial_sequence('"Equipamentos"', 'Id'),  COALESCE((SELECT MAX("Id") FROM "Equipamentos"), 1));
SELECT setval(pg_get_serial_sequence('"OrdensServico"', 'Id'), COALESCE((SELECT MAX("Id") FROM "OrdensServico"), 1));
SELECT setval(pg_get_serial_sequence('"HistoricosOS"', 'Id'),  COALESCE((SELECT MAX("Id") FROM "HistoricosOS"), 1));
SELECT setval(pg_get_serial_sequence('"RegistrosTempo"', 'Id'),COALESCE((SELECT MAX("Id") FROM "RegistrosTempo"), 1));

COMMIT;
