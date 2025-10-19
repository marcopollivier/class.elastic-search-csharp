using ElasticSearchPOC.Models;
using ElasticSearchPOC.Services;

Console.WriteLine("=== Elasticsearch POC com .NET 9 ===\n");

var elasticService = new ElasticSearchService();

try
{
    // 1. Indexar produtos (o índice será criado automaticamente)
    Console.WriteLine("1. Indexando produtos...");
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
        Console.WriteLine($"✓ Produto indexado: {product.Name}");
    }
    Console.WriteLine();

    // Aguardar indexação
    await Task.Delay(2000);

    // 2. Buscar produto por ID
    Console.WriteLine("2. Buscando produto por ID (1)...");
    var foundProduct = await elasticService.GetProductAsync("1");
    if (foundProduct != null)
    {
        Console.WriteLine($"✓ Produto encontrado: {foundProduct.Name} - R$ {foundProduct.Price:F2}");
    }
    Console.WriteLine();

    // 3. Busca textual
    Console.WriteLine("3. Busca textual por 'notebook'...");
    var searchResults = await elasticService.SearchProductsAsync("notebook");
    Console.WriteLine($"✓ Encontrados {searchResults.Count} produtos:");
    foreach (var product in searchResults)
    {
        Console.WriteLine($"  - {product.Name}: {product.Description}");
    }
    Console.WriteLine();

    // 4. Busca por categoria
    Console.WriteLine("4. Busca por 'periféricos'...");
    var categoryResults = await elasticService.SearchProductsAsync("periféricos");
    Console.WriteLine($"✓ Encontrados {categoryResults.Count} produtos:");
    foreach (var product in categoryResults)
    {
        Console.WriteLine($"  - {product.Name}: R$ {product.Price:F2}");
    }
    Console.WriteLine();

    // 5. Deletar produto
    Console.WriteLine("5. Deletando produto ID 2...");
    var deleted = await elasticService.DeleteProductAsync("2");
    if (deleted)
    {
        Console.WriteLine("✓ Produto deletado com sucesso");
    }
    Console.WriteLine();

    Console.WriteLine("=== Demonstração concluída ===");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro: {ex.Message}");
    Console.WriteLine("\n⚠️  Certifique-se de que o Elasticsearch está rodando em http://localhost:9200");
}
