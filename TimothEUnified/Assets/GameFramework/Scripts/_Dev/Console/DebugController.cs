using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFramework.Core.Dev
{
    public class DebugController : MonoBehaviour
    {
        // STATE
        bool _bShowConsole;
        bool _bShowHelp = false;
        bool _bShowError = false;

        string _input;
        string _error;
        Vector2 _scrollView;
        public List<object> _commandList;

        Queue<string> _commandQueue;
        int _commandIdx = 0;

        public static DebugController Instance { get; private set; }

        ///////////////  COMMAND LIST  ///////////////////
        public static DebugCommand HELP;
        public static DebugCommand<int> GIVE_GOLD;

        //////////////////////////////////////////////////
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _commandQueue = new Queue<string>();
            _commandList = new List<object>();

            CreateCommand(HELP, "help", "Toggles helpful descriptions of all commands to the developer", () => _bShowHelp = !_bShowHelp);
            CreateCommandInt(GIVE_GOLD, "give_gold", "Gives the player X gold", "give_gold <desiredAmount>", (int x) => Debug.Log(x));
        }

        //////////////////////////////////////////////////
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _bShowConsole = !_bShowConsole;
                _input = "";
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnEnter();
            }

            if (Input.GetKeyDown(KeyCode.Question))
            {
                Autocorrect();
            }
        }

        //////////////////////////////////////////////////
        private void OnGUI()
        {
            if (!_bShowConsole) return;
            float y = 0.0f;

            if (_bShowHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100.0f), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30.0f, 20.0f * _commandList.Count);

                _scrollView = GUI.BeginScrollView(new Rect(0, y + 5.0f, Screen.width, 90.0f), _scrollView, viewport);

                for (int i = 0; i < _commandList.Count; i++)
                {
                    DebugCommandBase command = _commandList[i] as DebugCommandBase;

                    if (command != null)
                    {
                        string label = $"{command.CommandId} - {command.CommandDescription} - {command.CommandFormat}";
                        Rect labelRect = new Rect(5.0f, 20.0f * i, viewport.width - 100.0f, 20.0f);

                        GUI.Label(labelRect, label);
                    }
                }
                y += 100.0f;

                GUI.EndScrollView();
            }

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            GUI.contentColor = Color.white;
            GUI.SetNextControlName("console");
            _input = GUI.TextField(new Rect(10.0f, y + 5.0f, Screen.width - 20.0f, 20), _input);
            TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl); 
            if (textEditor != null) { textEditor.MoveCursorToPosition(new Vector2(5555, 5555)); }

            GUI.FocusControl("console");

            if (_input.Length > 0)
            {
                if (_input.Contains("`"))
                {
                    _bShowConsole = false;
                }
                else if (_input.Contains("#"))
                {
                    HandleInput();
                }
                else if (_input.Contains("?"))
                {
                    Autocorrect();
                }
                else if (_input.Contains("[")) 
                {
                    if(_commandIdx > 0)
                    {
                        _input = _commandQueue.ElementAt(_commandIdx--);
                    }
                    _input.Remove(_input.Length - 1);
                }
                else if (_input.Contains("]"))
                {
                    if(_commandIdx < 50 - 1 && _commandIdx < _commandList.Count - 1)
                    {
                        Debug.Log(_commandIdx);
                        _input = _commandQueue.ElementAt(_commandIdx++);
                    }
                    _input.Remove(_input.Length - 1);
                }
            }
            y += 30.0f;

            if(_bShowError)
            {
                GUI.contentColor = Color.red;
                GUI.Label(new Rect(5.0f, y, Screen.width - 20.0f, 20), _error);
            }
        }

        //////////////////////////////////////////////////
        public void CreateCommand(object command, string cmd, string desc, Action function)
        {
            if(command == null)
            {
                command = new DebugCommand(cmd, desc, cmd, function);
                AddCommand(command);
            }
        }

        //////////////////////////////////////////////////
        public void CreateCommandInt(object command, string cmd, string desc, string format, Action<int> function)
        {
            if(command == null)
            {
                command = new DebugCommand<int>(cmd, desc, format, function);
                AddCommand(command);
            }
        }


        //////////////////////////////////////////////////
        public void AddCommand(object command)
        {
            if(!(command is DebugCommandBase))
            {
                return;
            }

            //We do not want to add in a command to the list that has already been added
            foreach(object obj in _commandList)
            {
                if (obj is DebugCommandBase)
                {
                    if((obj as DebugCommandBase).CommandId == (command as DebugCommandBase).CommandId)
                    {
                        return;
                    }
                }
            }
            _commandList.Add(command);
        }

        //////////////////////////////////////////////////
        private void OnEnter()
        {
            if (_bShowConsole)
            {
                HandleInput();
                _input = "";
            }
        }

        //////////////////////////////////////////////////
        private void DisplayError(string error)
        {
            _bShowError = true;
            _error = error;
        }

        //////////////////////////////////////////////////
        private void Autocorrect()
        {
            string bestMatch = "";
            int bestMatchCount = 0;

            _input = _input.Remove(_input.IndexOf("?"));

            //Loop through our command list 
            foreach (object obj in _commandList)
            {
                //Convert to a DebugCommand
                DebugCommandBase commandBase = obj as DebugCommandBase;
                if (commandBase != null)
                {
                    //Does the command contain our input
                    if (commandBase.CommandId.ContainsInsensitive(_input))
                    {
                        //Cycle through the characters of the command and compare against best match
                        string cmd = commandBase.CommandId;
                        int matchCount = 0;

                        for(int cmdIdx = 0; cmdIdx < cmd.Length; cmdIdx++)
                        {
                            if (cmdIdx >= _input.Length)
                            {
                                break;
                            }

                            if (cmd[cmdIdx] == _input[cmdIdx])
                            {
                                matchCount++;
                            }
                        }

                        if(matchCount > bestMatchCount)
                        {
                            bestMatchCount = matchCount;
                            bestMatch = cmd;
                        }
                    }
                }
            }

            if(bestMatch != "")
            {
                _input = bestMatch + " ";
            }
        }

        //////////////////////////////////////////////////
        private void HandleInput()
        {
            //Input Filtering to avoid errors
            if (_input.Contains("#"))
            {
                int index = _input.IndexOf("#");
                _input = _input.Remove(index);
            }
            _input = _input.ToLower();
            _commandQueue.Enqueue(_input);
            if(_commandQueue.Count > 50)
            {
                _commandQueue.Dequeue();
            }
            _commandIdx = _commandQueue.Count - 1;

            foreach(object obj in _commandList)
            {
                DebugCommandBase commandBase = obj as DebugCommandBase;
                if (commandBase != null)
                {
                    if (_input.ContainsInsensitive(commandBase.CommandId))
                    {
                        if (commandBase is DebugCommand)
                        {
                            (commandBase as DebugCommand).Invoke();
                            _input = "";
                            _bShowError = false;
                        }
                        else
                        {
                            string[] properties = _input.Split(' ');
                            if(properties.Length >= 2)
                            {
                                if (commandBase is DebugCommand<int>)
                                {
                                    int num;
                                    if (int.TryParse(properties[1], out num))
                                    {
                                        (commandBase as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                                        _bShowError = false;
                                    }
                                    else
                                    {
                                        DisplayError($"Invalid Paramater {properties[1]}");
                                    }
                                    _input = "";
                                }
                            }
                            else
                            {
                                DisplayError("Command Expects a paramater");
                            }
                        }
                    }
                }
            }

            if(_input.Length > 0)
            {
                DisplayError($"Invalid Command {_input}");
            }
        }
    }
}
