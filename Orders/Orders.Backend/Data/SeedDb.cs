using Microsoft.EntityFrameworkCore;
using Orders.Backend.Helpers;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IFileStorage _fileStorage;

        public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork, IFileStorage fileStorage)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesFullAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckProductsAsync();
            await CheckUserAsync("1111", "Jovanny", "Alvarez", "jovannyalvarez.py@gmail.com", "3012399996", "CL 1A N 22 -22", UserType.Admin);
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Buñuelo de Arequipe", 3500M, 100F, new List<string>() { "Desayuno y Cafetería", "Repostería" }, new List<string>() { "arequipe.png" });
                await AddProductAsync("Buñuelo de Bocadillo", 3200M, 100F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "bocadillo.png" });
                await AddProductAsync("Buñuelo de Chocolate", 3800M, 100F, new List<string>() { "Desayuno y Cafetería", "Repostería" }, new List<string>() { "chocolate.png" });
                await AddProductAsync("Buñuelo de Queso Especial", 3000M, 100F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "queso.png" });
                await AddProductAsync("Torta de Chocolate Selva Negra", 65000M, 10F, new List<string>() { "Repostería", "Panadería y Pastelería" }, new List<string>() { "c1.png" });
                await AddProductAsync("Torta de Vainilla y Frutos Rojos", 58000M, 10F, new List<string>() { "Repostería" }, new List<string>() { "c2.png" });
                await AddProductAsync("Torta Temática Especial", 85000M, 5F, new List<string>() { "Repostería" }, new List<string>() { "c3.png" });
                await AddProductAsync("Torta de Zanahoria y Nueces", 55000M, 10F, new List<string>() { "Repostería", "Panadería y Pastelería" }, new List<string>() { "c4.png" });
                await AddProductAsync("Torta de Chocolate Intenso", 68000M, 8F, new List<string>() { "Repostería" }, new List<string>() { "c5.png" });
                await AddProductAsync("Porción Torta Marmoleada", 7500M, 20F, new List<string>() { "Repostería", "Desayuno y Cafetería" }, new List<string>() { "c6.png" });
                await AddProductAsync("Cheesecake de Fresa", 72000M, 6F, new List<string>() { "Repostería" }, new List<string>() { "c7.png" });
                await AddProductAsync("Red Velvet Especial", 75000M, 6F, new List<string>() { "Repostería" }, new List<string>() { "c8.png" });
                await AddProductAsync("Banner Promocional Repostería", 0M, 0F, new List<string>() { "Repostería" }, new List<string>() { "banner-background_.png" });
                await AddProductAsync("Hojaldre de Jamón y Queso", 4500M, 50F, new List<string>() { "Panadería y Pastelería" }, new List<string>() { "hojaldre.png" });
                await AddProductAsync("Pan de Bono Familiar", 12000M, 30F, new List<string>() { "Panadería y Pastelería", "Desayuno y Cafetería" }, new List<string>() { "background_.png" }); // Ajustado según tus imágenes
                await AddProductAsync("Caja de Buñuelos x 4", 13000M, 40F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "cajax4.png" });
                await AddProductAsync("Caja Regalo x 8 Buñuelos", 25000M, 20F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "caja-8.png" });
                await AddProductAsync("Caja Surtida Premium", 35000M, 15F, new List<string>() { "Snacks y Golosinas" }, new List<string>() { "box1.png" });
                await AddProductAsync("Caja Especial", 32000M, 15F, new List<string>() { "Snacks y Golosinas" }, new List<string>() { "box2.png" });
                await AddProductAsync("Caja Pastel bocadillo", 38000M, 15F, new List<string>() { "Repostería" }, new List<string>() { "box3.png" });
                await AddProductAsync("Combo Buñuelo + Coca-cola", 15500M, 50F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "combo1.png" });
                await AddProductAsync("Combo Capuchino + Buñuelo", 12500M, 50F, new List<string>() { "Desayuno y Cafetería" }, new List<string>() { "combo2.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (var categoryName in categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category != null)
                {
                    prodcut.ProductCategories.Add(new ProductCategory { Category = category });
                }
            }

            foreach (string? image in images)
            {
                var filePath = $"{Environment.CurrentDirectory}\\Images\\products\\{image}";
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products");
                prodcut.ProductImages.Add(new ProductImage { Image = imagePath });
            }

            _context.Products.Add(prodcut);
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckCountriesFullAsync()
        {
            if (!_context.Countries.Any())
            {
                var countriesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Estados Unidos" });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Frutas y Verduras" });
                _context.Categories.Add(new Category { Name = "Carnes y Aves" });
                _context.Categories.Add(new Category { Name = "Pescados y Mariscos" });
                _context.Categories.Add(new Category { Name = "Lácteos y Huevos" });
                _context.Categories.Add(new Category { Name = "Panadería y Pastelería" });
                _context.Categories.Add(new Category { Name = "Abarrotes y Despensa" });
                _context.Categories.Add(new Category { Name = "Cereales y Legumbres" });
                _context.Categories.Add(new Category { Name = "Pastas y Salsas" });
                _context.Categories.Add(new Category { Name = "Aceites y Condimentos" });
                _context.Categories.Add(new Category { Name = "Enlatados y Conservas" });
                _context.Categories.Add(new Category { Name = "Snacks y Golosinas" });
                _context.Categories.Add(new Category { Name = "Bebidas y Jugos" });
                _context.Categories.Add(new Category { Name = "Licores y Cervezas" });
                _context.Categories.Add(new Category { Name = "Alimentos Congelados" });
                _context.Categories.Add(new Category { Name = "Embutidos y Charcutería" });
                _context.Categories.Add(new Category { Name = "Comida Preparada" });
                _context.Categories.Add(new Category { Name = "Desayuno y Cafetería" });
                _context.Categories.Add(new Category { Name = "Alimentos Orgánicos" });
                _context.Categories.Add(new Category { Name = "Repostería" });
                _context.Categories.Add(new Category { Name = "Alimentos para Bebés" });

                await _context.SaveChangesAsync();
            }
        }
    }
}