using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CheatConsole
{
    public class ConsoleBaseCommand
    {
        string _commandHead;
        string _commandDescription;
        string _commandUsage;

        public string CommandHead { get => _commandHead; }
        public string CommandDescription { get => _commandDescription; }
        public string CommandUsage { get => _commandUsage;  }

        public ConsoleBaseCommand(string head, string description, string usage)
        {
            _commandHead = head;
            _commandDescription = description;
            _commandUsage = usage;
        }

    }

    public class ConsoleCommand : ConsoleBaseCommand
    {
        public Action _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke()
        {
            _commandFunction.Invoke();
        }
    }

    public class ConsoleCommand<T1> : ConsoleBaseCommand
    {
        public Action<T1> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a)
        {
            _commandFunction.Invoke(a);
        }
    }

    public class ConsoleCommand<T1,T2> : ConsoleBaseCommand
    {
        public Action<T1,T2> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1,T2> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b)
        {
            _commandFunction.Invoke(a,b);
        }
    }

    public class ConsoleCommand<T1, T2, T3> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c)
        {
            _commandFunction.Invoke(a, b, c);
        }
    }

    public class ConsoleCommand<T1, T2, T3, T4> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3, T4> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3, T4> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c, T4 d)
        {
            _commandFunction.Invoke(a, b, c, d);
        }
    }

    public class ConsoleCommand<T1, T2, T3, T4, T5> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3, T4, T5> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3, T4, T5> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c, T4 d, T5 e)
        {
            _commandFunction.Invoke(a, b, c, d, e);
        }
    }

    public class ConsoleCommand<T1, T2, T3, T4, T5, T6> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3, T4, T5, T6> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3, T4, T5, T6> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f)
        {
            _commandFunction.Invoke(a, b, c, d, e, f);
        }
    }

    public class ConsoleCommand<T1, T2, T3, T4, T5, T6, T7> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3, T4, T5, T6, T7> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3, T4, T5, T6, T7> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g)
        {
            _commandFunction.Invoke(a, b, c, d, e, f, g);
        }
    }

    public class ConsoleCommand<T1, T2, T3, T4, T5, T6, T7, T8> : ConsoleBaseCommand
    {
        public Action<T1, T2, T3, T4, T5, T6, T7, T8> _commandFunction;

        public ConsoleCommand(string id, string description, string usage, Action<T1, T2, T3, T4, T5, T6, T7, T8> function) : base(id, description, usage)
        {
            _commandFunction = function;
        }

        public void Invoke(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g, T8 h)
        {
            _commandFunction.Invoke(a, b, c, d, e, f, g, h);
        }
    }
}
