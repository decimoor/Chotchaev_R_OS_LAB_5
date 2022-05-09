using System.Diagnostics;


namespace decimoor
{
  class Program
  {
    List<bool> PCUtoDelete;
    static void Main()
    {
      Scheduler scheduler = new Scheduler();
      scheduler.AddProccess(new PCU("High", new List<Action>() { f1, f4 }, stopFunction));
      scheduler.AddProccess(new PCU("Middle", new List<Action>() { f2, f2, f2 }, stopFunction));
      scheduler.AddProccess(new PCU("Low", new List<Action>() { f3, f3, f3 }, stopFunction));

      scheduler.Run();

      void f1()
      {
        Console.WriteLine("Program 1");
      }

      void f2()
      {
        Console.WriteLine("Program 2");
      }

      void f3()
      {
        Console.WriteLine("Program 3");
      }

      void f4()
      {
        Console.WriteLine(5);
      }

      void stopFunction(int id)
      {
        if (Console.ReadKey().Key == ConsoleKey.Q)
        {
          Scheduler._PCUtoDelete[id] = true;
        }
      }
    }
  }
}
