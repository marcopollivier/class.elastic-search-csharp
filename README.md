# Elasticsearch POC com .NET 9

Este projeto demonstra os conceitos básicos do Elasticsearch usando .NET 9 e C#, incluindo uma arquitetura com instâncias separadas para dados da aplicação e logs.

## Arquitetura

### Instâncias Elasticsearch:
- **Dados da Aplicação**: `localhost:9200` - Índice `products`
- **Logs da Aplicação**: `localhost:9201` - Índice `application-logs`

### Interfaces Kibana:
- **Dados**: `localhost:5601` - Visualização dos produtos
- **Logs**: `localhost:5602` - Monitoramento de logs da aplicação

## Conceitos Demonstrados

### 1. **Indexação de Documentos**
- Criação de índices com mapeamento de campos
- Inserção de documentos no Elasticsearch

### 2. **Busca por ID**
- Recuperação direta de documentos usando identificador único

### 3. **Busca Textual (Full-Text Search)**
- Busca em múltiplos campos usando `multi_match`
- Análise de texto com analyzer padrão

### 4. **Operações CRUD**
- Create: Indexação de novos documentos
- Read: Busca por ID e queries de pesquisa
- Update: Reindexação de documentos
- Delete: Remoção de documentos

### 5. **Logging Centralizado**
- Logs estruturados enviados para Elasticsearch separado
- Rastreamento de operações da aplicação
- Monitoramento de erros e eventos

## Estrutura do Projeto

```
ElasticSearchPOC/
├── Models/
│   └── Product.cs              # Modelo de dados
├── Services/
│   ├── ElasticSearchService.cs # Serviço com operações ES
│   └── LoggingService.cs       # Serviço de logging
└── Program.cs                  # Demonstração das funcionalidades
```

## Como Executar

### 1. Iniciar Elasticsearch e Kibana
```bash
docker-compose up -d
```

### 2. Carregar Dados de Exemplo (Opcional)
```bash
./load-sample-data.sh
```

### 3. Executar o Projeto .NET
```bash
cd ElasticSearchPOC
dotnet run
```

## Visualização no Kibana

### Acessos:
- **Elasticsearch API (Dados)**: `http://localhost:9200`
- **Elasticsearch API (Logs)**: `http://localhost:9201`
- **Kibana Interface (Dados)**: `http://localhost:5601`
- **Kibana Interface (Logs)**: `http://localhost:5602`

### Como usar o Discover (Exploração de Dados):

#### 1. **Criar Index Pattern - Dados**
- Acesse: `http://localhost:5601`
- Vá em **Stack Management** → **Index Patterns**
- Clique **Create index pattern**
- Digite: `products*`
- Selecione **createdAt** como time field
- Clique **Create index pattern**

#### 2. **Criar Index Pattern - Logs**
- Acesse: `http://localhost:5602`
- Vá em **Stack Management** → **Index Patterns**
- Clique **Create index pattern**
- Digite: `application-logs*`
- Selecione **timestamp** como time field
- Clique **Create index pattern**

#### 3. **Explorar no Discover**
- Menu lateral → **Discover**
- Agora você pode fazer consultas visuais:

**Exemplos de Consultas (Dados):**
```
# Filtrar por categoria
category:"Eletrônicos"
category:"Periféricos"

# Buscar por nome
name:"Notebook"
name:*Mouse*

# Filtrar por preço
price:[300 TO 500]    # Entre R$ 300-500
price:>1000           # Acima de R$ 1000

# Combinar filtros
category:"Periféricos" AND price:<400
```

**Exemplos de Consultas (Logs):**
```
# Filtrar por nível
level:"ERROR"
level:"INFO"

# Buscar por fonte
source:"Program"
source:"ElasticService"

# Filtrar por mensagem
message:*produto*
message:*erro*

# Combinar filtros
level:"ERROR" AND source:"Program"
```

#### 4. **Usar Filtros Visuais**
- Clique no **+** ao lado de qualquer campo nos resultados
- Isso cria filtros automáticos
- Clique em **Add field** para mostrar campos específicos

### Como criar Dashboards:

#### 1. **Criar Dashboard**
- Menu lateral → **Dashboard**
- Clique **Create dashboard**
- Clique **Create visualization**

#### 2. **Visualizações Sugeridas:**

**A) Gráfico de Pizza - Produtos por Categoria:**
- Tipo: **Pie**
- Buckets → Add → Split slices
- Aggregation: **Terms**
- Field: **category.keyword**

**B) Gráfico de Barras - Preços por Produto:**
- Tipo: **Vertical bar**
- Y-axis → Aggregation: **Max**, Field: **price**
- X-axis → Buckets → Add → X-axis
- Aggregation: **Terms**, Field: **name.keyword**

**C) Métrica - Valor Total:**
- Tipo: **Metric**
- Aggregation: **Sum**, Field: **price**

**D) Logs por Nível (Kibana Logs):**
- Tipo: **Pie**
- Buckets → Add → Split slices
- Aggregation: **Terms**
- Field: **level.keyword**

**E) Timeline de Logs:**
- Tipo: **Line**
- Y-axis → Aggregation: **Count**
- X-axis → Aggregation: **Date Histogram**, Field: **timestamp**

## Arquitetura e Boas Práticas

### **Separação de Responsabilidades**
- `Models`: Entidades de domínio
- `Services`: Lógica de acesso ao Elasticsearch
- `Program`: Orquestração e demonstração

### **Separação de Dados**
- **Instância Principal**: Dados de negócio (produtos)
- **Instância de Logs**: Logs da aplicação e monitoramento

### **Configuração de Mapeamento**
```csharp
// Campos keyword para busca exata
id = new { type = "keyword" }
category = new { type = "keyword" }

// Campos text para busca textual
name = new { type = "text", analyzer = "standard" }
description = new { type = "text", analyzer = "standard" }
```

### **Padrões de Busca**
- **Busca Exata**: Usando campos `keyword`
- **Busca Textual**: Usando campos `text` com analyzers
- **Multi-Match**: Busca em múltiplos campos simultaneamente

### **Logging Estruturado**
- Logs com timestamp, nível, mensagem e fonte
- Separação entre logs de aplicação e dados de negócio
- Rastreamento de operações e erros

## Conceitos Avançados para Explorar

1. **Aggregations**: Análise e sumarização de dados
2. **Filtering**: Combinação de queries e filtros
3. **Scoring**: Relevância e ordenação de resultados
4. **Bulk Operations**: Operações em lote para performance
5. **Index Templates**: Configuração automática de índices
6. **Log Analysis**: Análise de padrões em logs
7. **Alerting**: Configuração de alertas baseados em logs
