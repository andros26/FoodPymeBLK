using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesFullAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
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