
using Catalog.Api.Products.CreateProduct;

namespace Catalog.Api.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException();
            }

            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}

/*
docker ps
docker exec -it  e30fe225b154 bash
psql -U postgres -d CatalogDb
SELECT * FROM mt_doc_product;
*/