# EduPlatform - Plataforma Educacional em C# ASP.NET Core

Uma API REST completa para plataforma educacional desenvolvida em C# com ASP.NET Core 8.0 e integração com Supabase.

## Tecnologias Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Supabase** - Backend como serviço (banco de dados PostgreSQL)
- **JWT Authentication** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação da API

## Estrutura do Projeto

```
EduPlatform.API/
├── Controllers/          # Controladores da API
│   ├── AuthController.cs
│   ├── CoursesController.cs
│   ├── EnrollmentsController.cs
│   └── ProfileController.cs
├── Services/            # Camada de serviços
│   ├── AuthService.cs
│   ├── CourseService.cs
│   ├── EnrollmentService.cs
│   └── ProfileService.cs
├── Models/              # Modelos de dados
│   ├── Profile.cs
│   ├── Course.cs
│   ├── Lesson.cs
│   └── Enrollment.cs
├── DTOs/                # Data Transfer Objects
│   ├── AuthDtos.cs
│   └── CourseDtos.cs
└── Program.cs           # Configuração da aplicação
```

## Funcionalidades

### Autenticação
- Registro de usuários
- Login com email/senha
- Gerenciamento de perfis

### Cursos
- Listagem de cursos disponíveis
- Visualização detalhada de cursos
- Criação e edição de cursos (instrutores)
- Gerenciamento de aulas

### Inscrições
- Inscrição em cursos
- Acompanhamento de progresso
- Marcação de aulas como concluídas
- Dashboard do estudante

## Endpoints da API

### Autenticação
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Fazer login
- `GET /api/auth/profile/{userId}` - Obter perfil do usuário

### Cursos
- `GET /api/courses` - Listar todos os cursos
- `GET /api/courses/{id}` - Obter detalhes do curso
- `POST /api/courses` - Criar novo curso (requer autenticação)
- `PUT /api/courses/{id}` - Atualizar curso (requer autenticação)
- `DELETE /api/courses/{id}` - Deletar curso (requer autenticação)
- `GET /api/courses/{id}/lessons` - Listar aulas do curso
- `POST /api/courses/{id}/lessons` - Criar nova aula (requer autenticação)

### Inscrições
- `POST /api/enrollments` - Inscrever-se em um curso (requer autenticação)
- `GET /api/enrollments` - Listar minhas inscrições (requer autenticação)
- `PUT /api/enrollments/{enrollmentId}/lessons/{lessonId}` - Atualizar progresso da aula (requer autenticação)

### Perfil
- `GET /api/profile` - Obter meu perfil (requer autenticação)
- `PUT /api/profile` - Atualizar meu perfil (requer autenticação)

## Pré-requisitos

- .NET SDK 8.0 ou superior
- Conta no Supabase (já configurada no projeto)

## Como Executar

1. **Restaurar os pacotes NuGet:**
   ```bash
   cd backend-csharp/EduPlatform.API
   dotnet restore
   ```

2. **Configurar as variáveis de ambiente:**
   - O arquivo `appsettings.json` já está configurado com as credenciais do Supabase
   - Para produção, use variáveis de ambiente ou Azure Key Vault

3. **Executar a aplicação:**
   ```bash
   dotnet run
   ```

4. **Acessar a documentação Swagger:**
   - Navegue para `http://localhost:5000/swagger`
   - Ou `https://localhost:5001/swagger`

## Configuração do Supabase

O banco de dados Supabase já está configurado com as seguintes tabelas:

- `profiles` - Perfis dos usuários
- `courses` - Cursos disponíveis
- `lessons` - Aulas dos cursos
- `enrollments` - Inscrições dos estudantes
- `lesson_progress` - Progresso nas aulas

Todas as tabelas possuem Row Level Security (RLS) configurado para garantir a segurança dos dados.

## Segurança

- Autenticação JWT implementada
- Row Level Security (RLS) no Supabase
- Validação de dados nos DTOs
- Autorização baseada em roles
- CORS configurado para permitir apenas origens confiáveis

## Integração com Frontend

O frontend React já existente pode ser facilmente integrado com esta API:

1. Configure a URL base da API no frontend
2. Use os endpoints documentados no Swagger
3. Inclua o token JWT nos headers das requisições autenticadas

Exemplo de requisição autenticada:
```javascript
const response = await fetch('http://localhost:5000/api/courses', {
  headers: {
    'Authorization': `Bearer ${accessToken}`,
    'Content-Type': 'application/json'
  }
});
```

## Desenvolvimento

### Adicionar Novo Endpoint

1. Criar o DTO em `DTOs/`
2. Adicionar método no service correspondente
3. Implementar o endpoint no controller
4. Testar usando Swagger

### Boas Práticas

- Sempre use DTOs para entrada/saída de dados
- Implemente validação de dados
- Use async/await para operações I/O
- Trate exceções adequadamente
- Mantenha a camada de serviços separada dos controllers

## Deploy

Para deploy em produção:

1. Configure as variáveis de ambiente adequadamente
2. Use HTTPS
3. Configure CORS para domínios específicos
4. Ative logging apropriado
5. Use Azure App Service, AWS, ou outro provedor de hospedagem

## Licença

Este projeto é parte de um sistema de plataforma educacional.
