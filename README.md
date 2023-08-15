# CampCorr

Sistema para pontuação de campeonatos de corrida

Bem-vindo ao CampCorr! Este sistema é projetado para facilitar a gestão e pontuação de campeonatos de corrida, independentemente da modalidade. Ele permite o cadastro de pilotos, campeonatos, temporadas, equipes, etapas e resultados, bem como o cálculo de pontos com base nas regras do campeonato.

## Funcionalidades

- **Cadastro de Piloto**: Permite o cadastro de pilotos sem requisitos prévios.
- **Cadastro de Campeonato**: Possibilita o registro de campeonatos.
- **Cadastro de Temporada**: Um campeonato pode ter várias temporadas, organizadas por ano. Os pilotos e equipes são cadastrados em cada temporada. As etapas e regulamentos também são definidos para cada temporada.
  - **Cadastro de Equipe**: Permite a nomeação de equipes para o campeonato.
  - **Pilotos do Campeonato**: Somente pilotos cadastrados podem ser adicionados ao campeonato.
- **Cadastro de Etapa**: Cada temporada pode ter várias etapas, que precisam de data e local obrigatórios.
- **Cadastro de Resultado**: Após cada corrida do campeonato, os resultados podem ser cadastrados, incluindo as posições dos pilotos. (Note que os campos obrigatórios podem variar de acordo com as regras do campeonato.)
- **Cálculo Parcial**: O sistema permite acompanhar a pontuação do campeonato após o cadastro dos resultados das etapas.
- **Resultado da Temporada**: Após o cadastro dos resultados de todas as etapas, o sistema calcula e exibe o campeão da temporada.

## Requisitos do Sistema

Certifique-se de ter os seguintes requisitos instalados em seu sistema antes de executar o CampCorr:

- **.NET Core SDK** (Versão 6.0 ou superior)
- **ASP.NET Core Runtime** (Versão 6.0 ou superior)
- **Banco de Dados SQL Server** (ou outro banco de dados compatível)

## Dependências

Certifique-se de incluir as seguintes dependências em seu projeto:

- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**:
  - Versão: 6.0.14

- **Microsoft.EntityFrameworkCore.Design**:
  - Versão: 6.0.14

- **Microsoft.EntityFrameworkCore.SqlServer**:
  - Versão: 6.0.14

- **Microsoft.EntityFrameworkCore.Tools**:
  - Versão: 6.0.14

- **Microsoft.VisualStudio.Web.CodeGeneration.Design**:
  - Versão: 6.0.12

- **MockQueryable.Moq**:
  - Versão: 6.0.1

- **ReflectionIT.Mvc.Paging**:
  - Versão: 6.0.1

Certifique-se de adicionar essas dependências ao arquivo de projeto conforme necessário. Lembre-se de verificar se há atualizações ou versões mais recentes disponíveis quando você for implementar a aplicação.



