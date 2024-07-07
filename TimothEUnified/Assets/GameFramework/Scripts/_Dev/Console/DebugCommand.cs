using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescripton;
    private string _commandFormat;

    public string CommandId { get => _commandId; }
    public string CommandDescription { get => _commandDescripton; }
    public string CommandFormat { get => _commandFormat; }

    public DebugCommandBase(string id, string descripton, string format)
    {
        _commandId = id;
        _commandDescripton = descripton;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase 
{
    private Action _command;

    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke()
    {
        _command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> _command;

    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        _command = command;
    }

    public void Invoke(T1 value)
    {
        _command.Invoke(value);
    }
}

