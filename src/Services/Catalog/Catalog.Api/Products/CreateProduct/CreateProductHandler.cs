
namespace Catalog.Api.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                Price = command.Price,
                ImageFile = command.ImageFile
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }
    }
}

/*
docker ps
docker exec -it  e30fe225b154 bash
psql -U postgres -d CatalogDb
SELECT * FROM mt_doc_product;
*/