using Backend.Services;
using Core.Models;
using Xunit;
using System.Threading.Tasks;

namespace Unit
{
    public class CellServiceTests
    {
        [Fact]
        public async Task OpenCell_Should_ChangeStatus_ToOpen()
        {
            // 1. Arrange (Подготовка)
            var service = new MockCellService();
            int cellId = 7;

            // 2. Act (Действие)
            bool result = await service.OpenCellAsync(cellId);

            // ВАЖНО: вызываем GetStatusCells() - имя из твоего нового кода
            var allCells = await service.GetStatusCells();

            // Если файл cells.json не найден в папке тестов, allCells будет пустым.
            // Добавим проверку, чтобы тест не падал с непонятной ошибкой:
            Assert.NotEmpty(allCells);

            var targetCell = allCells.Find(c => c.CellId == cellId);

            // 3. Assert (Проверка)
            Assert.NotNull(targetCell);
            Assert.True(result);

            // ВАЖНО: проверяем на false (так как в твоем сервисе теперь bool)
            Assert.False(targetCell.IsClosed);
        }
    }
}