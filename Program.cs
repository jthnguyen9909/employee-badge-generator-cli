using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
  class Program
  {
    public static List<Option> options;
    async static Task Main(string[] args)
    {
      List<Employee> employees = new List<Employee>();
      options = new List<Option>
      {
      new Option("Manually Input Employees", () => employees = PeopleFetcher.GetEmployees()),
      new Option("Randomly Generate Employees", async () => employees = await PeopleFetcher.GetFromApi()),
      // new Option("Manually Input Employees", () => Console.WriteLine("Manually Input Employees")),
      // new Option("Randomly Generate Employees", () => Console.WriteLine("Randomly Generate Employees")),
    };

      int index = 0;

      // Write the menu out
      WriteMenu(options, options[index]);

      // Store key info in here
      ConsoleKeyInfo keyinfo;
      do
      {
        keyinfo = Console.ReadKey();

        // Handle each key input (down arrow will write the menu again with a different selected item)
        if (keyinfo.Key == ConsoleKey.DownArrow)
        {
          if (index + 1 < options.Count)
          {
            index++;
            WriteMenu(options, options[index]);
          }
        }
        if (keyinfo.Key == ConsoleKey.UpArrow)
        {
          if (index - 1 >= 0)
          {
            index--;
            WriteMenu(options, options[index]);
          }
        }
        // Handle different action for the option
        if (keyinfo.Key == ConsoleKey.Enter)
        {
          options[index].Selected.Invoke();
          index = 0;
          break;
        }
      }
      while (keyinfo.Key != ConsoleKey.X);

      Console.ReadKey();




      // List<Employee> employees = PeopleFetcher.GetEmployees();
      // employees = await PeopleFetcher.GetFromApi();
      Util.PrintEmployees(employees);
      Util.MakeCSV(employees);
      await Util.MakeBadges(employees);
      return;
    }



    static void WriteTemporaryMessage(string message)
    {
      Console.Clear();
      Console.WriteLine(message);
      Thread.Sleep(3000);
      WriteMenu(options, options.First());
    }

    static void WriteMenu(List<Option> options, Option selectedOption)
    {
      Console.Clear();

      foreach (Option option in options)
      {
        if (option == selectedOption)
        {
          Console.Write("> ");
        }
        else
        {
          Console.Write(" ");
        }

        Console.WriteLine(option.Name);
      }
    }
  }



  public class Option
  {
    public string Name { get; }
    public Action Selected { get; }

    public Option(string name, Action selected)
    {
      Name = name;
      Selected = selected;
    }
  }
}