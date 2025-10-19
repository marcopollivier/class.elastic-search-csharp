using ElasticSearchPOC.Models;
using ElasticSearchPOC.Services;

Console.WriteLine("=== Elasticsearch POC com .NET 9 ===\n");

var elasticService = new ElasticSearchService();
var loggingService = new LoggingService();

try
{
    await loggingService.LogInfoAsync("Aplicação iniciada", "Program");

    // 1. Indexar produtos (o índice será criado automaticamente)
    Console.WriteLine("1. Indexando produtos...");
    await loggingService.LogInfoAsync("Iniciando indexação de produtos", "Program");
    
    var products = new[]
    {
        new Product { Id = "1", Name = "Notebook Dell", Description = "Notebook para desenvolvimento", Price = 2500.00m, Category = "Eletrônicos" },
        new Product { Id = "2", Name = "Mouse Logitech", Description = "Mouse ergonômico sem fio", Price = 150.00m, Category = "Periféricos" },
        new Product { Id = "3", Name = "Teclado Mecânico", Description = "Teclado mecânico RGB", Price = 300.00m, Category = "Periféricos" },
        new Product { Id = "4", Name = "Monitor 4K", Description = "Monitor 27 polegadas 4K", Price = 1200.00m, Category = "Eletrônicos" }
    };

    foreach (var product in products)
    {
        await elasticService.IndexProductAsync(product);
        await loggingService.LogInfoAsync($"Produto indexado: {product.Name}", "ElasticService");
        Console.WriteLine($"✓ Produto indexado: {product.Name}");
    }
    Console.WriteLine();

    // Aguardar indexação
    await Task.Delay(2000);

    // 2. Buscar produto por ID
    Console.WriteLine("2. Buscando produto por ID (1)...");
    await loggingService.LogInfoAsync("Buscando produto por ID: 1", "Program");
    var foundProduct = await elasticService.GetProductAsync("1");
    if (foundProduct != null)
    {
        Console.WriteLine($"✓ Produto encontrado: {foundProduct.Name} - R$ {foundProduct.Price:F2}");
        await loggingService.LogInfoAsync($"Produto encontrado: {foundProduct.Name}", "ElasticService");
    }
    Console.WriteLine();

    // 3. Busca textual
    Console.WriteLine("3. Busca textual por 'notebook'...");
    await loggingService.LogInfoAsync("Executando busca textual: notebook", "Program");
    var searchResults = await elasticService.SearchProductsAsync("notebook");
    Console.WriteLine($"✓ Encontrados {searchResults.Count} produtos:");
    await loggingService.LogInfoAsync($"Busca textual retornou {searchResults.Count} resultados", "ElasticService");
    foreach (var product in searchResults)
    {
        Console.WriteLine($"  - {product.Name}: {product.Description}");
    }
    Console.WriteLine();

    // 4. Busca por categoria
    Console.WriteLine("4. Busca por 'periféricos'...");
    await loggingService.LogInfoAsync("Executando busca por categoria: periféricos", "Program");
    var categoryResults = await elasticService.SearchProductsAsync("periféricos");
    Console.WriteLine($"✓ Encontrados {categoryResults.Count} produtos:");
    await loggingService.LogInfoAsync($"Busca por categoria retornou {categoryResults.Count} resultados", "ElasticService");
    foreach (var product in categoryResults)
    {
        Console.WriteLine($"  - {product.Name}: R$ {product.Price:F2}");
    }
    Console.WriteLine();

    // 5. Deletar produto
    Console.WriteLine("5. Deletando produto ID 2...");
    await loggingService.LogInfoAsync("Deletando produto ID: 2", "Program");
    var deleted = await elasticService.DeleteProductAsync("2");
    if (deleted)
    {
        Console.WriteLine("✓ Produto deletado com sucesso");
        await loggingService.LogInfoAsync("Produto deletado com sucesso: ID 2", "ElasticService");
    }
    Console.WriteLine();

    await loggingService.LogInfoAsync("Demonstração concluída com sucesso", "Program");
    Console.WriteLine("=== Demonstração concluída ===");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro: {ex.Message}");
    await loggingService.LogErrorAsync(ex, "Program execution");
    Console.WriteLine("\n⚠️  Certifique-se de que o Elasticsearch está rodando em http://localhost:9200");
}
