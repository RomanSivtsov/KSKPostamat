using Backend.Services;
using Xunit;

namespace Unit
{
    public class OrderServiceTests
    {
        private readonly MockOrderService _service = new MockOrderService();

        [Fact]
        public async Task ProcessPayment_Should_SetPaidStatus_WhenOrderExists()
        {
            // Проверяем: если заказ есть, оплата должна пройти
            int orderId = 1001;

            bool result = await _service.ProcessPaymentAsync(orderId);
            var order = await _service.GetOrderByIdAsync(orderId);

            Assert.True(result);
            Assert.True(order.IsPaid); // Убеждаемся, что статус в "БД" изменился
        }

        [Fact]
        public async Task ProcessPayment_Should_ReturnFalse_WhenOrderNotFound()
        {
            // Проверяем: оплата несуществующего заказа не должна "ломать" систему
            int fakeId = 9999;

            bool result = await _service.ProcessPaymentAsync(fakeId);

            Assert.False(result);
        }
    }
}