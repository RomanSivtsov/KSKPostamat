using Core.Models;
using Core.Validators;
using FluentValidation.TestHelper;

namespace Unit
{
    public class ValidationTests
    {
        private readonly PostamatConfigValidator _validator = new PostamatConfigValidator();

        [Fact]
        public void Should_Have_Error_When_CellId_Is_Duplicate()
        {
            // две ячейки с ID = 1
            var config = new PostamatConfig
            {
                Cells = new List<Cell>
                {
                    new Cell { CellId = 1 },
                    new Cell { CellId = 1 }
                }
            };

            // тестирование
            var result = _validator.TestValidate(config);

            // проверка на ошибки
            result.ShouldHaveValidationErrorFor(x => x.Cells)
                  .WithErrorMessage("Обнаружены дубликаты ID ячеек в конфигурации.");
        }

        [Fact]
        public void Should_Have_Error_When_Package_Exists_But_OrderId_Is_Null()
        {
            // ячейка 2 занята, но OrderId = null
            var config = new PostamatConfig
            {
                Cells = new List<Cell>
        {
            new Cell { CellId = 2, CellHasPackage = true, OrderId = null }
        }
            };

            var result = _validator.TestValidate(config);

            // 1. Проверяем через IsValid (базовая проверка xUnit)
            Assert.False(result.IsValid);

            // 2. ИЛИ проверяем конкретную ошибку (это лучший стиль в тестировании)
            // Мы ожидаем ошибку в первом элементе списка Cells (индекс 0) для поля OrderId
            result.ShouldHaveValidationErrorFor("Cells[0].OrderId");
        }

        [Fact]
        public void Should_Have_Error_When_OrderId_Is_Duplicate_In_Different_Cells()
        {
            // Ситуация: Заказ 1001 ошибочно прописан в двух разных ячейках
            var config = new PostamatConfig
            {
                Cells = new List<Cell>
        {
            new Cell { CellId = 1, OrderId = 1001, CellHasPackage = true },
            new Cell { CellId = 2, OrderId = 1001, CellHasPackage = true }
        }
            };

            var result = _validator.TestValidate(config);

            // Если вы добавили правило уникальности OrderId в валидатор, тест это поймает
            result.ShouldHaveValidationErrorFor(x => x.Cells)
                  .WithErrorMessage("Один и тот же номер заказа найден в разных ячейках.");
        }
    }
}