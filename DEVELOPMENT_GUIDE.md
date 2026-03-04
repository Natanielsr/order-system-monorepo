# AI Development Guide - Order System

> **Arquivo de memória e regras para agentes de IA**  
> Este documento serve como guia de referência para o desenvolvimento do projeto Order System.  
> Sempre que novas regras surgirem, adicione-as aqui.

---

## 📋 **Índice**

1. [Regras Gerais](#regras-gerais)
2. [Convenções de Código](#convenções-de-código)
3. [Padrões de Testes](#padrões-de-testes)
4. [Estrutura do Projeto](#estrutura-do-projeto)
5. [Workflows Comuns](#workflows-comuns)
6. [Decisões Técnicas](#decisões-técnicas)

---

## Regras Gerais

### Língua e Comunicação

- **Chat/Conversa:** PT-BR (português do Brasil)
- **Código e Comentários:** INGLÊS (todos os identifiers, comentários, mensagens de commit, strings de UI)
- **Documentação:** INGLÊS (arquivos .md, swagger, etc.)

**Exemplo:**
```csharp
// ❌ Errado:
// Este método valida o usuário

// ✅ Correto:
// This method validates the user
public bool ValidateUser(string username) { ... }
```

### Commits

- Use inglês nas mensagens de commit
- Prefira commits pequenos e focados
- Formato: `tipo: descrição breve`
- Tipos comuns: `feat`, `fix`, `test`, `chore`, `docs`, `refactor`
- Exemplo: `feat: add user authentication handler`

### Permissão para Execução de Comandos e Alterações

- **Antes de executar qualquer comando** (`dotnet`, `git`, terminal, etc.) ou **realizar alterações no código**, **peça permissão explícita** ao responsável pelo projeto.
- Esta regra aplica-se a:
  - Criar, modificar ou excluir arquivos
  - Executar migrações de banco de dados
  - Rodar testes ou coletar cobertura
  - Fazer commits, pushes, ou alterações no repositório
- **Exceções**: Comandos de leitura (ex: `git status`, `cat`, `ls`) não precisam de permissão
- **Motivo**: Garantir integridade do código, evitar conflitos e manter qualidade do projeto

---

## Convenções de Código

### C# e .NET

- **Naming Conventions:**
  - Classes/Interfaces: `PascalCase` (ex: `UserRepository`, `IOrderService`)
  - Métodos/Propriedades: `PascalCase` (ex: `GetUserById`, `UserName`)
  - Variáveis/Parâmetros: `camelCase` (ex: `userId`, `orderItem`)
  - Constantes: `PascalCase` ou `UPPER_CASE` (siga o padrão do projeto)
  - Interfaces: prefixo `I` (ex: `IUserRepository`, `IUnitOfWork`)

- **Arquivos:**
  - Um arquivo por classe (regra geral)
  - Nome do arquivo = nome da classe (ex: `UserHandler.cs` contém `UserHandler`)
  - Namespaces seguem estrutura de pastas (ex: `OrderSystem.Application.Users.Commands`)

### Estrutura de Pastas

```
OrderSystem.Application/
├── DTOs/                    # Data Transfer Objects
│   ├── User/
│   ├── Product/
│   ├── Order/
│   └── Address/
├── Users/
│   ├── Commands/
│   │   ├── Auth/
│   │   ├── AuthEmail/
│   │   ├── CreateUser/
│   │   └── UpdateUser/
│   └── Queries/
├── Products/
│   ├── Commands/
│   │   └── CreateProduct/
│   └── Queries/
│       ├── GetAll/
│       └── GetById/
├── Addresses/
│   └── Commands/
│       └── CreateAddress/
└── Validator/              # Validators (FluentValidation)
```

---

## Padrões de Testes

### Nomenclatura de Testes

- **Classe de teste:** `[Handler/Validator]Test.cs`
  - Ex: `AuthHandlerTest`, `CreateUserValidatorTest`

- **Métodos de teste:** `[Método]_[Cenário]_[ResultadoEsperado]`
  - Ex: `Handle_ValidCredentials_ReturnsAuthResponseDto`
  - Ex: `Validate_EmptyUsername_ShouldHaveError`

### Estrutura de Testes (xUnit + Moq)

```csharp
[Fact]
public async Task Handle_ValidRequest_ReturnsExpectedResult()
{
    // Arrange
    var command = new SomeCommand(...);
    // Setup mocks
    _mockRepository.Setup(...);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(expected, result.Property);

    // Verify
    _mockRepository.Verify(..., Times.Once);
}
```

### Mocks e Assertions

- Sempre verifique interações com `Times.Once`, `Times.Never`, etc.
- Use `ReturnsAsync` para métodos assíncronos
- Para entities genéricas (Entity base), use `It.IsAny<Entity>()` nos callbacks
- Prefira `Assert.NotNull()` / `Assert.Null()` em vez de `assert != null`

### Nullable Context

- Use `#nullable disable` no início dos arquivos de teste para evitar warnings
- Para valores anuláveis, use `(User)null` (cast simples) conforme necessário

---

## Workflows Comuns

### Criando um novo Handler Test

1. Identifique o Handler e sua interface (IRequestHandler<,>)
2. Crie a pasta correspondente em `Tests/Application/[Module]/[Commands|Queries]/[HandlerName]/`
3. Crie o arquivo `[HandlerName]Test.cs`
4. Siga o padrão:
   - Mocks das dependencies (repositories, unit of work, services)
   - Setup dos retornos (ReturnsAsync, ThrowsAsync)
   - Arrange -> Act -> Assert
   - Verify chamadas importantes
5. Teste cenários:
   - ✅ Sucesso
   - ❌ Erros de validação/business
   - ❌ Exceções de Repository/Service
   - ✅ Mapeamento correto

### Criando um novo Validator Test

1. Identifique o Validator (AbstractValidator<Command>)
2. Crie ou use `Tests/Application/Validator/`
3. Use `FluentValidation.TestHelper`:
   ```csharp
   var result = _validator.TestValidate(command);
   result.ShouldNotHaveAnyValidationErrors();
   result.ShouldHaveValidationErrorFor(c => c.Property);
   ```
4. Teste todas as regras de validação
5. Teste casos extremos (null, empty, valores limites)

---

## Decisões Técnicas

### Framework e Bibliotecas

- **Testes:** xUnit
- **Mocks:** Moq
- **AutoMapper:** Profiles reais (não criar mappers manuais)
- **Validação:** FluentValidation
- **Entity Framework:** InMemory para testes de repositório (quando aplicável)

### Padrões Arquiteturais

- **CQRS:** Commands (escrita) e Queries (leitura) separados
- **MediatR:** Para envio de comandos/queries
- **Unit of Work:** Commit via `IUnitOfWork.CommitAsync()`
- **Repository Pattern:** Interfaces em `Domain.Repository`, implementações em `Infrastructure`
- **DTOs:** Usar `record class` para DTOs (C# 9+)

### Exceções Customizadas

- `UserNotFoundException`
- `InvalidPasswordException`
- `ProductNotFoundException`
- `AddressCountExceededException`
- `NotValidImageException`
- `UsernameAlreadyExistsException`
- `EmailAlreadyExistsException`
- `DuplicateProductInOrderException`

Sempre lançar exceções específicas, não genéricas.

---

## Estado do Projeto (Snapshot)

### ✅ Completos (100% coverage target)

- **Auth:** `AuthHandler`, `AuthEmailHandler`
- **Users:** `CreateUserHandler`, `GetUserByIdHandler`, `UpdateUserHandler`, `CreateUserValidator`
- **Products:** `CreateProductHandler`, `GetAllProductsHandler`, `GetProductByIdHandler`, `CreateProductValidator`, `ImageValidator`
- **Addresses:** `CreateAddressHandler`, `GetUserAddressesHandler`, `GetAddressByIdHandler`, `UpdateAddressHandler`, `DisableAddressHandler`, `DeleteAddressHandler` **(falta apenas: `CreateAddressValidator`)**
- **Orders:** `CreateOrderHandler`, `GetOrderByIdHandler`, `ListOrdersHandler`, `ListUserOrdersHandler`, `CreateOrderValidator`

### 🔄 Parciais

- **AddressValidator:** `CreateAddressValidator` (validator em falta para Addresses)

### ⏳ Não iniciados

Nenhum módulo pendente. Todos os handlers e validators principais estão implementados.

---

## Lembretes Importantes

1. **Sempre rodar testes** após mudanças: `dotnet test`
2. **Verificar cobertura** periodicamente: `dotnet test --collect:"XPlat Code Coverage"`
3. **Commits separados** por funcionalidade (não juntar tudo em um)
4. **Mensagens de commit em inglês**
5. **Comentários de código em inglês**
6. **Não commitar dados sensíveis** (senhas, chaves)
7. **Manter .gitignore** atualizado

---

## Como UsarEsteGuia

1. Leia este arquivo antes de iniciar qualquer desenvolvimento
2. Consulte as convenções antes de escrever código
3. Adicione novas regras aqui conforme o projeto evolui
4. Passe este guia para outros agentes/desenvolvedores

---

**Última atualização:** 2025-06-18  
**Versão:** 1.2
