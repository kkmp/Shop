using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shop.Data;
using Shop.Data.Models;
using Shop.Data.UnitOfWork;
using Shop.DTO.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject1
{
    public class Tests
    {
        private BaseApplicationFactory _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new BaseApplicationFactory();

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<DataContext>();
            context.Database.EnsureDeleted();
        }

        private static IEnumerable<TestCaseData> ProductCreateDtoSourceData
        {
            get
            {
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt 1",
                        Category = "Kategoria 1",
                        Description = "Przyk³adowy opis 1",
                        Photo = "Przyk³adowy url 1",
                        Price = 19.99F
                    }
                );
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt 2",
                        Category = "Kategoria 2",
                        Description = "Przyk³adowy opis 2",
                        Photo = "Przyk³adowy url 2",
                        Price = 119.99F
                    }
                );
                yield return new TestCaseData(
                     new ProductCreateDTO
                     {
                         Name = "Produkt 3",
                         Category = "Kategoria 3",
                         Description = "Przyk³adowy opis 3",
                         Photo = "Przyk³adowy url 3",
                         Price = 100.00F
                     }
                );
            }
        }

        private static IEnumerable<TestCaseData> ProductToAddSourceData
        {
            get
            {
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt 4",
                        Category = "Kategoria 4",
                        Description = "Przyk³adowy opis 4",
                        Photo = "Przyk³adowy url 4",
                        Price = 19.99F
                    }
                );
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt 5",
                        Category = "Kategoria 5",
                        Description = "Przyk³adowy opis 5",
                        Photo = "Przyk³adowy url 5",
                        Price = 119.99F
                    }
                );
                yield return new TestCaseData(
                     new ProductCreateDTO
                     {
                         Name = "Produkt 6",
                         Category = "Kategoria 6",
                         Description = "Przyk³adowy opis 6",
                         Photo = "Przyk³adowy url 6",
                         Price = 100.00F
                     }
                );
            }
        }

        private static IEnumerable<TestCaseData> ProductsToDeleteSourceData
        {
            get
            {
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt DELETE 1",
                        Category = "Kategoria DELETE 1",
                        Description = "Przyk³adowy opis DELETE 1",
                        Photo = "Przyk³adowy url DELETE 1",
                        Price = 19.99F
                    }
                );
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt DELETE 2",
                        Category = "Kategoria DELETE 2",
                        Description = "Przyk³adowy opis DELETE 2",
                        Photo = "Przyk³adowy url DELETE 2",
                        Price = 119.99F
                    }
                );
                yield return new TestCaseData(
                     new ProductCreateDTO
                     {
                         Name = "Produkt DELETE 3",
                         Category = "Kategoria DELETE 3",
                         Description = "Przyk³adowy opis DELETE 3",
                         Photo = "Przyk³adowy url DELETE 3",
                         Price = 100.00F
                     }
                );
            }
        }

        private static IEnumerable<TestCaseData> ProductsToUpdateSourceData
        {
            get
            {
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt UPDATE 1",
                        Category = "Kategoria UPDATE 1",
                        Description = "Przyk³adowy opis UPDATE 1",
                        Photo = "Przyk³adowy url UPDATE 1",
                        Price = 19.99F
                    },
                    new ProductUpdateDTO
                    {
                        Id = Guid.Empty,
                        Name = "Produkt UPDATE 1",
                        Category = "Kategoria UPDATED 1",
                        Description = "Przyk³adowy opis 1",
                        Photo = "Przyk³adowy url 1",
                        Price = 10.00F
                    }
                );
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt UPDATE 2",
                        Category = "Kategoria UPDATE 2",
                        Description = "Przyk³adowy opis UPDATE 2",
                        Photo = "Przyk³adowy url UPDATE 2",
                        Price = 119.99F
                    },
                    new ProductUpdateDTO
                    {
                        Id = Guid.Empty,
                        Name = "Produkt UPDATE 2",
                        Category = "Kategoria UPDATED 2",
                        Description = "Przyk³adowy opis 2",
                        Photo = "Przyk³adowy url 2",
                        Price = 34.45F
                    }
                );
                yield return new TestCaseData(
                     new ProductCreateDTO
                     {
                         Name = "Produkt UPDATE 3",
                         Category = "Kategoria UPDATE 3",
                         Description = "Przyk³adowy opis UPDATE 3",
                         Photo = "Przyk³adowy url UPDATE 3",
                         Price = 100.00F
                     },
                     new ProductUpdateDTO
                     {
                         Id = Guid.Empty,
                         Name = "Produkt UPDATE 3",
                         Category = "Kategoria UPDATED 3",
                         Description = "Przyk³adowy opis 3",
                         Photo = "Przyk³adowy url 3",
                         Price = 101.99F
                     }
                );
            }
        }

        private static IEnumerable<TestCaseData> ProductsToGetDetailsSourceData
        {
            get
            {
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt GET DETAILS 1",
                        Category = "Kategoria GET DETAILS 1",
                        Description = "Przyk³adowy opis GET DETAILS 1",
                        Photo = "Przyk³adowy url GET DETAILS 1",
                        Price = 19.99F
                    }
                );
                yield return new TestCaseData(
                    new ProductCreateDTO
                    {
                        Name = "Produkt GET DETAILS 2",
                        Category = "Kategoria GET DETAILS 2",
                        Description = "Przyk³adowy opis GET DETAILS 2",
                        Photo = "Przyk³adowy url GET DETAILS 2",
                        Price = 119.99F
                    }
                );
                yield return new TestCaseData(
                     new ProductCreateDTO
                     {
                         Name = "Produkt GET DETAILS 3",
                         Category = "Kategoria GET DETAILS 3",
                         Description = "Przyk³adowy opis GET DETAILS 3",
                         Photo = "Przyk³adowy url GET DETAILS 3",
                         Price = 100.00F
                     }
                );
            }
        }

        [Test, Order(1)]
        [TestCaseSource(nameof(ProductCreateDtoSourceData))]
        public async Task AddProduct_TestCorrect(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var result = await unitOfWork.ProductRepository.AddProduct(productCreateDto);
            result.EnsureSuccessMessage("Product added");
            Assert.AreEqual(productCreateDto.Name, result.Data.Name);
            Assert.AreEqual(productCreateDto.Category, result.Data.Category);
            Assert.AreEqual(productCreateDto.Description, result.Data.Description);
            Assert.AreEqual(productCreateDto.Photo, result.Data.Photo);
            Assert.AreEqual(productCreateDto.Price, result.Data.Price);
        }

        [Test, Order(2)]
        [TestCaseSource(nameof(ProductToAddSourceData))]
        public async Task AddProduct_ShouldReturnProductExists(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var result = await unitOfWork.ProductRepository.AddProduct(productCreateDto);
            Assert.True(result.Success);


            result = await unitOfWork.ProductRepository.AddProduct(productCreateDto);
            result.EnsureFailMessage("A product with that name already exists");
        }

        [Test, Order(3)]
        [TestCaseSource(nameof(ProductCreateDtoSourceData))]
        public async Task GetProducts_TestCorrect(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            await unitOfWork.ProductRepository.AddProduct(productCreateDto);

            var result = await unitOfWork.ProductRepository.GetProducts();
            result.EnsureSuccessMessage("Success");
        }

        [Test, Order(4)]
        [TestCaseSource(nameof(ProductsToDeleteSourceData))]
        public async Task DeleteProduct_TestCorrect(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();
            var context = scopedServices.GetRequiredService<DataContext>();

            var call = await unitOfWork.ProductRepository.AddProduct(productCreateDto);

            var result = await unitOfWork.ProductRepository.DeleteProduct(call.Data.Id);
            result.EnsureSuccessMessage("Product has been deleted");
            Assert.AreEqual(call.Data.Id, result.Data.Id);
            AssertCartRemoved(context, call.Data.Id);
            AssertOrderRemoved(context, call.Data.Id);
        }

        private void AssertCartRemoved(DataContext context, Guid cartId)
        {
            Assert.False(context.Carts.AnyAsync(x => x.ProductId == cartId).Result);
        }

        private void AssertOrderRemoved(DataContext context, Guid orderId)
        {
            Assert.False(context.Orders.AnyAsync(x => x.ProductId == orderId).Result);
        }

        [Test, Order(5)]
        [Repeat(3)]
        public async Task DeleteProduct_ShouldReturnProductDoesNotExist()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var result = await unitOfWork.ProductRepository.DeleteProduct(Guid.Empty);
            result.EnsureFailMessage("Product does not exist");
        }

        [Test, Order(6)]
        [TestCaseSource(nameof(ProductsToUpdateSourceData))]
        public async Task EditProduct_TestCorrect(ProductCreateDTO productCreateDto, ProductUpdateDTO productUpdateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var call = await unitOfWork.ProductRepository.AddProduct(productCreateDto);

            productUpdateDto.Id = call.Data.Id;

            var result = await unitOfWork.ProductRepository.EditProduct(productUpdateDto);

            result.EnsureSuccessMessage("Product edited");
            Assert.AreEqual(productUpdateDto.Id, result.Data.Id);
            Assert.AreEqual(productUpdateDto.Name, result.Data.Name);
            Assert.AreEqual(productUpdateDto.Category, result.Data.Category);
            Assert.AreEqual(productUpdateDto.Description, result.Data.Description);
            Assert.AreEqual(productUpdateDto.Photo, result.Data.Photo);
            Assert.AreEqual(productUpdateDto.Price, result.Data.Price);
        }

        [Test, Order(7)]
        [Repeat(3)]
        public async Task EditProduct_ShouldReturnProductDoesNotExist()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            ProductUpdateDTO productUpdateDto = new ProductUpdateDTO
            {
                Id = Guid.Empty,
                Name = "Test",
                Category = "Test",
                Description = "Test",
                Photo = "Test",
                Price = 0.0F
            };

            var result = await unitOfWork.ProductRepository.EditProduct(productUpdateDto);
            result.EnsureFailMessage("Product does not exist");
        }

        [Test, Order(8)]
        [TestCaseSource(nameof(ProductsToGetDetailsSourceData))]
        public async Task GetProductDetails_TestCorrect(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var call = await unitOfWork.ProductRepository.AddProduct(productCreateDto);

            var result = await unitOfWork.ProductRepository.GetProductDetails(call.Data.Id);
            result.EnsureSuccessMessage("Success");
            Assert.AreEqual(call.Data.Id, result.Data.Id);
            Assert.AreEqual(productCreateDto.Name, result.Data.Name);
            Assert.AreEqual(productCreateDto.Category, result.Data.Category);
            Assert.AreEqual(productCreateDto.Description, result.Data.Description);
            Assert.AreEqual(productCreateDto.Photo, result.Data.Photo);
            Assert.AreEqual(productCreateDto.Price, result.Data.Price);
        }

        [Test, Order(9)]
        [Repeat(3)]
        public async Task GetProductDetails_TestShouldReturnProductDoesNotExist()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var result = await unitOfWork.ProductRepository.GetProductDetails(Guid.Empty);
            result.EnsureFailMessage("Product does not exist");
        }

        [Test, Order(10)]
        [TestCaseSource(nameof(ProductCreateDtoSourceData))]
        public async Task SearchProducts_TestCorrect(ProductCreateDTO productCreateDto)
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();
            var context = scopedServices.GetRequiredService<DataContext>();

            await unitOfWork.ProductRepository.AddProduct(productCreateDto);

            var result = await unitOfWork.ProductRepository.SearchProducts(productCreateDto.Name);
            result.EnsureSuccessMessage("Success");
            AssertProductFound(context, result.Data, productCreateDto.Name);
        }

        private void AssertProductFound(DataContext context, List<Product> products, string productName)
        {
            Assert.True(products.Contains(context.Products.FirstOrDefaultAsync(x => x.Name == productName).Result));
        }

        [Test, Order(11)]
        [Repeat(3)]
        public async Task SearchProducts_TestCorrectQueryNull()
        {
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var unitOfWork = scopedServices.GetRequiredService<IUnitOfWork>();

            var result = await unitOfWork.ProductRepository.SearchProducts(null);
            result.EnsureSuccessMessage("Success");
            Assert.True(result.Data.Count == 0);
        }
    }
}