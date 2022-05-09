using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace decimoor
{

  struct Context
  {
    public int numOfInstruction;
    public bool isDone;
  }

  struct Descriptor
  {
    public int _id;
    public string _state;
    public int _priority;
    public Context _context;
    public List<Action> _instructions;
    public bool _haveControl = false;

   
  }
  internal class PCU
  {

    Dictionary<string, int> _priorities = new Dictionary<string, int>()
    {
      {"Low", 1 },
      {"Middle", 2 },
      {"High", 3 }
    };

    private readonly int Quant = 45;
    private Stopwatch _stopwatch = new Stopwatch();
    private Descriptor _descriptor;
    private Thread _thread;

    static int _lastId = 0;

    public PCU(string priority, List<Action> instr, Action<int> stopFunction)
    {
      instr.Add(() => stopFunction(_descriptor._id));
      _descriptor = new Descriptor()
      {
        _id = _lastId++,
        _priority = _priorities[priority],
        _state = "Not Ready",
        _context = new Context()
        {
          numOfInstruction = 0,
          isDone = false
        },
        _instructions = instr
      };


    }

    public void Run()
    {
      
      _thread = new Thread(() =>
      {
        Thread keyListener = new Thread(() => _descriptor._instructions[_descriptor._instructions.Count - 1]());
        _stopwatch.Restart();
        _descriptor._haveControl = true;
        for (int i = _descriptor._context.numOfInstruction; _stopwatch.ElapsedMilliseconds < Quant * _descriptor._priority; )
        {
          _descriptor._instructions[i % (_descriptor._instructions.Count - 1)].Invoke();
          _descriptor._context.numOfInstruction = ++i;
          Thread.Sleep(500);
        }
        Console.WriteLine($"Finished {_descriptor._context.numOfInstruction}");
        _descriptor._haveControl = false;
        _stopwatch.Stop();
        keyListener.Interrupt();
      });
      
      _thread.Start();
    }

    public Descriptor GetDescriptor()
    {
      return _descriptor;
    }

    public void Done()
    {
      _descriptor._context.isDone = true;
    }
    public int Compare(PCU toCompare)
    {
      int priority1 = _descriptor._priority;
      int priority2 = toCompare._descriptor._priority;

      if (priority1 > priority2) return -1;
      if (priority1 < priority2) return 1;

      return 0;
    }

  }
}
