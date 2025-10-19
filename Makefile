.PHONY: help up down status logs run run-container build test-data test-logs clean

help: ## Mostra os comandos disponíveis
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-15s\033[0m %s\n", $$1, $$2}'

up: ## Sobe as instâncias do Elasticsearch e Kibana
	docker compose up -d elasticsearch elasticsearch-logs kibana

up-all: ## Sobe tudo incluindo aplicação .NET
	docker compose up -d

build: ## Builda a aplicação .NET
	docker compose build app

down: ## Para e remove os containers
	docker compose down

status: ## Verifica status dos containers
	@docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | grep -E "(elasticsearch|kibana|app)"

logs: ## Mostra logs dos containers
	docker compose logs -f

logs-app: ## Mostra logs apenas da aplicação
	docker compose logs -f app

run: ## Executa a aplicação .NET localmente
	cd ElasticSearchPOC && dotnet run

run-container: ## Executa a aplicação .NET no container
	docker compose up app

test-data: ## Testa conectividade com Elasticsearch de dados
	@echo "=== Elasticsearch Dados (9200) ==="
	@curl -s http://localhost:9200/_cluster/health | jq '.status'
	@echo "\n=== Produtos Indexados ==="
	@curl -s "http://localhost:9200/products/_search?size=3" | jq '.hits.hits[]._source | {name, price, category}'

test-logs: ## Testa conectividade com Elasticsearch de logs
	@echo "=== Elasticsearch Logs (9201) ==="
	@curl -s http://localhost:9201/_cluster/health | jq '.status'
	@echo "\n=== Logs Recentes ==="
	@curl -s "http://localhost:9201/application-logs/_search?size=5&sort=timestamp:desc" | jq '.hits.hits[]._source | {timestamp, level, message, source}'

load-sample: ## Carrega dados de exemplo
	./load-sample-data.sh

kibana: ## Abre Kibana no navegador
	open http://localhost:5601

clean: ## Remove volumes e containers
	docker compose down -v
	docker system prune -f

restart: down up ## Reinicia toda a aplicação

restart-all: down up-all ## Reinicia tudo incluindo app

full-test: up run test-data test-logs ## Sobe aplicação, executa e testa tudo

full-test-container: up-all test-data test-logs ## Testa tudo com app containerizada

setup-kibana: ## Configura index patterns no Kibana via API
	@echo "Aguardando Kibana inicializar..."
	@sleep 30
	@echo "Criando index pattern para produtos..."
	@curl -X POST "localhost:5601/api/saved_objects/index-pattern/products-pattern" \
		-H "kbn-xsrf: true" -H "Content-Type: application/json" \
		-d '{"attributes":{"title":"products*","timeFieldName":"createdAt"}}' || true
	@echo "\nCriando index pattern para logs..."
	@curl -X POST "localhost:5601/api/saved_objects/index-pattern/logs-pattern" \
		-H "kbn-xsrf: true" -H "Content-Type: application/json" \
		-d '{"attributes":{"title":"application-logs*","timeFieldName":"timestamp"}}' || true
	@echo "\nIndex patterns criados! Acesse: http://localhost:5601"
