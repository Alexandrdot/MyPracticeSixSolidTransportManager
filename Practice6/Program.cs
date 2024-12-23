using System;
using System.Collections.Generic;
using System.Linq;

// Interfaces
public interface ITransport
{
    string Firm { get; set; }
    string Model { get; set; }
    int Price { get; set; }
    void Print();
}

public interface IModifiableTransport : ITransport
{
    void Modify(string propertyName, string newValue);
}


public abstract class Transport : ITransport
{
    public string Firm { get; set; }
    public string Model { get; set; }
    public int Price { get; set; }

    protected Transport(string firm, string model, int price)
    {
        Firm = firm;
        Model = model;
        Price = price;
    }

    public virtual void Print()
    {
        Console.Write($"Фирма: {Firm}, Модель: {Model}, Цена: {Price}");
    }
}


public class Car : Transport, IModifiableTransport
{
    public int Torque { get; set; }

    public Car(string firm, string model, int price, int torque) : base(firm, model, price)
    {
        Torque = torque;
    }

    public override void Print()
    {
        base.Print();
        Console.Write($", Крутящий момент: {Torque}");
    }

    public void Modify(string propertyName, string newValue)
    {
        switch (propertyName.ToLower())
        {
            case "firm": Firm = newValue; break;
            case "model": Model = newValue; break;
            case "price": Price = int.Parse(newValue); break;
            case "torque": Torque = int.Parse(newValue); break;
            default: Console.WriteLine("Неизвестное свойство."); break;
        }
    }
}

public class E_Scooter : Transport, IModifiableTransport
{
    public int TimeWorking { get; set; }

    public E_Scooter(string firm, string model, int price, int timeWorking) : base(firm, model, price)
    {
        TimeWorking = timeWorking;
    }

    public override void Print()
    {
        base.Print();
        Console.Write($", Время работы: {TimeWorking}");
    }

    public void Modify(string propertyName, string newValue)
    {
        switch (propertyName.ToLower())
        {
            case "firm": Firm = newValue; break;
            case "model": Model = newValue; break;
            case "price": Price = int.Parse(newValue); break;
            case "timeworking": TimeWorking = int.Parse(newValue); break;
            default: Console.WriteLine("Неизвестное свойство."); break;
        }
    }
}

//обработка ввода
public class ConsoleIO
{
    public string ReadLine() => Console.ReadLine();
    public void WriteLine(string message) => Console.WriteLine(message);
    public void Write(string message) => Console.Write(message);
    public int ReadInt(string prompt)
    {
        Console.Write(prompt);
        while (true)
        {
            if (int.TryParse(ReadLine(), out int value)) return value;
            WriteLine("Некорректный ввод. Повторите.");
        }
    }
}


public class TransportManager
{
    private readonly ConsoleIO _io;
    private List<ITransport> _transports = new List<ITransport>();

    public TransportManager(ConsoleIO io)
    {
        _io = io;
    }

    public void AddTransport(ITransport transport) => _transports.Add(transport);
    public void RemoveTransport(int index)
    {
        if (index <= 0 || index > _transports.Count)
        {
            _io.WriteLine("Некорректный номер.");
            return;
        }
        _transports.RemoveAt(index - 1);
    }  
    public void PrintAll()
    {
        if (_transports.Count == 0)
        {
            _io.WriteLine("Список транспортных средств пуст.");
            return;
        }
        for (int i = 0; i < _transports.Count; i++)
        {
            _io.WriteLine("");
            _io.Write($"({i + 1}) ");
            _transports[i].Print();
        }
    }

    public void ModifyTransport()
    {
        PrintAll();
        _io.WriteLine("");
        int index = _io.ReadInt("Введите номер транспортного средства для изменения: ");

        if (index <= 0 || index > _transports.Count)
        {
            _io.WriteLine("Некорректный номер.");
            return;
        }

        var transport = _transports[index - 1];
        if (transport is IModifiableTransport modifiableTransport)
        {
            _io.WriteLine("Выберите свойство для изменения:");
            _io.WriteLine("1. Фирма");
            _io.WriteLine("2. Модель");
            _io.WriteLine("3. Цена");

            if (transport is Car) _io.WriteLine("4. Крутящий момент");
            if (transport is E_Scooter) _io.WriteLine("4. Время работы");

            int choice = _io.ReadInt("Выбор: ");

            string propertyName = null;
            switch (choice)
            {
                case 1:
                    propertyName = "Firm";
                    Console.Write("Новая фирма: ");
                    break;
                case 2:
                    propertyName = "Model";
                    Console.Write("Новая модель: ");
                    break;
                case 3:
                    propertyName = "Price";
                    Console.Write("Новая цена: ");
                    break;
                case 4:
                    if (transport is Car)
                    {
                        propertyName = "Torque";
                        Console.Write("Новый крутящий момент: ");
                        break;
                    }

                    else if (transport is E_Scooter)
                    {
                        propertyName = "TimeWorking";
                        Console.Write("Новое время работы: ");
                        break;
                    }
                    break;
                default: _io.WriteLine("Некорректный выбор."); return;
            }
            if (propertyName != null)
            {
                string newValue = _io.ReadLine();
                modifiableTransport.Modify(propertyName, newValue);
            }
        }
        else
        {
            _io.WriteLine("Транспортное средство нельзя изменить.");
        }
    }
}



public class TransportFactory
{
    public static ITransport CreateTransport(string type, ConsoleIO io)
    {
        Console.Write("Фирма: ");
        string firm = io.ReadLine();
        Console.Write("Модель: ");
        string model = io.ReadLine();
        int price = io.ReadInt("Цена: ");

        if (type.Equals("car", StringComparison.OrdinalIgnoreCase))
        {
            int torque = io.ReadInt("Крутящий момент: ");
            return new Car(firm, model, price, torque);
        }
        else if (type.Equals("e-scooter", StringComparison.OrdinalIgnoreCase))
        {
            int timeWorking = io.ReadInt("Время работы: ");
            return new E_Scooter(firm, model, price, timeWorking);
        }
        else
        {
            return null;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var io = new ConsoleIO();
        var transportManager = new TransportManager(io);

        // Main loop
        while (true)
        {
            io.WriteLine("\nМеню:");
            io.WriteLine("1. Добавить автомобиль");
            io.WriteLine("2. Добавить электросамокат");
            io.WriteLine("3. Вывести список");
            io.WriteLine("4. Изменить характеристики т/с");
            io.WriteLine("5. Удалить т/с");
            io.WriteLine("0. Выход");

            int choice = io.ReadInt("Ваш выбор: ");

            switch (choice)
            {
                case 1:
                    var car = TransportFactory.CreateTransport("car", io);
                    if (car != null) transportManager.AddTransport(car);
                    break;
                case 2:
                    var scooter = TransportFactory.CreateTransport("e-scooter", io);
                    if (scooter != null) transportManager.AddTransport(scooter);
                    break;
                case 3: transportManager.PrintAll(); break;
                case 4: transportManager.ModifyTransport(); break;
                case 5: // реализовать удаление
                    transportManager.PrintAll();
                    io.WriteLine("");
                    int indexToRemove = io.ReadInt("Введите номер т/с для удаления: ");
                    
                    transportManager.RemoveTransport(indexToRemove);
                    break;
                case 0: return;
                default: io.WriteLine("Некорректный выбор."); break;
            }
        }
    }
}