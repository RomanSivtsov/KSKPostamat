using Backend.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. РЕГИСТРАЦИЯ СЕРВИСОВ (Dependency Injection)
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IOrderService, MockOrderService>();
builder.Services.AddSingleton<ICellService, MockCellService>();

// 2. НАСТРОЙКА CORS (Разрешаем запросы от нашего интерфейса Terminal)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // Позволяет запросы с любого адреса
              .AllowAnyMethod()   // Позволяет любые методы (GET, POST и т.д.)
              .AllowAnyHeader();  // Позволяет любые заголовки
    });
});

var app = builder.Build();

// 3. ПОДКЛЮЧЕНИЕ ПРОМЕЖУТОЧНОГО ПО (Middleware)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ОБЯЗАТЕЛЬНО: Включаем CORS перед эндпоинтами
app.UseCors();

// Мы можем оставить или убрать HttpsRedirection. 
// Для локальной отладки WebView2 иногда проще без него.
// app.UseHttpsRedirection();

// 4. ЭНДПОИНТЫ (API МАРШРУТЫ)

// Получить состояние всех ячеек для сетки в интерфейсе
app.MapGet("/api/cells", async (ICellService cellService) =>
{
    var cells = await cellService.GetStatusCells();
    return Results.Ok(cells);
});

// Получить информацию о заказе по номеру
app.MapGet("/api/order/{id}", async (int id, IOrderService orderService) =>
{
    var order = await orderService.GetOrderByIdAsync(id);
    return order is not null ? Results.Ok(order) : Results.NotFound("Заказ не найден");
});

// Ручное открытие конкретной ячейки по клику в интерфейсе
app.MapPost("/api/cells/open/{id}", async (int id, ICellService cellService) =>
{
    var result = await cellService.OpenCellAsync(id);
    return result ? Results.Ok($"Ячейка {id} открыта") : Results.NotFound("Ячейка не найдена");
});

app.MapPost("/api/cells/close/{id}", async (int id, ICellService cellService) =>
{
    await cellService.CloseCellAsync(id);
    return Results.Ok();
});

// Основной процесс: оплата и автоматическое открытие ячейки
// Мы используем [FromQuery], чтобы сервер точно подхватил id из URL (?orderId=1001)
app.MapPost("/api/process-payment", async ([FromQuery] int orderId, IOrderService orderService, ICellService cellService) =>
{
    // 1. Ищем заказ
    var order = await orderService.GetOrderByIdAsync(orderId);
    if (order == null)
        return Results.NotFound("Заказ не найден в системе 1С");

    // 2. Имитируем оплату
    var paymentSuccess = await orderService.ProcessPaymentAsync(orderId);
    if (!paymentSuccess)
        return Results.BadRequest("Ошибка при проведении транзакции оплаты");

    // 3. Если оплата прошла, открываем соответствующую ячейку
    var openSuccess = await cellService.OpenCellAsync(order.CellId);

    if (openSuccess)
    {
        return Results.Ok(new
        {
            Message = $"Заказ {orderId} успешно оплачен. Пожалуйста, заберите посылку из ячейки №{order.CellId}.",
            CellId = order.CellId
        });
    }

    return Results.Problem("Оплата зафиксирована, но произошел технический сбой при открытии замка ячейки.");
});

app.Run();