using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace decimoor
{
  
  internal class Scheduler
  {
    public static List<PCU> _processes = new List<PCU>();
    public static List<bool> _PCUtoDelete = new List<bool>();

    public static bool Ready = true;

    public void AddProccess(PCU p)
    {
      _processes.Add(p);
      _processes.Sort((PCU1, PCU2) =>  PCU1.Compare(PCU2));
      _PCUtoDelete.Add(false);

    }

    public void Run()
    {

      bool condition = true;
      while (condition)
      {
        
        foreach (var p in _processes)
        {
          for (int i = 0; i < _PCUtoDelete.Count; i++)
          {
            if (_PCUtoDelete[i])
            {
              _processes[i].Done();
              _PCUtoDelete[i] = true;
            }
          }
          if (!p.GetDescriptor()._context.isDone) p.Run();
          Thread.Sleep(100);
          while (p.GetDescriptor()._haveControl)
          {
            Console.WriteLine("Scheduler is waiting");
            Thread.Sleep(800);
          }
          Console.WriteLine("Getting to the new process");
        }
        condition = false;
        _PCUtoDelete.ForEach(p =>
        {
          if (!p)
            condition = true;
        });
      }
    }

  }
}
