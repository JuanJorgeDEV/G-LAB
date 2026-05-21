-- =============================================================================
-- ProjetoOS — Seed de demonstração (PostgreSQL / Supabase)
-- =============================================================================
-- Como usar:
--   1. Supabase → SQL Editor → New query → cole este arquivo → Run
--   2. Ou: psql "$CONNECTION_STRING" -f scripts/seed-dados-demo.sql
--
-- ATENÇÃO: apaga TODOS os dados das tabelas da aplicação e recria do zero.
-- Senha de todos os usuários criados aqui: 123 (texto plano; trocar na Parte 3)
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
-- Usuários (28): 3 contas fixas + 25 colaboradores variados
-- -----------------------------------------------------------------------------
INSERT INTO "Usuarios" ("Nome", "Email", "Senha", "Perfil")
VALUES
    ('OPP Administrador',      'admin@projeto.local',  '123', 'Administrador'),
    ('Joao Colaborador',     'joao@projeto.local',   '123', 'Colaborador'),
    ('Maria Colaboradora',   'maria@projeto.local',  '123', 'Colaborador'),
    ('Carlos Silva',         'carlos@projeto.local', '123', 'Colaborador'),
    ('Ana Souza',            'ana@projeto.local',    '123', 'Colaborador'),
    ('Pedro Lima',           'pedro@projeto.local',  '123', 'Administrador'),
    ('Fernanda Costa',       'fernanda@projeto.local','123','Colaborador'),
    ('Ricardo Alves',        'ricardo@projeto.local','123', 'Colaborador'),
    ('Juliana Mendes',       'juliana@projeto.local','123', 'Colaborador'),
    ('Bruno Oliveira',       'bruno@projeto.local',  '123', 'Colaborador'),
    ('Camila Rocha',         'camila@projeto.local', '123', 'Colaborador'),
    ('Diego Martins',        'diego@projeto.local',  '123', 'Colaborador'),
    ('Larissa Ferreira',     'larissa@projeto.local','123', 'Colaborador'),
    ('Gustavo Pereira',      'gustavo@projeto.local','123', 'Colaborador'),
    ('Patricia Gomes',       'patricia@projeto.local','123','Colaborador'),
    ('Rodrigo Barbosa',      'rodrigo@projeto.local','123', 'Colaborador'),
    ('Amanda Dias',          'amanda@projeto.local', '123', 'Colaborador'),
    ('Felipe Cardoso',       'felipe@projeto.local', '123', 'Colaborador'),
    ('Beatriz Nunes',        'beatriz@projeto.local','123', 'Colaborador'),
    ('Thiago Ramos',         'thiago@projeto.local', '123', 'Colaborador'),
    ('Vanessa Teixeira',     'vanessa@projeto.local','123', 'Colaborador'),
    ('Lucas Carvalho',       'lucas@projeto.local',  '123', 'Colaborador'),
    ('Renata Monteiro',      'renata@projeto.local', '123', 'Colaborador'),
    ('Marcos Araujo',        'marcos@projeto.local', '123', 'Colaborador'),
    ('Helena Castro',        'helena@projeto.local', '123', 'Colaborador'),
    ('Igor Pinto',           'igor@projeto.local',   '123', 'Colaborador'),
    ('Simone Freitas',       'simone@projeto.local', '123', 'Colaborador'),
    ('Paulo Cunha',          'paulo@projeto.local',  '123', 'Colaborador');

-- -----------------------------------------------------------------------------
-- Equipamentos (65): 50 com NI único + 15 sem NI
-- -----------------------------------------------------------------------------
INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT
    true,
    'NI-' || LPAD(g::text, 5, '0'),
    (ARRAY[
        'Projetor Epson', 'Notebook Dell', 'Impressora HP', 'Impressora 3D', 'TV Samsung',
        'Lousa digital', 'Nobreak APC', 'Roteador Cisco', 'Switch 24 portas', 'Microfone sem fio',
        'Caixa de som', 'Scanner Canon', 'Tablet educacional', 'Chromebook', 'Servidor rack',
        'Ar-condicionado', 'Camera IP', 'DVR gravador', 'Estabilizador', 'Multimetro digital',
        'Osciloscopio', 'Fonte bench', 'Solda estacao', 'Compressor ar', 'Torno CNC',
        'Fresadora', 'Plotter recorte', 'Balança precision', 'Bancada ESD', 'Kit ferramentas'
    ])[1 + (g % 30)],
    'Equipamento de laboratorio/sala - item ' || g,
    (ARRAY['Sala 1','Sala 2','Sala 3','Sala 4','Sala 5','Sala 6','Sala 7','Sala 8',
           'Laboratorio A','Laboratorio B','Laboratorio C','Oficina','Estoque','Recepcao'])[1 + (g % 14)],
    (ARRAY['Aprendizagem','FIC','Outro'])[1 + (g % 3)],
    (g % 17 <> 0)  -- ~1 equipamento inativo
