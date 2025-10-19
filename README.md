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

### 1. Iniciar Elasticsearch
```bash
docker run -d --name elasticsearch \
  -p 9200:9200 \
  -e "discovery.type=single-node" \
  -e "xpack.security.enabled=false" \
  docker.elastic.co/elasticsearch/elasticsearch:8.11.0
```

### 2. Executar o Projeto
```bash
cd ElasticSearchPOC
dotnet run
```

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
