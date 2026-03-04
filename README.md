# Sistema de Pedidos - Order System

Sistema completo de gerenciamento de pedidos com frontend React/Next.js e backend ASP.NET Core em arquitetura limpa (Clean Architecture).

## 🚀 Tecnologias

### Frontend
- [Next.js 15](https://nextjs.org/)
- [React 19](https://react.dev/)
- [TypeScript](https://www.typescriptlang.org/)
- [Tailwind CSS](https://tailwindcss.com/)
- [Axios](https://axios-http.com/)

### Backend
- [ASP.NET Core](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/)
- [PostgreSQL](https://www.postgresql.org/)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/)
- [AutoMapper](https://automapper.org/)
- [JWT Authentication](https://jwt.io/)

### DevOps
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

---

## 📁 Estrutura do Projeto

```
.
├── .git/
├── .gitignore
├── docker-compose.yml
├── package-lock.json
├── package.json
├── README.md
├── apps/
│   ├── backend/
│   │   ├── .gitignore
│   │   ├── dotnet-tools.json
│   │   ├── dotnet_ef_commands.txt
│   │   ├── OrderSystem.sln
│   │   ├── OrderSystem.Tests/
│   │   │   ├── Application/
│   │   │   ├── Domain/
│   │   │   ├── Infrastructure/
│   │   │   ├── GlobalUsings.cs
│   │   │   └── OrderSystem.Tests.csproj
│   │   ├── OrderSystem.API/
│   │   ├── OrderSystem.Application/
│   │   ├── OrderSystem.Domain/
│   │   ├── OrderSystem.Infrastructure/
│   │   └── OrderSystem.sln
│   └── frontend/
│       ├── .gitignore
│       ├── eslint.config.mjs
│       ├── next.config.ts
│       ├── package-lock.json
│       ├── package.json
│       ├── postcss.config.mjs
│       ├── tsconfig.json
│       ├── app/
│       │   ├── address/
│       │   ├── cart/
│       │   ├── checkout/
│       │   │   ├── loading.tsx
│       │   │   ├── page.tsx
│       │   │   └── success/
│       │   ├── favicon.ico
│       │   ├── globals.css
│       │   ├── layout.tsx
│       │   ├── loading.tsx
│       │   ├── login/
│       │   ├── order/
│       │   │   ├── [id]/
│       │   │   └── page.tsx
│       │   ├── page.tsx
│       │   ├── payment/
│       │   │   ├── change/
│       │   │   └── page.tsx
│       │   ├── profile/
│       │   ├── register/
│       │   └── register/page.tsx
│       ├── components/
│       │   ├── AddressCard.tsx
│       │   ├── AddressSelector.tsx
│       │   ├── AlertWrapper.tsx
│       │   ├── App.tsx
│       │   ├── Button.tsx
│       │   ├── CartIcon.tsx
│       │   ├── CartItemComponent.tsx
│       │   ├── CartSidebar.tsx
│       │   ├── ConfirmDeleteModal.tsx
│       │   ├── Header.tsx
│       │   ├── LoginStatus.tsx
│       │   ├── MobileSidebar.tsx
│       │   ├── PaymentResume.tsx
│       │   ├── PaymentSelector.tsx
│       │   ├── ProductCard.tsx
│       │   ├── ProductImage.tsx
│       │   ├── ProductList.tsx
│       │   ├── SimpleAlert.tsx
│       │   ├── Spinner.tsx
│       │   └── UserDropdown.tsx
│       ├── config/
│       │   └── api.ts
│       ├── context/
│       │   ├── AuthContext.tsx
│       │   └── CartContext.tsx
│       ├── public/
│       │   ├── file.svg
│       │   ├── globe.svg
│       │   ├── next.svg
│       │   ├── no-image.jpg
│       │   ├── vercel.svg
│       │   └── window.svg
│       ├── types/
│       │   ├── address.ts
│       │   ├── auth.ts
│       │   ├── cart.ts
│       │   ├── order.ts
│       │   ├── orderItem.ts
│       │   ├── paymentInfo.ts
│       │   ├── product.ts
│       │   └── user.ts
│       └── utils/
│           ├── format.ts
│           ├── mask.ts
│           ├── orderStatus.tsx
│           └── validation.ts
└── README.md (frontend)
```

---

## 🏗️ Arquitetura do Backend (Clean Architecture)

### OrderSystem.Domain
- Entidades (User, Product, Order, Address, PaymentInfo, OrderProduct)
- Interfaces de Repository e Unit of Work
- Serviços de domínio (IPasswordService)
- Exceções customizadas

### OrderSystem.Application
- DTOs e Commands/Queries (MediatR)
- Validadores (FluentValidation)
- Behaviors (logging, validation, etc.)
- Mappers (AutoMapper)
- Injeção de dependências

### OrderSystem.Infrastructure
- Entity Framework Core (DbContext, Migrations)
- Repositórios concretos (EntityFramework)
- Serviços de infraestrutura (JWTTokenService, PasswordService, LocalStorageService)
- Unit of Work

### OrderSystem.API
- Controllers RESTful
- Middlewares (autenticação, erro, etc.)
- Program.cs com configuração de serviços

### OrderSystem.Tests
- Testes unitários e de integração
- Testes para Application, Domain e Infrastructure

---

## 📦 Instalação e Execução

### Pré-requisitos
- [Node.js 20+](https://nodejs.org/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker & Docker Compose](https://www.docker.com/)
- PostgreSQL (se não usar Docker)

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd order-system
```

### 2. Backend

#### Usando Docker (Recomendado)
```bash
cd apps/backend
docker-compose up -d
```

A API estará disponível em: `http://localhost:5012/api`

#### Execução Local
```bash
cd apps/backend
dotnet restore
dotnet run --project OrderSystem.API
```

### 3. Frontend

```bash
cd apps/frontend
npm install
npm run dev
```

A aplicação estará disponível em: `http://localhost:3000`

### 4. Desenvolvimento Simultâneo (Backend + Frontend)

Para rodar ambos os serviços simultaneamente em modo de desenvolvimento, execute na raiz do projeto:

```bash
npm run dev
```

Este comando utiliza o `concurrently` para iniciar:
- Backend (ASP.NET Core) na porta **5012**
- Frontend (Next.js) na porta **3000**

Ambos os serviços terão hot-reload habilitado para uma melhor experiência de desenvolvimento.

---

## 🔧 Configuração do Banco de Dados

O banco de dados PostgreSQL é configurado via Docker Compose. As migrations são aplicadas automaticamente na inicialização.

### Variáveis de Ambiente

#### Backend
Crie um arquivo `OrderSystem.API/appsettings.json` ou use variáveis de ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=OrderDb;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-aqui",
    "Issuer": "OrderSystem",
    "Audience": "OrderSystemClient"
  }
}
```

#### Frontend
No `apps/frontend/config/api.ts`, configure a URL da API se necessário.

---

## 📋 Funcionalidades

### Usuário
- Registro e autenticação
- Gerenciamento de perfil
- Endereços de entrega (máximo 5 por usuário)
- Histórico de pedidos

### Produtos
- Listagem de produtos
- Busca por nome/categoria
- Imagens dos produtos (upload via LocalStorageService)

### Pedidos
- Carrinho de compras
- Checkout com seleção de endereço e pagamento
- Acompanhamento de status (Pendente, Processando, Enviado, Entregue, Cancelado)
- Detalhes do pedido
- Código do pedido (OrderCode)

### Pagamento
- Cartão de crédito
- PIX
- Dinheiro na entrega

### Admin
- Gestão de produtos
- Gestão de usuários
- Gestão de pedidos

---

## 🧪 Testes

```bash
# Rodar todos os testes
cd apps/backend
dotnet test

# Rodar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📖 API Endpoints

A documentação completa da API estará disponível em `/swagger` quando o backend estiver rodando:

```
GET    /api/auth/register          # Registro de usuário
POST   /api/auth/login             # Login
GET    /api/auth/profile           # Perfil do usuário
PUT    /api/auth/profile           # Atualizar perfil

GET    /api/products               # Listar produtos
GET    /api/products/{id}          # Detalhe do produto

GET    /api/addresses              # Listar endereços
POST   /api/addresses              # Criar endereço
PUT    /api/addresses/{id}         # Atualizar endereço
DELETE /api/addresses/{id}         # Remover endereço

GET    /api/orders                 # Listar pedidos do usuário
POST   /api/orders                 # Criar pedido
GET    /api/orders/{id}            # Detalhe do pedido

GET    /api/cart                   # Carrinho
POST   /api/cart/items             # Adicionar item
PUT    /api/cart/items/{id}        # Atualizar quantidade
DELETE /api/cart/items/{id}        # Remover item
DELETE /api/cart                   # Limpar carrinho
```

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📄 Licença

[MIT](https://choosealicense.com/licenses/mit/)

---

## 👨‍💻 Autor

Feito com ❤️ por Nataniel
