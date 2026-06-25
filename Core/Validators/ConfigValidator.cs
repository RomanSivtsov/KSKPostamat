using Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validators
{
    public class CellValidator : AbstractValidator<Cell>
    {
        public CellValidator()
        {
            // номера ячеек должны быть положительными
            RuleFor(x => x.CellId)
                .GreaterThan(0)
                .WithMessage("ID ячейки должен быть больше 0.");

            RuleFor(x => x.OrderId)
                .NotNull()
                .NotEmpty()
                .When(x => x.CellHasPackage)
                .WithMessage(x => $"Ячейка {x.CellId} " +
                $"помечена как занятая, но номер заказа отсутствует.");
        }
    }

    public class PostamatConfigValidator : AbstractValidator<PostamatConfig>
    {
        public PostamatConfigValidator()
        {
            // ... ваши прошлые правила (RuleForEach и уникальность CellId) ...
            RuleForEach(x => x.Cells).SetValidator(new CellValidator());

            RuleFor(x => x.Cells)
                .Must(HaveUniqueIds)
                .WithMessage("Обнаружены дубликаты ID ячеек в конфигурации.");

            // ДОБАВЬТЕ ЭТО ПРАВИЛО:
            RuleFor(x => x.Cells)
                .Must(HaveUniqueOrderIds)
                .WithMessage("Один и тот же номер заказа найден в разных ячейках.");
        }

        // Метод для проверки уникальности CellId (у вас он уже должен быть)
        private bool HaveUniqueIds(List<Cell> cells) =>
            cells.Select(c => c.CellId).Distinct().Count() == cells.Count;

        // НОВЫЙ МЕТОД: Проверка уникальности OrderId
        private bool HaveUniqueOrderIds(List<Cell> cells)
        {
            // Собираем все OrderId, которые НЕ null
            var activeOrders = cells
                .Where(c => c.OrderId != null)
                .Select(c => c.OrderId)
                .ToList();

            // Проверяем, совпадает ли общее количество с количеством уникальных
            return activeOrders.Count == activeOrders.Distinct().Count();
        }
    }
}
