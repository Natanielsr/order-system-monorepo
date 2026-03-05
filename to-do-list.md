# 📋 Lista de Tarefas - Projeto OrderSystem

## 🎯 Prioridade Alta

### Segurança e Autenticação
- [ ] **BACKEND**: Implementar rate limiting nas tentativas de login
  - Limitar tentativas por IP e por usuário
  - Configurar tempo de bloqueio após tentativas falhas
  
- [ ] **BACKEND & FRONTEND**: Adicionar CAPTCHA no login e cadastro
  - Integrar com serviço como Google reCAPTCHA ou hCaptcha
  - Proteger formulários contra bots e ataques automatizados

- [ ] **BACKEND & FRONTEND**: Sistema de confirmação de e-mail
  - Enviar e-mail de verificação após cadastro
  - Bloquear funcionalidades até confirmação
  - Permitir reenvio do e-mail de confirmação

- [ ] **BACKEND & FRONTEND**: Recuperação de senha
  - Fluxo completo de "Esqueci minha senha"
  - Gerar token temporário com expiração
  - Enviar link seguro por e-mail
  - Página para criar nova senha

### Testes e Qualidade
- [ ] **BACKEND**: Completar cobertura de testes
  - Testes unitários para serviços e utilitários
  - Testes de integração para APIs e banco de dados
  - Testes de fluxos críticos (autenticação, pagamento)
  - Mínimo de 80% de cobertura

## 🎯 Prioridade Média

### Funcionalidades Core
- [ ] **BACKEND**: Serviço de pagamento
  - Integrar com gateway de pagamento (Stripe/PagSeguro/MercadoPago)
  - Implementar webhooks para confirmação de pagamento
  - Gerenciar status de pagamento no pedido

- [ ] **BACKEND**: Melhorias no modelo de pedidos (Order)
  - Adicionar campos de endereço de entrega
  - Incluir informações completas do cliente
  - Histórico de status do pedido
  - Rastreamento de entrega

- [ ] **BACKEND**: Serviço de notificação por e-mail
  - Sistema de filas para envio assíncrono
  - Templates de e-mail para diferentes eventos
  - Logs de envio e tratamento de falhas

- [ ] **FRONTEND**: Página de detalhes do produto
  - Imagens em galeria com zoom
  - Especificações técnicas
  - Avaliações e comentários
  - Produtos relacionados
  - Botões de compra e carrinho

## 🎯 Prioridade Baixa

### Melhorias e Experiência do Usuário
- [ ] **FRONTEND**: Mensagem de logout
  - Toast ou notificação confirmando saída
  - Redirecionamento suave para home/login

- [ ] **FRONTEND**: Melhorias na página inicial
  - Inspiração em Amazon e Mercado Livre
  - Banner rotativo de promoções
  - Categorias em destaque
  - Produtos recomendados
  - Ofertas do dia
  - Busca inteligente com autocomplete
  - Design responsivo e moderno

### Administração e Conteúdo
- [ ] **BACKEND & FRONTEND**: Área de gerenciamento de produtos
  - Acesso restrito para perfis Manager e Admin
  - CRUD completo de produtos
  - Upload de imagens
  - Gestão de estoque
  - Categorização e tags
  - Histórico de alterações

## 🚀 Infraestrutura e DevOps

### Containerização e Deploy
- [ ] **INFRA**: Configurar containers separados
  - Container para Frontend (React/Next.js/Vue)
  - Container para Backend (Node.js/Python/Java)
  - Container para Banco de Dados (PostgreSQL/MySQL)
  - Rede interna entre containers
  - Volumes para persistência de dados

- [ ] **INFRA**: Script de deploy em produção
  - Automatizar build das imagens
  - Configurar variáveis de ambiente
  - Backup automático do banco de dados
  - Monitoramento básico

- [ ] **INFRA**: Pipeline de CI/CD
  - Integração com GitHub/GitLab Actions
  - Rodar testes automaticamente
  - Build e deploy automatizado
  - Rollback automático em caso de falha

## 📊 Progresso

| Módulo | Status | Observações |
|--------|--------|-------------|
| Segurança | 🟡 Em andamento | Foco inicial |
| Testes | 🟡 Em andamento | - |
| Pagamentos | 🔴 Não iniciado | - |
| Frontend | 🟡 Em andamento | Home e detalhes |
| Backend | 🟡 Em andamento | APIs básicas prontas |
| Infraestrutura | 🔴 Não iniciado | - |

## 📝 Notas e Observações
- Priorizar itens de segurança e autenticação
- Manter padrões de código e boas práticas
- Documentar decisões técnicas importantes
- Revisar e atualizar prioridades quinzenalmente

---
**Última atualização:** [05/03/2026 13:32]
**Responsável:** [Nataniel]