using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugController : MonoBehaviour
{
    // STATE
    bool _bShowConsole;
    string _input;
    Vector2 _scrollView;
    bool _bShowHelp = false;
    public List<object> _commandList;


    ///////////////  COMMAND LIST  ///////////////////
    public static DebugCommand KILL_PLAYER;
    public static DebugCommand HELP;
    public static DebugCommand<int> GIVE_GOLD;

    //////////////////////////////////////////////////
    private void Awake()
    {
        KILL_PLAYER = new DebugCommand("kill_player", "Kills the current player", "kill_all", () =>
        {
            PlayerController pc = FindObjectOfType<PlayerController>();
            if(pc)
            {
                Health playerHealth = pc.GetComponent<Health>();
                if(playerHealth)
                {
                    playerHealth.Kill();
                }
            }
        });

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
            KILL_PLAYER,
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

        if(Input.GetKeyDown(KeyCode.Return))
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

            for(int i = 0; i < _commandList.Count; i++)
            {
                DebugCommandBase command = _commandList[i] as DebugCommandBase;

                if(command != null)
                {
                    string label = $"{command.CommandFormat} - {command.CommandDescription} - {command.CommandFormat}";
                    Rect labelRect = new Rect(5.0f, 20.0f * i, viewport.width - 100.0f, 20.0f);

                    GUI.Label(labelRect, label);
                }
            }
            y += 100.0f;

            GUI.EndScrollView();
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
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

        for(int i  = 0; i < _commandList.Count; i++)
        {
            DebugCommandBase commandBase = _commandList[i] as DebugCommandBase;
            if (commandBase != null)
            {
                if (_input.Contains(commandBase.CommandId))
                {
                    if(commandBase as DebugCommand != null)
                    {
                        (commandBase as DebugCommand).Invoke();
                        _input = "";
                    }
                    else if(commandBase as DebugCommand<int> != null)
                    {
                        (commandBase as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                        _input = "";
                    }
                    else if(commandBase as DebugCommand<bool> != null)
                    {
                        (commandBase as DebugCommand<bool>).Invoke(bool.Parse(properties[1]));
                        _input = "";
                    }
                }
            }
        }
    }
}
