# ClimaApi
Projeto acadêmico desenvolvido para fins educacionais sobre o uso de Api

# 🌤️ ClimaApi - API de Consulta de Clima

Uma API robusta para consultar dados de clima em tempo real para diferentes cidades brasileiras. O projeto utiliza **ASP.NET Core** como backend principal e consome a API pública **Open-Meteo** para obter informações climáticas precisas.

---

## 📋 Descrição

**ClimaApi** é um serviço REST que fornece informações de clima (temperatura atual, velocidade do vento e outros dados meteorológicos) para qualquer cidade. A API valida os nomes das cidades, busca suas coordenadas geográficas e recupera dados climáticos em tempo real.

---

## 🎯 Pontos Importantes

### **Backend (C# - ASP.NET Core)**

✅ **Arquitetura em Camadas**
- **Controllers**: Camada de apresentação com validação de requisições
- **Services**: Lógica de negócio (busca de cidades e dados climáticos)
- **Models**: Estruturas de dados (ClimaResponse, Cidade, etc.)

✅ **Validações Robustas**
- Validação de nome da cidade (mínimo 2 caracteres)
- Tratamento de cidades não encontradas (HTTP 404)
- Tratamento de serviços externos indisponíveis (HTTP 503)

✅ **Integração com APIs Externas**
- Consome **Open-Meteo API** para dados meteorológicos reais
- Busca coordenadas geográficas (latitude/longitude) das cidades
- Geração de respostas com dados atualizados

✅ **Endpoints RESTful**
- Usa versionamento de API (`/api/v1/...`)
- Padrão consistente de resposta (JSON)
- Timestamps UTC para auditoria

✅ **.NET 10.0**
- Última versão do .NET com Nullable reference types habilitado
- Suporte a Implicit Usings para código mais limpo

### **Frontend Alternativo (Node.js - Express)**

