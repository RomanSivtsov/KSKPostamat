using Core.Interfaces;
using Core.Models;
using System.Text.Json;

namespace Backend.Services
{
    public class MockOrderService : IOrderService
    {
        private List<Order> _orders = new();

        // Класс для десериализации обертки {"orders": [...]}
        private class OrderWrapper { public List<Order> Orders { get; set; } = new(); }

        public MockOrderService()
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                // Формируем путь к файлу в папке Config
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "orders.json");

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var wrapper = JsonSerializer.Deserialize<OrderWrapper>(json, options);

                    _orders = wrapper?.Orders ?? new();
                    Console.WriteLine($"[MockOrderService] Загружено заказов: _orders.Count");
                }
                else
                {
                    Console.WriteLine($"[MockOrderService] ОШИБКА: Файл не найден по пути {jsonPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MockOrderService] Ошибка чтения JSON: {ex.Message}");
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            return await Task.FromResult(order);
        }

        public async Task<bool> ProcessPaymentAsync(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return false;

            // Меняем статус в оперативной памяти бэкенда
            order.IsPaid = true;
            Console.WriteLine($"[MockOrderService] Заказ {orderId} помечен как оплаченный.");

            return await Task.FromResult(true);
        }
    }
}