FROM generate_series(1, 50) AS g;

INSERT INTO "Equipamentos" ("PossuiNI", "NI", "Nome", "Descricao", "Localizacao", "Vinculo", "Ativo")
SELECT
    false,
    NULL,
    'Item avulso ' || g,
    'Material/consumivel sem NI cadastrado',
    (ARRAY['Sala 2','Sala 5','Corredor','Estoque'])[1 + (g % 4)],
    (ARRAY['Aprendizagem','FIC'])[1 + (g % 2)],
    true
FROM generate_series(1, 15) AS g;

-- -----------------------------------------------------------------------------
-- Ordens de serviço (120) — status e datas variados nos últimos 90 dias
-- -----------------------------------------------------------------------------
INSERT INTO "OrdensServico" (
    "EquipamentoId", "SolicitanteId", "ResponsavelId",
    "DescricaoProblema", "TipoProblema", "Status",
    "DataAbertura", "DataConclusao"
)
SELECT
    1 + (s % 65)                                                    AS "EquipamentoId",
    1 + (s % 28)                                                    AS "SolicitanteId",
    CASE
        WHEN status_txt IN ('Em Execucao', 'Aguardando', 'Concluida')
        THEN 1 + ((s * 3) % 28)
        ELSE NULL
    END                                                             AS "ResponsavelId",
    'Chamado #' || s || ': ' || (ARRAY[
        'Equipamento nao liga apos queda de energia.',
        'Imagem com listras na projecao.',
        'Conexao de rede intermitente no laboratorio.',
        'Ruido excessivo no funcionamento.',
        'Tecla do teclado travada / sem resposta.',
        'Vazamento de tinta na impressora.',
        'Superaquecimento reportado pelos alunos.',
        'Software educacional nao abre apos atualizacao.',
        'Cabo HDMI com mau contato.',
        'Solicitada limpeza e organizacao da bancada.'
    ])[1 + (s % 10)]                                                AS "DescricaoProblema",
    (ARRAY[
        'Não liga', 'Mau funcionamento', 'Cabo/conexão',
        'Peça quebrada', 'Falta de material', 'Limpeza/organização'
    ])[1 + (s % 6)]                                                AS "TipoProblema",
    status_txt,
    abertura,
    CASE WHEN status_txt = 'Concluida' THEN abertura + ((s % 72) + 2) * INTERVAL '1 hour' ELSE NULL END
FROM (
    SELECT
        s,
        (ARRAY['Pendente','Em Execucao','Aguardando','Concluida'])[1 + (s % 4)] AS status_txt,
        TIMESTAMPTZ '2026-01-01 08:00:00+00'
            + ((s * 17) % 90) * INTERVAL '1 day'
            + ((s * 5) % 8) * INTERVAL '1 hour'                          AS abertura
    FROM generate_series(1, 120) AS s
) AS base;

-- -----------------------------------------------------------------------------
-- Histórico das OS (3–4 eventos por OS = ~420 linhas)
-- -----------------------------------------------------------------------------
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT
    o."Id",
    COALESCE(o."ResponsavelId", o."SolicitanteId"),
    ev.acao,
    o."DataAbertura" + ev.delta,
    ev.obs
