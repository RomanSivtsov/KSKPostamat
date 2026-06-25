using Core.Interfaces;
using Core.Models;
using System.Text.Json;

namespace Backend.Services
{
    public class MockCellService : ICellService
    {
        private List<Cell> _cells = new();

        // Класс-обертка для соответствия структуре JSON {"cells": [...]}
        private class ConfigWrapper { public List<Cell> Cells { get; set; } = new(); }

        public MockCellService()
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Пытаемся найти файл в папке Config (как в твоем проекте)
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "cells.json");

                // Если там нет, проверяем корень (на всякий случай)
                if (!File.Exists(jsonPath))
                {
                    jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cells.json");
                }

                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var wrapper = JsonSerializer.Deserialize<ConfigWrapper>(json, options);

                    if (wrapper?.Cells != null && wrapper.Cells.Any())
                    {
                        _cells = wrapper.Cells;
                        Console.WriteLine($"Успешно загружено ячеек: {_cells.Count} из {jsonPath}");
                    }
                }
                else
                {
                    // Если файл так и не найден, выводим в консоль бэкенда, где именно мы его искали
                    Console.WriteLine("КРИТИЧЕСКАЯ ОШИБКА: Файл cells.json не найден!");
                    Console.WriteLine($"Путь поиска 1: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "cells.json")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении JSON: {ex.Message}");
            }
        }

        public async Task<List<Cell>> GetStatusCells() => await Task.FromResult(_cells);

        public async Task<bool> OpenCellAsync(int cellId)
        {
            var cell = _cells.FirstOrDefault(c => c.CellId == cellId);
            if (cell != null)
            {
                cell.IsClosed = false;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> CloseCellAsync(int cellId)
        {
            var cell = _cells.FirstOrDefault(c => c.CellId == cellId);
            if (cell != null)
            {
                cell.IsClosed = true;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}