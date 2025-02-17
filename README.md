# JokenpoBet

A JokenpoBet é uma casa de apostas onde você pode apostar no clássico jogo de pedra, papel ou tesoura. As regras do jogo são as seguintes:

- Pedra vence tesoura.
- Tesoura vence papel.
- Papel vence pedra.

### Regras de negócio:
- Somente usuários do tipo PLAYER podem fazer apostas.
- As apostas devem ser de no mínimo R$ 1,00.
- Após acumular 5 apostas perdidas o jogador recebe um bonus de 10% que foi gasto nessas apostas.
- A cada aposta ganha o jogador recebe o dobro do valor da aposta.

<br>

## Executando a API

1. **.NET SDK**: Certifique-se de ter o .NET 6 SDK instalado. Você pode baixar a versão mais recente em [dotnet.microsoft.com](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0).

2. **Ferramentas de Desenvolvimento**: Você pode usar qualquer editor de código, mas recomendo o Visual Studio.

1. **Execute o container Postgres** <br>
    A API utiliza um banco de dados PostgreSQL que roda em um container Docker. O arquivo `desafio/docker-compose.yml` está configurado para facilitar a execução.
o projeto também acompanha um MER localizado na raiz `MER-desafio.png` 
    
 
1. **Clone o projeto**

1. **Navegue até o diretório da API**:
   ```bash
   cd /desafio
   ```

1. **Suba o container docker com o banco de dados**
    ```
    docker compose up -d  
    ```

1. **Verifique se o container está rodando**
    ```
    docker ps
    ```

1. **Execute as migrations**
    ```
    dotnet ef database update
    ```

2. **Restaure as dependências** (caso ainda não tenha feito):
   ```bash
   dotnet restore
   ```

3. **Execute a API**:
   ```bash
   dotnet run
   ```

1. **Acessar o swagger**, a API conta com uma interface gráfica para visualizar os endpoints e pode ser acessada após executar o projeto
    ```
    http://localhost:{porta}/swagger
    ```
   
<br>

## Executando os Testes

1. **Navegue até o diretório de testes**:
   ```bash
   cd /DesafioTests
   ```

2. **Restaure as dependências dos testes** (caso ainda não tenha feito):
   ```bash
   dotnet restore
   ```

3. **Execute os testes**:
   ```bash
   dotnet test
   ```

<br>

## Endpoints

Para facilitar o teste dos endpoints utilize a collection do Postman, localizada no arquivo `JokenpoBet.postman_collection.json` na raiz do projeto 

### Usuário 

#### POST /api/User
Faz o cadastro do usuário na API.

**Corpo da requisição (Body)** <br>
```
{
    "nome": "Tailyne",
    "email": "tailyne@gmail.com",
    "senha": "12345678",
    "tipoUsuario": "PLAYER",
    "saldoInicial": 100
}
```

O campo `tipoUsuario` deve ser:
- `"ADMIN"` - Para os administradores da plataforma
- `"PLAYER"` - Para os jogadores da plataforma

<br>

#### GET /api/User/allUsers
Busca todos os usuários. Este endpoint é permitido somente para os usuários do tipo `ADMIN`.

**Cabeçalho da Requisição:**

- Authorization: `Bearer {token}` (necessário para autenticação)

<br>

#### PUT /api/User/updatePassword
Esse endpoint altera a senha do usuário.

**Cabeçalho da Requisição:**

- Authorization: `Bearer {token}` (necessário para autenticação)

**Corpo da requisição (Body)** <br>
```
{
    "email": "tailyne@gmail.com",
    "senhaAtual": "87654321",
    "novaSenha": "12345678"
}
```

<br>

### Login

#### POST /api/Login
Esse endpoint gera o token para a autenticação de acordo com o usuário já cadastrado

**Corpo da requisição (Body)** <br>
```
{
    "email": "tailyne@gmail.com",
    "senha": "12345678"
}
```

<br>

### Transações

#### POST /api/Transaction
Esse endpoint faz uma aposta na plataforma.

**Pré-requisitos:**
- A aposta mínima é R$ 1,00.
- Possuir saldo em carteira suficiente para realizar a aposta.
- Escolher um palpite válido para a aposta:
    - PEDRA
    - PAPEL
    - TESOURA

**Cabeçalho da Requisição:**
- Authorization: `Bearer {token}` (necessário para autenticação)

**Corpo da requisição (Body)** <br>
```
{
    "valor": 5,
    "palpite": "PAPEL"
}
```

<br>

#### PUT /api/Transaction/canceled/id
Esse endpoint cancela a transação. Este endpoint é permitido somente para os usuários do tipo `ADMIN`.

**Pré-requisitos:**
- Adicionar o id da transação na URL do endpoint `{id}`.

**Cabeçalho da Requisição:** <br>
Authorization: `Bearer {token}` (necessário para autenticação)

<br>

#### GET /api/Transaction/allTransactions
Esse endpoint lista todas as transações paginadas e pode ser adicionado um tipo de transação para ser feito uma filtragem, com as seguintes opções
- APOSTA
- PREMIO
- RECARGA
- BONUS

**Cabeçalho da Requisição:** <br>
Authorization: `Bearer {token}` (necessário para autenticação)

**Corpo da requisição (Body)** <br>
```
{
    "pagina": 1,
    "registrosPorPagina": 10,
    "tipo": "APOSTA"
}
```