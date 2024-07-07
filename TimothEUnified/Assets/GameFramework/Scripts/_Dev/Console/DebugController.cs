using System.Collections.Generic;
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

            GIVE_GOLD = new DebugCommand<int>("give_gold", "Gives <x> gold to the current player", "give_gold <desiredAmount>", (int x) =>
            {
                Debug.Log(x);
            });

            HELP = new DebugCommand("help", "Toggles helpful description of all commands to the developer", "help", () =>
            {
                _bShowHelp = !_bShowHelp;
            });

            _commandList = new List<object>()
        {
            HELP,
            GIVE_GOLD
        };
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
            }
            y += 30.0f;

            if(_bShowError)
            {
                GUI.contentColor = Color.red;
                GUI.Label(new Rect(5.0f, y, Screen.width - 20.0f, 20), _error);
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
        private void HandleInput()
        {
            //Input Filtering to avoid errors
            if (_input.Contains("#"))
            {
                int index = _input.IndexOf("#");
                _input = _input.Remove(index);
            }
            _input = _input.ToLower();

            string[] properties = _input.Split(' ');

            for (int i = 0; i < _commandList.Count; i++)
            {
                DebugCommandBase commandBase = _commandList[i] as DebugCommandBase;
                if (commandBase != null)
                {
                    if (_input.Contains(commandBase.CommandId))
                    {
                        if (commandBase is DebugCommand)
                        {
                            (commandBase as DebugCommand).Invoke();
                            _input = "";
                            _bShowError = false;
                        }
                        else if (commandBase is DebugCommand<int>)
                        {
                            if(properties.Length >= 2)
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
                            else
                            {
                                DisplayError("Command expects a paramater");
                            }
                        }
                        else if (commandBase is DebugCommand<bool>)
                        {
                            if(properties.Length >= 2)
                            {
                                bool val;
                                if (bool.TryParse(properties[1], out val))
                                {
                                    (commandBase as DebugCommand<bool>).Invoke(bool.Parse(properties[1]));
                                    _bShowError = false;
                                }
                                else
                                {
                                    DisplayError($"Invalid Paramater {properties[1]}");
                                }
                                _input = "";
                            }
                            else
                            {
                                DisplayError("Command expects a paramater");
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
