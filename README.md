# 💡 Humanize — Tecnologia que valoriza o lado humano do trabalho

O Humanize é uma plataforma que une tecnologia, empatia e gamificação para fortalecer o relacionamento entre gestores e suas equipes. 
A proposta é ajudar empresas a acompanharem o bem-estar e o engajamento dos colaboradores, sem vigilância nem competição. 
A IA e os dados servem apenas como apoio ao olhar humano, organizando informações para que gestores possam agir com mais empatia e consciência.
#### A Inteligência Artificial apoia, mas quem cuida, compreende e transforma é o ser humano.


## 🚀 Funcionalidades
- Check-ins semanais com perguntas rápidas sobre humor e carga de trabalho
- Gamificação positiva com pontos e badges de engajamento
- Recompensas reais com vouchers por participação contínua
- Dashboard para gestores com métricas de humor e engajamento
- Relatórios automáticos com sugestões geradas por IA
- Interface intuitiva e acolhedora, com linguagem positiva e microinterações


## 🧩 Requisitos funcionais
- O sistema deve permitir o cadastro de usuários (gestores e colaboradores)
- O colaborador deve poder responder check-ins semanais
- O sistema deve registrar e armazenar histórico de respostas e humor
- O colaborador deve receber pontos e badges conforme a constância de respostas
- O gestor deve poder visualizar o painel com dados e gráficos do time
- A IA deve apresentar os dados da equipe aos gestores, sem tomar decisões por eles

## 🧱 Requisitos não funcionais
- O backend deve ser desenvolvido em .NET
- O frontend deve ser desenvolvido em React Native
- O banco de dados Oracle deve armazenar respostas, pontos e recompensas
- O sistema deve garantir segurança e privacidade dos dados dos colaboradores
- O design deve ser leve, positivo e inspirar empatia

## 🛠️ Tecnologias
- Backend: .NET (API REST)
- Banco de Dados: Oracle
- Frontend: React Native com Expo
- Gerenciamento de Estado: Context API
- Swagger/OpenAPI*: Documentação automática

### Arquitetura
- **Clean Architecture**: Separação clara entre camadas (Controllers, DTOs, Entities, Repositories)
- **Repository Pattern**: Abstração do acesso a dados
- **Entity Framework Core**: ORM para acesso ao banco Oracle
- **Dependency Injection**: Injeção de dependências nativa do .NET

### Estrutura do Projeto
```
Humanize/
├── Controllers/ # Controladores da API
├── DTOs/         # Data Transfer Objects
├── Infrastructure/      
│   └── Persistence/
│   ├── Entities/    # Entidades do banco de dados
│       ├── Repositories/# Implementação dos repositórios
│       └── Configurations/# Configurações do Entity Framework
├── Application/       # Serviços de aplicação
├── UseCase/       # Casos de uso
└── Program.cs          # Configuração da aplicação
```
### Estrutura das Tabelas
```sql
-- Tabelas principais
T_HUMANIZE_EQUIPE        -- Equipes
T_HUMANIZE_USUARIO       -- Usuários (colaboradores e gestores)
T_HUMANIZE_VOUCHER       -- Vouchers/recompensas
T_HUMANIZE_PERGUNTA      -- Perguntas dos check-ins
T_HUMANIZE_AVALIACAO     -- Avaliações dos usuários
T_HUMANIZE_RESPOSTA      -- Respostas às perguntas
```

### Relacionamentos
- `Usuario` → `Equipe` (N:1)
- `Usuario` → `Voucher` (N:1, opcional)
- `Avaliacao` → `Usuario` (N:1)
- `Resposta` → `Avaliacao` (N:1)
- `Resposta` → `Pergunta` (N:1)

## Rotas e endpoints
### Usuários (`/api/Usuario`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca usuários com filtros | Query params |
| `GET` | `/{id}` | Buscar por ID | - |
| `GET` | `/email/{email}` | Buscar por email | - |
| `POST` | `/` | Criar usuário | CreateUsuarioDTO |
| `PUT` | `/{id}` | Atualizar usuário | UpdateUsuarioDTO |
| `DELETE` | `/{id}` | Deletar usuário | - |
| `GET` | `/check-email/{email}` | Verificar se email existe | - |

### Equipes (`/api/Equipe`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca equipes com filtros | Query params |
| `GET` | `/` | Listar todas as equipes | - |
| `GET` | `/{id}` | Buscar por ID | - |
| `GET` | `/nome/{nome}` | Buscar por nome | - |
| `GET` | `/com-usuarios` | Equipes com usuários | - |
| `POST` | `/` | Criar equipe | CreateEquipeDTO |
| `PUT` | `/{id}` | Atualizar equipe | UpdateEquipeDTO |
| `DELETE` | `/{id}` | Deletar equipe | - |

### Vouchers (`/api/Voucher`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca vouchers com filtros | Query params |
| `GET` | `/` | Listar todos os vouchers | - |
| `GET` | `/{id}` | Buscar por ID | - |
| `GET` | `/validos` | Vouchers válidos | - |
| `GET` | `/vencidos` | Vouchers vencidos | - |
| `GET` | `/status/{status}` | Buscar por status | - |
| `POST` | `/` | Criar voucher | CreateVoucherDTO |
| `PUT` | `/{id}` | Atualizar voucher | UpdateVoucherDTO |
| `DELETE` | `/{id}` | Deletar voucher | - |

