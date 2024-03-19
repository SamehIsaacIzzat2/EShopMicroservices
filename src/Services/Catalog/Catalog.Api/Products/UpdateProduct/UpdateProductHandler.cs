
namespace Catalog.Api.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class CreateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required!");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required!")
                .Length(2,100).WithMessage("Name must be from 2 to 100 char!");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero!");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var existingProduct = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            existingProduct.Name= command.Name;
            existingProduct.Category= command.Category;
            existingProduct.Description= command.Description;
            existingProduct.ImageFile= command.ImageFile;
            existingProduct.Price= command.Price;

            session.Update(existingProduct);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}

/*
docker ps
docker exec -it  e30fe225b154 bash
psql -U postgres -d CatalogDb
SELECT * FROM mt_doc_product;
*/