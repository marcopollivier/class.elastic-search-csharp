# Elasticsearch POC com .NET 9

Este projeto demonstra os conceitos básicos do Elasticsearch usando .NET 9 e C#.

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

## Estrutura do Projeto

```
ElasticSearchPOC/
├── Models/
│   └── Product.cs          # Modelo de dados
├── Services/
│   └── ElasticSearchService.cs  # Serviço com operações ES
└── Program.cs              # Demonstração das funcionalidades
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
- **Elasticsearch API**: `http://localhost:9200`
- **Kibana Interface**: `http://localhost:5601`

### Como usar o Discover (Exploração de Dados):

#### 1. **Criar Index Pattern**
- Acesse: `http://localhost:5601`
- Vá em **Stack Management** → **Index Patterns**
- Clique **Create index pattern**
- Digite: `products*`
- Selecione **createdAt** como time field
- Clique **Create index pattern**

#### 2. **Explorar no Discover**
- Menu lateral → **Discover**
- Agora você pode fazer consultas visuais:

**Exemplos de Consultas:**
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

#### 3. **Usar Filtros Visuais**
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

## Arquitetura e Boas Práticas

### **Separação de Responsabilidades**
- `Models`: Entidades de domínio
- `Services`: Lógica de acesso ao Elasticsearch
- `Program`: Orquestração e demonstração

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

## Conceitos Avançados para Explorar

1. **Aggregations**: Análise e sumarização de dados
2. **Filtering**: Combinação de queries e filtros
3. **Scoring**: Relevância e ordenação de resultados
4. **Bulk Operations**: Operações em lote para performance
5. **Index Templates**: Configuração automática de índices
