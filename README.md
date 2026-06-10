# 🧪 G-Lab — Sistema de Ordens de Serviço (OS)

Um sistema web moderno e intuitivo desenvolvido para gerenciar ordens de serviço de equipamentos em laboratórios e salas de aula de forma ágil e organizada.

O projeto já está publicado e pronto para uso! Acesse aqui: **[G-Lab no Render](https://g-lab.onrender.com/)**

---

## 🎯 Por que desenvolvemos?
O **G-Lab** foi criado para resolver a necessidade de otimizar e organizar a manutenção e o suporte técnico de equipamentos em ambientes educacionais e laboratoriais. 
Antes, o controle de chamados e a alocação de técnicos podiam ser manuais ou confusos. O G-Lab soluciona isso ao:
* **Centralizar o suporte:** Mantém todas as ordens de serviço em um só lugar.
* **Evitar duplicidade:** Impede a abertura de múltiplos chamados para o mesmo equipamento em manutenção.
* **Equilibrar a carga de trabalho:** Mostra quantas tarefas ativas cada técnico possui antes de delegar um novo chamado.
* **Controlar o foco técnico:** Limita a execução de no máximo uma tarefa ativa por vez por colaborador.
* **Medir tempos reais:** Cronometra de forma líquida o tempo gasto em cada serviço, pausando automaticamente em períodos de espera.

## 🛠️ O que foi feito?
Implementamos as seguintes funcionalidades essenciais:
1. **Controle de Acesso (Login):** Níveis de permissão distintos para Administradores (gestores) e Colaboradores (técnicos).
2. **Cadastro e Histórico de Equipamentos:** Registro detalhado dos ativos com ou sem Número de Inventário (NI).
3. **Abertura Inteligente de OS:** Autocompletar de equipamentos via busca rápida AJAX e alertas de chamados já abertos.
4. **Gestão do Tempo de Trabalho:** Cronômetro preciso baseado nas transições de status da OS (iniciado, pausado, finalizado).
5. **Painel de Atribuição:** Visualização clara da carga de tarefas pendentes de cada técnico.

## 💻 Como desenvolvemos?
A aplicação foi construída utilizando tecnologias modernas e eficientes de desenvolvimento web:
* **Tecnologia Principal:** `ASP.NET Core MVC` (.NET 10), utilizando uma arquitetura limpa, escalável e de excelente desempenho.
* **Persistência de Dados:** `Entity Framework Core` para comunicação ágil e mapeamento objeto-relacional.
* **Banco de Dados:** `PostgreSQL` hospedado em nuvem (Supabase).
* **Interface de Usuário:** `Razor Views` combinadas com `Bootstrap` e estilos customizados em CSS para um visual moderno e responsivo.
* **Interatividade Dinâmica:** Consultas assíncronas via `AJAX` com JavaScript nativo para uma experiência ágil no navegador.

---

## 🚀 Como Executar Localmente
Para executar este projeto na sua máquina de desenvolvimento, siga os passos abaixo:

### Pré-requisitos
* Possuir o **.NET SDK 10** instalado.

### Passo a Passo
1. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```
2. Execute a aplicação:
   ```bash
   dotnet run
   ```
3. Acesse a aplicação no seu navegador no endereço que aparecer no terminal (geralmente `http://localhost:5093`).

### 🔑 Credenciais para Teste Rápido
* **Administrador Geral:** `geral@projeto.local` | Senha: `123`
* **Administrador OPP:** `admin@projeto.local` | Senha: `123`
* **Colaborador João:** `joao@projeto.local` | Senha: `123`
* **Colaboradora Maria:** `maria@projeto.local` | Senha: `123`
