using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PddApp.Models; 

namespace PddApp.Services
{
    public class DataService
    {
        public static List<Ticket>? LoadTicketsFromFile(string fileName)
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory;


            string fullPath = Path.Combine(basePath, fileName);

            try
            {
                if (!File.Exists(fullPath))
                {

                    System.Diagnostics.Debug.WriteLine($"ФАЙЛ НЕ НАЙДЕН ПО ПУТИ: {fullPath}");
                    return null;
                }

                string jsonString = File.ReadAllText(fullPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var tickets = JsonSerializer.Deserialize<List<Ticket>>(jsonString, options);

                System.Diagnostics.Debug.WriteLine($"✅ Успешно загружено билетов: {tickets?.Count}");
                return tickets;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ОШИБКА: {ex.Message}");
                return null;
            }
        }
    }
}