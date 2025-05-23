﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Делегат для обработки имени пользователя
public delegate bool NameValidator(string name);

// Класс пользователя
public class User
{
    public string Name { get; set; }
    public int Age { get; set; }

    static NameValidator nameValidator = new NameValidator(ValidateName);

    public bool YName(string name)
    {
        bool yname = nameValidator(name);
        if (yname)
        {
            return true;
        }
        return false;
    }
    public void CreateUser(string name, int age)
    {
        Name = name;
        Age = age;
        User_OnUserCreated(this);
    }

    private static bool ValidateName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private static void User_OnUserCreated(User user)
    {
        Console.WriteLine($"Пользователь {user.Name} успешно зарегистрирован.");

        if (user.Age >= 18)
        {
            using (StreamWriter writer = new StreamWriter("users.txt", true))
            {
                writer.WriteLine($"Имя: {user.Name}, Возраст: {user.Age}");
            }
            Console.WriteLine($"Пользователь {user.Name} добавлен в файл users.txt.");
        }
    }
}

namespace VIZ
{
    internal class Program
    {
        public delegate void Action(string message);
        public static event Action OnUserCreated;
        static void Main(string[] args)
        {
            User user = new User();

            // Подписываемся на событие
            OnUserCreated += DisplayGreeting;

            // Запрос данных у пользователя
            Console.Write("Введите ваше имя: ");
            string name;
            while (!user.YName(name = Console.ReadLine()))
            {
                Console.WriteLine("Пожалуйста, введите корректное имя.");
            }

            Console.Write("Введите ваш возраст: ");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age) || age < 0 || age > 130)
            {
                Console.WriteLine("Пожалуйста, введите корректный возраст.");
            }
            OnUserCreated?.Invoke($"Привет, {name}!");

            // Создание пользователя
            user.CreateUser(name, age);

            void DisplayGreeting(string message)
            {
                Console.WriteLine(message);
            }
        }
    }
}