⚠️ **Servidor simulado em Node.js** (`index.js`)
- Fornece endpoints de teste para desenvolvimento
- Health check em `/api/v1/health`
- Dados simulados de clima para testes
- Porta: 3000 (mesma que o C#)

---

## 🏗️ Estrutura do Projeto

```
ClimaApi/
├── Program.cs                 # Configuração principal da aplicação
├── ClimaApi.csproj            # Definição do projeto .NET
├── ClimaApi.http              # Requisições HTTP para testes (VS Code)
├── appsettings.json           # Configurações de produção
├── appsettings.Development.json # Configurações de desenvolvimento
│
├── src/
│   ├── Controllers/
│   │   ├── ClimaController.cs      # Endpoint principal de clima
│   │   ├── CidadeController.cs     # Endpoint de cidades
│   │   └── HealthController.cs     # Health check
│   │
│   ├── Services/
│   │   ├── ClimaService.cs         # Busca dados da Open-Meteo API
│   │   └── CidadeService.cs        # Busca dados de cidades
│   │
│   └── Models/
│       ├── ClimaResponse.cs        # Resposta de clima
│       └── Cidade.cs               # Dados de cidade
│
├── Properties/                # Configurações do projeto
├── index.js                   # Servidor Node.js alternativo (teste)
├── package.json               # Dependências Node.js
│
└── bin/, obj/                 # Artefatos de build (ignorar)
```

---

## 🚀 Como Usar

### **Pré-requisitos**
- .NET 10.0 SDK instalado
- Visual Studio Code ou Visual Studio 2022+
- Node.js 18+ (opcional, apenas se usar o servidor Express)

### **Executar a API C#**

1. **Restaurar dependências:**
   ```bash
   dotnet restore
   ```

2. **Executar em desenvolvimento:**
   ```bash
   dotnet run
   ```

3. **Acessar via browser ou ferramenta HTTP:**
   - API roda em: `http://localhost:3000`
   - Swagger/OpenAPI: `http://localhost:3000/openapi/v1.json` (em desenvolvimento)

### **Executar o Servidor Node.js (opcional)**
```bash
npm install
npm start  # ou: node index.js
```

---

## 📡 Endpoints da API

### **1. Buscar Clima de uma Cidade**
```http
GET /api/v1/clima/{nome}
```

**Exemplo:**
```bash
curl "http://localhost:3000/api/v1/clima/São%20Paulo"
```

**Resposta de Sucesso (200):**
```json
{
  "nome": "São Paulo",
  "estado": "SP",
  "clima": {
    "temperature": 28.5,
    "windspeed": 12.3
  },
  "consultado_em": "2024-01-15T10:30:00Z"
}
```

**Erros Possíveis:**

| Código HTTP | Erro | Causa |
|---|---|---|
| 400 | `NOME_INVALIDO` | Nome da cidade vazio ou < 2 caracteres |
| 404 | `CIDADE_NAO_ENCONTRADA` | Cidade não existe no banco de dados |
| 503 | `SERVICO_EXTERNO_INDISPONIVEL` | Open-Meteo API indisponível |

### **2. Health Check** (apenas Node.js)
```http
GET /api/v1/health
```

**Resposta:**
```json
{ "status": "OK" }
```

---

## 🔧 Configuração

### **Program.cs - Configurações Principais**

```csharp
// ✅ Adiciona suporte a controllers REST
builder.Services.AddControllers();

// ✅ HttpClient para chamar Open-Meteo API
builder.Services.AddHttpClient();

// ✅ Injeção de dependência dos serviços
builder.Services.AddScoped<CidadeService>();
builder.Services.AddScoped<ClimaService>();

// ✅ Swagger/OpenAPI em desenvolvimento
app.MapOpenApi();

// ✅ Mapeia controllers automaticamente
app.MapControllers();

// ✅ Porta: 3000
app.Run("http://localhost:3000");
```

---

## 📊 Fluxo de Requisição

```
1. Cliente → GET /api/v1/clima/{cidade}
   ↓
2. ClimaController → Valida nome da cidade
   ↓
3. CidadeService → Busca coordenadas (lat, lon)
   ↓
4. ClimaService → Chama Open-Meteo API com coordenadas
   ↓
5. Open-Meteo API → Retorna dados meteorológicos
   ↓
6. ClimaController → Formata resposta JSON
   ↓
7. Cliente ← Clima atualizado com timestamp
```

---

## 🔗 APIs Externas Utilizadas

### **Open-Meteo API** (Gratuita)
- **URL Base:** `https://api.open-meteo.com/v1/forecast`
- **Parâmetros:** `latitude`, `longitude`, `current_weather=true`
- **Sem autenticação necessária**
- **Dados retornados:**
  - `temperature`: Temperatura em °C
  - `windspeed`: Velocidade do vento em km/h
  - Muitos outros dados disponíveis

---

## 📝 Exemplos de Uso

### **JavaScript/Fetch**
```javascript
const cidade = "Rio de Janeiro";
const response = await fetch(`http://localhost:3000/api/v1/clima/${cidade}`);
const dados = await response.json();
console.log(`Temperatura em ${dados.nome}: ${dados.clima.temperature}°C`);
```

### **Python/Requests**
```python
import requests

url = "http://localhost:3000/api/v1/clima/Brasília"
resp = requests.get(url)
clima = resp.json()
print(f"Clima em {clima['nome']}: {clima['clima']['temperature']}°C")
```

### **cURL**
```bash
curl -X GET "http://localhost:3000/api/v1/clima/Salvador"
```

---

## ⚙️ Variáveis de Ambiente

Configurações em `appsettings.json` e `appsettings.Development.json`:
- Porta padrão: `3000`
- HTTPS redirecionamento habilitado
- OpenAPI disponível em desenvolvimento

---

## 🐛 Tratamento de Erros

A API segue padrão RESTful com status codes apropriados:

- **200 OK**: Dados retornados com sucesso
- **400 Bad Request**: Entrada inválida
- **404 Not Found**: Cidade não encontrada
- **503 Service Unavailable**: API externa indisponível

Todas as respostas de erro incluem um objeto com:
```json
{
  "erro": true,
  "codigo": "TIPO_DE_ERRO"
}
```

---

## 📦 Dependências

### **C# (.NET 10.0)**
- `Microsoft.AspNetCore.OpenApi` (v10.0.8) - Swagger/OpenAPI

### **Node.js**
- `express` (v5.2.1) - Framework web (opcional)

---

## 🚦 Status do Projeto

- ✅ API funcional
- ✅ Validações implementadas
- ✅ Integração com Open-Meteo
- ⚠️ Servidor Node.js com dados simulados (teste)
- 📋 Possível expansão futura com banco de dados de cidades

---

## 📚 Recursos Úteis

- [Documentação ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/)
- [Open-Meteo API Docs](https://open-meteo.com/en/docs)
- [REST API Best Practices](https://restfulapi.net/)

---

## 👨‍💻 Autor

Desenvolvido como exemplo de API REST com ASP.NET Core e integração com serviços externos.

---

**Última atualização:** 29 de maio de 2026
