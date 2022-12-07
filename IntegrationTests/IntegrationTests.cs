using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Shop.DTO.Product;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;

namespace IntegrationTests
{
    class LoginResponse
    {
        public string Token { get; set; }
    }

    class ProductsResponse
    {
        public List<ProductGetDTO> Products { get; set; }   
    }

    record PriceResponse(float orderPrice);

    public class Tests
    {
        private static HttpClient client;

        [SetUp]
        public void Setup()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            client = webAppFactory.CreateDefaultClient();
        }

        [Test]
        public async Task RegisterTest()
        {
            Faker fake = new Faker();
            var response = await client.PostAsJsonAsync("api/user/register",
                new { Password1 = "Test1!!!", Password2 = "Test1!!!", Username = fake.Person.LastName, Email = fake.Internet.Email() });
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task LoginTest()
        {
            var response = await client.PostAsJsonAsync("api/user/login",
                new { Email = "Neoma_Jacobs67@gmail.com", Password = "Test1!!!" });
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.IsNotNull(data);
            Assert.IsNotEmpty(data.Token);
        }

        [Test]
        [Repeat(10)]
        public async Task IncorrectLoginTest()
        {
            Random rand = new Random();
            var response = await client.PostAsJsonAsync("api/user/login",
                new { Email = rand.Next(100000, 1000000) + "@gmail.com", Password = "Test1!!!" });
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task AddProductToCart()
        {
            await AuthorizeUser();
            var response = await client.GetAsync("api/product/getProducts");
            ProductsResponse data = await response.Content.ReadFromJsonAsync<ProductsResponse>();
            Assert.IsNotEmpty(data.Products);

            response = await client.PostAsJsonAsync("api/cart/addToCart", new { Id = data.Products[0].Id });
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task CheckProductsInCartPrice()
        {
            Faker fake = new Faker();
            string email = fake.Internet.Email();
            string password = "Test1!!!";
            var response = await client.PostAsJsonAsync("api/user/register",
                new { Password1 = password, Password2 = password, Username = fake.Person.LastName, Email = email });
            response.EnsureSuccessStatusCode();

            await AuthorizeClient(email, password);

            response = await client.GetAsync("api/product/getProducts");
            ProductsResponse data = await response.Content.ReadFromJsonAsync<ProductsResponse>();
            Assert.IsNotEmpty(data.Products);

            response = await client.PostAsJsonAsync("api/cart/addToCart", new { Id = data.Products[0].Id });
            response.EnsureSuccessStatusCode();

            response = await client.PostAsJsonAsync("api/cart/addToCart", new { Id = data.Products[1].Id });
            response.EnsureSuccessStatusCode();

            PriceResponse price = await client.GetFromJsonAsync<PriceResponse>("api/order/orderPrice");
            Assert.AreEqual(float.Parse(data.Products[0].Price) + float.Parse(data.Products[1].Price), price.orderPrice);
        }

        [Test]
        [TestCaseSource(nameof(ProductCreateDtoSourceData))]
        public async Task AddProductNotAdmin(ProductCreateDTO dto)
        {
            await AuthorizeUser();
            Faker fake = new Faker();
            dto.Name = fake.Commerce.ProductName();
            var response = await client.PostAsJsonAsync("api/product/addProduct", dto);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        [TestCaseSource(nameof(ProductCreateDtoSourceData))]
        public async Task AddProduct(ProductCreateDTO dto)
        {
            await AuthorizeAdmin();
            Faker fake = new Faker();
            dto.Name = fake.Commerce.ProductName();
            var response = await client.PostAsJsonAsync("api/product/addProduct", dto);
            response.EnsureSuccessStatusCode();
        }

        [Test]
        [TestCaseSource(nameof(ProductNamesSourceData))]
        public async Task SearchProduct(string productName)
        {
            await AuthorizeUser();
            var response = await client.GetAsync($"api/product/search/{productName}");
            response.EnsureSuccessStatusCode();
            ProductsResponse data = await response.Content.ReadFromJsonAsync<ProductsResponse>();
            Assert.IsNotEmpty(data.Products);
            foreach(var item in data.Products)
            {
                Assert.True(item.Name.Contains(productName));
            }
        }

        private async Task AuthorizeUser()
        {
            await AuthorizeClient("Neoma_Jacobs67@gmail.com", "Test1!!!");
        }

        private async Task AuthorizeAdmin()
        {
            await AuthorizeClient("admin@gmail.com", "password123");
        }

        private async Task AuthorizeClient(string username, string password)
        {
            var response = await client.PostAsJsonAsync("api/user/login",
                new { Email = username, Password = password });
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + data.Token);
        }

        private static IEnumerable<string> ProductNamesSourceData
        {
            get
            {
                yield return "Ksi¹¿ka o C++";
                yield return "Small Soft Tuna";
            }
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
    }
}