FROM "OrdensServico" o
CROSS JOIN LATERAL (
    VALUES
        (INTERVAL '0 minutes',  'OS registrada no sistema',                    NULL),
        (INTERVAL '15 minutes', 'Triagem inicial pela OPP',                    'Prioridade definida'),
        (INTERVAL '2 hours',    'Tecnico designado',                           'Encaminhado ao responsavel'),
        (INTERVAL '1 day',      'Comentario de acompanhamento',                'Aguardando retorno do solicitante')
) AS ev(delta, acao, obs)
WHERE o."Id" <= 120;

-- Histórico extra para OS concluídas
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT
    o."Id",
    o."ResponsavelId",
    'OS concluida',
    o."DataConclusao",
    'Encerramento apos validacao'
FROM "OrdensServico" o
WHERE o."Status" = 'Concluida' AND o."ResponsavelId" IS NOT NULL;

-- Histórico de pausa para OS aguardando
INSERT INTO "HistoricosOS" ("OrdemServicoId", "UsuarioId", "Acao", "DataHora", "Observacao")
SELECT
    o."Id",
    o."ResponsavelId",
    'OS pausada',
    o."DataAbertura" + INTERVAL '6 hours',
    (ARRAY[
        'Aguardando peca',
        'Aguardando liberacao',
        'Aguardando outro responsavel'
    ])[1 + (o."Id" % 3)]
FROM "OrdensServico" o
WHERE o."Status" = 'Aguardando' AND o."ResponsavelId" IS NOT NULL;

-- -----------------------------------------------------------------------------
-- Registros de tempo (2 blocos por OS em execução ou concluída com responsável)
-- -----------------------------------------------------------------------------
INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT
    o."Id",
    o."ResponsavelId",
    o."DataAbertura" + INTERVAL '30 minutes',
    o."DataAbertura" + INTERVAL '2 hours'
FROM "OrdensServico" o
WHERE o."ResponsavelId" IS NOT NULL
  AND o."Status" IN ('Em Execucao', 'Aguardando', 'Concluida');

INSERT INTO "RegistrosTempo" ("OrdemServicoId", "ResponsavelId", "Inicio", "Fim")
SELECT
    o."Id",
    o."ResponsavelId",
    o."DataAbertura" + INTERVAL '1 day',
    CASE
        WHEN o."Status" = 'Em Execucao' THEN NULL
        WHEN o."Status" = 'Concluida'  THEN o."DataConclusao"
        ELSE o."DataAbertura" + INTERVAL '1 day 3 hours'
    END
FROM "OrdensServico" o
WHERE o."ResponsavelId" IS NOT NULL
  AND o."Status" IN ('Em Execucao', 'Aguardando', 'Concluida');

-- Ajusta sequências dos IDs (por garantia após INSERTs com generate_series)
SELECT setval(pg_get_serial_sequence('"Usuarios"', 'Id'),       COALESCE((SELECT MAX("Id") FROM "Usuarios"), 1));
SELECT setval(pg_get_serial_sequence('"Equipamentos"', 'Id'),  COALESCE((SELECT MAX("Id") FROM "Equipamentos"), 1));
SELECT setval(pg_get_serial_sequence('"OrdensServico"', 'Id'), COALESCE((SELECT MAX("Id") FROM "OrdensServico"), 1));
SELECT setval(pg_get_serial_sequence('"HistoricosOS"', 'Id'),  COALESCE((SELECT MAX("Id") FROM "HistoricosOS"), 1));
SELECT setval(pg_get_serial_sequence('"RegistrosTempo"', 'Id'),COALESCE((SELECT MAX("Id") FROM "RegistrosTempo"), 1));

COMMIT;

-- -----------------------------------------------------------------------------
-- Conferência rápida (opcional — rode separado se quiser)
-- -----------------------------------------------------------------------------
-- SELECT 'Usuarios' AS tabela, COUNT(*) FROM "Usuarios"
-- UNION ALL SELECT 'Equipamentos', COUNT(*) FROM "Equipamentos"
-- UNION ALL SELECT 'OrdensServico', COUNT(*) FROM "OrdensServico"
-- UNION ALL SELECT 'HistoricosOS', COUNT(*) FROM "HistoricosOS"
-- UNION ALL SELECT 'RegistrosTempo', COUNT(*) FROM "RegistrosTempo";

-- SELECT "Status", COUNT(*) FROM "OrdensServico" GROUP BY "Status" ORDER BY 2 DESC;
