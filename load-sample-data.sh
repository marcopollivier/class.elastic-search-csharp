#!/bin/bash

echo "🔄 Carregando dados de exemplo no Elasticsearch..."

# Criar índice com mapeamento
echo "📋 Criando índice 'products'..."
curl -X PUT "localhost:9200/products" -H 'Content-Type: application/json' -d'
{
  "mappings": {
    "properties": {
      "id": { "type": "keyword" },
      "name": { "type": "text" },
      "description": { "type": "text" },
      "price": { "type": "double" },
      "category": { "type": "keyword" },
      "createdAt": { "type": "date" }
    }
  }
}'

echo -e "\n\n📦 Inserindo produtos..."

# Produto 1
curl -X POST "localhost:9200/products/_doc/1" -H 'Content-Type: application/json' -d'
{
  "id": "1",
  "name": "Notebook Dell Inspiron",
  "description": "Notebook para desenvolvimento com 16GB RAM",
  "price": 2500.00,
  "category": "Eletrônicos",
  "createdAt": "2024-10-19T14:30:00Z"
}'

# Produto 2
curl -X POST "localhost:9200/products/_doc/2" -H 'Content-Type: application/json' -d'
{
  "id": "2",
  "name": "Mouse Logitech MX Master",
  "description": "Mouse ergonômico sem fio para produtividade",
  "price": 350.00,
  "category": "Periféricos",
  "createdAt": "2024-10-19T14:31:00Z"
}'

# Produto 3
curl -X POST "localhost:9200/products/_doc/3" -H 'Content-Type: application/json' -d'
{
  "id": "3",
  "name": "Teclado Mecânico Keychron",
  "description": "Teclado mecânico RGB com switches blue",
  "price": 450.00,
  "category": "Periféricos",
  "createdAt": "2024-10-19T14:32:00Z"
}'

# Produto 4
curl -X POST "localhost:9200/products/_doc/4" -H 'Content-Type: application/json' -d'
{
  "id": "4",
  "name": "Monitor LG UltraWide 34",
  "description": "Monitor 34 polegadas ultrawide 4K",
  "price": 1800.00,
  "category": "Eletrônicos",
  "createdAt": "2024-10-19T14:33:00Z"
}'

# Produto 5
curl -X POST "localhost:9200/products/_doc/5" -H 'Content-Type: application/json' -d'
{
  "id": "5",
  "name": "Headset Gamer HyperX",
  "description": "Headset gamer com microfone destacável",
  "price": 280.00,
  "category": "Periféricos",
  "createdAt": "2024-10-19T14:34:00Z"
}'

echo -e "\n\n✅ Carga concluída! Verificando dados..."

# Verificar dados
curl -X GET "localhost:9200/products/_search?pretty" | head -20

echo -e "\n\n🎯 Acesse o Kibana em: http://localhost:5601"
echo "📊 Para explorar os dados, vá em Discover após criar o Index Pattern 'products*'"