### Perguntas (`/api/Pergunta`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca perguntas com filtros | Query params |
| `GET` | `/` | Listar todas as perguntas | - |
| `GET` | `/{id}` | Buscar por ID | - |
| `GET` | `/buscar/{titulo}` | Buscar por título | - |
| `GET` | `/com-respostas` | Perguntas com respostas | - |
| `POST` | `/` | Criar pergunta | CreatePerguntaDTO |
| `PUT` | `/{id}` | Atualizar pergunta | CreatePerguntaDTO |
| `DELETE` | `/{id}` | Deletar pergunta | - |

### Avaliações (`/api/Avaliacao`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca avaliações com filtros | Query params |
| `GET` | `/` | Listar todas as avaliações | - |
| `GET` | `/{id}` | Buscar por ID | - |
| `POST` | `/` | Criar avaliação | CreateAvaliacaoDTO |
| `PUT` | `/{id}` | Atualizar avaliação | CreateAvaliacaoDTO |
| `DELETE` | `/{id}` | Deletar avaliação | - |

### Respostas (`/api/Resposta`)
| Método | Endpoint | Descrição | Body |
|--------|----------|-----------|------|
| `GET` | `/search` | Busca respostas com filtros | Query params |
| `GET` | `/` | Listar todas as respostas | - |
| `GET` | `/{id}` | Buscar por ID | - |
| `POST` | `/` | Criar resposta | CreateRespostaDTO |
| `PUT` | `/{id}` | Atualizar resposta | CreateRespostaDTO |
| `DELETE` | `/{id}` | Deletar resposta | - |

## 📋 Exemplos de Uso

### 1. Swagger UI
Acesse `http://localhost:5168/swagger` para testar todos os endpoints interativamente.

### 2. Curl - Criar Usuário
```bash
curl -X POST "http://localhost:5168/api/Usuario" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao.silva@empresa.com",
    "senha": "senha123",
    "tipo": "Colaborador",
    "equipeId": 1
  }'
```

### 3. Curl - Buscar Usuários
```bash
curl -X GET "http://localhost:5168/api/Usuario/search?page=1&pageSize=10&tipo=Colaborador"
```


## 🚀 Como Rodar o Projeto 

### Pré-requisitos
- .NET 9 SDK
- Oracle Database (acesso ao servidor FIAP)
 
####  *Para o front-end:*
- Node.js instalado (versão 16 ou superior)
- Expo CLI instalado (`npm install -g expo-cli`)
- Expo Go instalado no celular (Android ou iOS)
- Backend .NET rodando

### Executar a Aplicação
```bash
# Modo desenvolvimento
dotnet run

# Ou especificar perfil
dotnet run --launch-profile http
```
 - Acesse `http://0.0.0.0:5168/swagger`[substitua o '0.0.0.0' pelo ip da sua máquina*] para verificar a API

### Passo 2: *Descobrir seu IP Local
1. Abra o terminal do seu computador (PowerShell ou CMD)
2. Execute o comando:
   ```powershell
   ipconfig
   ```
3. Localize o **Endereço IPv4** da sua rede 
4. Anote este endereço IP

### FRONT-END
### Passo 3: Configurar o IP no Projeto Mobile
1. Abra o arquivo `src/api/apiClient.ts`
2. Na linha 3, altere a constante `BASE_URL` para o seu IP local:
   ```typescript
   const BASE_URL = 'http://SEU_IP_AQUI:5168';
   ```
   **Exemplo**: Se seu IP é `192.168.1.10`, ficará:
   ```typescript
   const BASE_URL = 'http://192.168.1.10:5168';
   ```
3. Salve o arquivo

### Passo 4: Instalar Dependências
No terminal, dentro da pasta do projeto, execute:
```powershell
npm install
```

### Passo 5: Iniciar o Projeto
Execute o comando:
```powershell
npx expo start
```

### Passo 6: Abrir no Celular
1. Aguarde o QR Code aparecer no terminal
2. Abra o app **Expo Go** no seu celular
3. Escaneie o QR Code exibido no terminal
4. Aguarde o app carregar

### ⚠️ Importante
- **Celular e computador devem estar na mesma rede Wi-Fi**
- **O backend deve estar rodando** antes de iniciar o app mobile
- Se houver erro de conexão, verifique se:
  - O IP em `apiClient.ts` está correto
  - O backend está rodando na porta 5168
  - Não há firewall bloqueando a conexão
  - Você usou o IP correto da interface de rede ativa

## 🌍 ODS Relacionados
- ODS 3 – Saúde e Bem-Estar: promove ambientes saudáveis e previne burnout.
- ODS 8 – Trabalho Decente e Crescimento Econômico: estimula cultura organizacional mais humana.
- ODS 10 – Redução das Desigualdades: dá voz a todos, de forma igual e empática.

## 👥 Equipe
- Barbara Bonome Filipus - RM 560431 | 2TDSPR
- Vinicius Lira Ruggeri - RM 560593 | 2TDSPR
- Yasmin Pereira da Silva - RM 560039 | 2TDSPR
