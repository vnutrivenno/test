using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите ваше имя:");
        string userName = Console.ReadLine();

        TypingTest typingTest = new TypingTest(userName);

        Console.WriteLine("Нажмите Enter, чтобы начать тест...");
        Console.ReadLine();

        typingTest.StartTest();

        Console.WriteLine("Тест завершен. Ваши результаты:");

        // Вывод результатов теста
        typingTest.DisplayResults();

        // Сохранение результатов теста
        typingTest.SaveResults();

        Console.WriteLine("Нажмите Enter, чтобы выйти.");
        Console.ReadLine();
    }
}

class TypingTest
{
    private string userName;
    private Stopwatch stopwatch;
    private List<TestResult> testResults;

    public TypingTest(string userName)
    {
        this.userName = userName;
        this.testResults = LoadResults();
    }

    public void StartTest()
    {
        Console.WriteLine("Напишите следующий текст:");

        string targetText = "Это тестовый текст для проверки набора. Попробуйте ввести его как можно быстрее и без ошибок.";

        Console.WriteLine(targetText);

        stopwatch = Stopwatch.StartNew();
        string userInput = Console.ReadLine();
        stopwatch.Stop();

        // Рассчитываем количество символов в минуту и в секунду
        int charactersTyped = userInput.Length;
        double charactersPerMinute = charactersTyped / (stopwatch.Elapsed.TotalMinutes);
        double charactersPerSecond = charactersTyped / (stopwatch.Elapsed.TotalSeconds);

        // Сохраняем результат теста
        TestResult result = new TestResult(userName, charactersPerMinute, charactersPerSecond);
        testResults.Add(result);
    }

    public void DisplayResults()
    {
        Console.WriteLine("Таблица рекордов:");
        Console.WriteLine("Имя\tСимволов/мин\tСимволов/сек");

        foreach (var result in testResults)
        {
            Console.WriteLine($"{result.UserName}\t{result.CharactersPerMinute:F2}\t\t{result.CharactersPerSecond:F2}");
        }
    }

    public void SaveResults()
    {
        string json = JsonConvert.SerializeObject(testResults, Formatting.Indented);
        File.WriteAllText("results.json", json);
    }

    private List<TestResult> LoadResults()
    {
        if (File.Exists("results.json"))
        {
            string json = File.ReadAllText("results.json");
            return JsonConvert.DeserializeObject<List<TestResult>>(json);
        }

        return new List<TestResult>();
    }
}

class TestResult
{
    public string UserName { get; set; }
    public double CharactersPerMinute { get; set; }
    public double CharactersPerSecond { get; set; }

    public TestResult(string userName, double charactersPerMinute, double charactersPerSecond)
    {
        UserName = userName;
        CharactersPerMinute = charactersPerMinute;
        CharactersPerSecond = charactersPerSecond;
    }
}
