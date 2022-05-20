using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Seance.CheatConsole
{
    public class ConsoleManager : Singleton<ConsoleManager>
    {
        [SerializeField] private bool _enableConsole = false;
        
        private bool _showConsole = false;
        private int _scrollIndex;
        private List<string> _commandHistory;
        private string _ongoingCommand;
        private List<GameObject> _messageList;

        private static ConsoleCommand HELLO;
        private static ConsoleCommand HELP;
        private static ConsoleCommand<string, string[]> ALIAS;
        private static ConsoleCommand<string, float, float, float> CAMERA_VECT3;
        private static ConsoleCommand<string, float> CAMERA_AXIS;
        private List<ConsoleBaseCommand> _commandList;
        private Dictionary<string, string> _commandAlias;

        [Header("Inputs")]
        [SerializeField] private KeyCode _bindOpen;
        [SerializeField] private KeyCode _bindClose;
        [SerializeField] private KeyCode _bindSend;
        [SerializeField] private KeyCode _bindPreviousCommand;
        [SerializeField] private KeyCode _bindNextCommand;

        [Header("Colors")]
        [SerializeField] private Color _traceColor;
        [SerializeField] private Color _logColor;
        [SerializeField] private Color _infoColor;
        [SerializeField] private Color _successColor;
        [SerializeField] private Color _warnColor;
        [SerializeField] private Color _errorColor;

        [Header("Components")]
        [SerializeField] private CanvasGroup _consoleGroup;
        [SerializeField] private RectTransform _scrollviewContent;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private GameObject _commandMessageTemplate;

        #region Unity events

        private void Awake()
        {
            CreateSingleton();

            _consoleGroup.alpha = 0;
            _consoleGroup.interactable = false;
            _consoleGroup.blocksRaycasts = false;

            _commandHistory = new List<string>();
            _messageList = new List<GameObject>();
            _scrollIndex = 0;

            HELLO = new ConsoleCommand("hello", "Say hello", "hello", () => CommandHello());
            HELP = new ConsoleCommand("help", "Display list of all available commands", "help", () => CommandHelp());
            ALIAS = new ConsoleCommand<string, string[]>("alias", "Create shorten command for a preregistered command", "alias <name> <subcommand>...|delete", (a, b) => CommandAlias(a, b));
            CAMERA_VECT3 = new ConsoleCommand<string, float, float, float>("camera", "Move or rotate the main camera", "camera <pos|rot> <x> <y> <z>", (a, x, y, z) => CommandCameraVect3(a, x, y, z));
            CAMERA_AXIS = new ConsoleCommand<string, float>("camera", "Move or rotate the main camera along a specific axis", "camera <up|down|left|right|forward|pitch|roll|yaw> <units>", (a, x) => CommandCameraAxis(a, x));


            _commandList = new List<ConsoleBaseCommand>()
            {
                HELLO,
                HELP,
                ALIAS,
                CAMERA_VECT3,
                CAMERA_AXIS
            };
            _commandAlias = new Dictionary<string, string>();
        }

        private void Update()
        {
            if (!_enableConsole) return;


            // OnOpenConsole
            if(Input.GetKeyDown(_bindOpen))
            {
                if(!_showConsole)
                {
                    _showConsole = true;
                    _consoleGroup.alpha = 1;
                    _consoleGroup.interactable = true;
                    _consoleGroup.blocksRaycasts = true;
                    _inputField.ActivateInputField();
                }
            }
            
            // OnCloseConsole
            if(Input.GetKeyDown(_bindClose))
            {
                if(_showConsole)
                {
                    _showConsole = false;
                    _consoleGroup.alpha = 0;
                    _consoleGroup.interactable = false;
                    _consoleGroup.blocksRaycasts = false;
                    _inputField.DeactivateInputField();
                }
            }

            // OnSendCommand
            if(Input.GetKeyDown(_bindSend))
            {
                if(_showConsole && !_inputField.text.Equals(""))
                {
                    HandleInput();
                    _commandHistory.Add(_inputField.text);
                    _scrollIndex = _commandHistory.Count;
                    _inputField.text = "";
                    _ongoingCommand = "";
                    _inputField.ActivateInputField();
                }
                else if(_showConsole && _inputField.text.Equals(""))
                {
                    _inputField.ActivateInputField();
                }
            }

            //OnScrollCommand
            if(Input.GetKeyDown(_bindPreviousCommand) || Input.GetKeyDown(_bindNextCommand))
            {
                int sign = Input.GetKeyDown(_bindNextCommand) ? -1 : 1;

                if(_scrollIndex == _commandHistory.Count)
                {
                    _ongoingCommand = _inputField.text;
                }

                _scrollIndex = Mathf.Clamp(_scrollIndex - sign, 0, _commandHistory.Count);
                if(_scrollIndex < _commandHistory.Count)
                {
                    _inputField.text = _commandHistory[_scrollIndex];
                    _inputField.MoveTextEnd(false);
                }
                else
                {
                    _inputField.text = _ongoingCommand;
                    _inputField.MoveTextEnd(false);
                }
                
            }
        }

        #endregion

        #region Public methods

        public void PrintConsole(LogLevel level, string message)
        {
            GameObject instance = Instantiate(_commandMessageTemplate, _scrollviewContent);
            TextMeshProUGUI textComponent = instance.GetComponent<TextMeshProUGUI>();
            
            textComponent.text = message;
            switch (level)
            {
                case LogLevel.TRACE: 
                    textComponent.color = _traceColor; 
                    break;

                case LogLevel.LOG: 
                    textComponent.color = _logColor; 
                    break;

                case LogLevel.INFO: 
                    textComponent.color = _infoColor; 
                    break;

                case LogLevel.SUCCESS: 
                    textComponent.color = _successColor; 
                    break;

                case LogLevel.WARN: 
                    textComponent.color = _warnColor; 
                    break;

                case LogLevel.ERROR: 
                    textComponent.color = _errorColor; 
                    break;
            }
            _messageList.Add(instance);
        }

        #endregion

        #region Private methods

        private void HandleInput()
        {
            string[] args = _inputField.text.Split(' ');
            bool commandFound = false;
            for (int i = 0; i < _commandList.Count && !commandFound; i++)
            {
                ConsoleBaseCommand commandBase = _commandList[i] as ConsoleBaseCommand;
                if (args[0].Equals(commandBase.CommandHead))
                {
                    // 0 arg
                    if (_commandList[i] as ConsoleCommand != null)
                    {
                        (_commandList[i] as ConsoleCommand).Invoke();
                        commandFound = true;
                    }

                    // 2 args
                    // --- string, float
                    else if (_commandList[i] as ConsoleCommand<string, float> != null)
                    {
                        try
                        {
                            (_commandList[i] as ConsoleCommand<string, float>).Invoke(
                                args[1],
                                float.Parse(args[2], CultureInfo.InvariantCulture));
                            commandFound = true;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    // --- string, string[]
                    else if (_commandList[i] as ConsoleCommand<string, string[]> != null)
                    {
                        string[] mergedArgs = new string[Mathf.Max(0,args.Length-2)];
                        for(int a = 2; a<args.Length; a++)
                        {
                            mergedArgs[a - 2] = args[a];
                        }

                        try
                        {
                            (_commandList[i] as ConsoleCommand<string, string[]>).Invoke(
                                args[1],
                                mergedArgs);
                            commandFound = true;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    // 4 args
                    else if (_commandList[i] as ConsoleCommand<string,float,float,float> != null)
                    {
                        try
                        {
                            (_commandList[i] as ConsoleCommand<string, float, float, float>).Invoke(
                                args[1], 
                                float.Parse(args[2], CultureInfo.InvariantCulture), 
                                float.Parse(args[3], CultureInfo.InvariantCulture),
                                float.Parse(args[4], CultureInfo.InvariantCulture));
                            commandFound = true;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    // Add new command condition if dont exist yet, shitty part...
                    /*
                    else if(_commandList[i] as ConsoleCommand<T1,T2...> != null)
                    {
                        try
                        {
                            (_commandList[i] as ConsoleCommand<T1,T2...>).Invoke(T1.Parse(args[1]), T2.Parse(args[1])...);
                            commandFound = true;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    */
                }
            }

            foreach(string k in _commandAlias.Keys)
            {
                if(args[0].Equals(k))
                {
                    _inputField.text = _commandAlias[k];
                    HandleInput();
                    commandFound = true;
                }
            }

            if(!commandFound)
            {
                PrintConsole(LogLevel.ERROR, "An error has occured while executing the command");
            }
        }

        private void CommandHello()
        {
            PrintConsole(LogLevel.SUCCESS, "Hello !");
        }

        private void CommandHelp()
        {
            string strOutput = "---------- HELP ----------\n";
            foreach (ConsoleBaseCommand cmd in _commandList)
            {
                strOutput +=
                    $"{cmd.CommandHead} :\n" +
                    $"Format : {cmd.CommandUsage}\n" +
                    $"{cmd.CommandDescription}\n\n";
            }
            PrintConsole(LogLevel.INFO, strOutput);
        }

        private void CommandAlias(string a, string[] b)
        {
            string strOutput;

            if(b.Length > 0)
            {
                if(b[0].Equals("delete"))
                {
                    if(_commandAlias.ContainsKey(a))
                    {
                        string subcommand = _commandAlias[a];
                        _commandAlias.Remove(a);
                        strOutput = $"Alias '{a}' deleted.\nSubcommand: {subcommand}";
                        PrintConsole(LogLevel.SUCCESS, strOutput);
                    }
                    else
                    {
                        strOutput = $"Alias name '{a}' is unknown.";
                        PrintConsole(LogLevel.WARN, strOutput);
                    }
                }
                else
                {
                    if(_commandAlias.ContainsKey(a))
                    {
                        strOutput = $"Alias '{a}' already exist. Use another alias name or use 'alias {a} delete' to remove existing alias";
                        PrintConsole(LogLevel.WARN, strOutput);
                    }
                    else
                    {
                        string subcommand = b[0];
                        for(int i = 1; i < b.Length; i++)
                        {
                            subcommand = subcommand + " " + b[i];
                        }
                        _commandAlias.Add(a, subcommand);
                        strOutput = $"Alias '{a}' created.\nSubcommand: {subcommand}";
                        PrintConsole(LogLevel.SUCCESS, strOutput);
                    }
                }
            }
            else
            {
                strOutput = $"'alias' require a subcommand. See command usage.";
                PrintConsole(LogLevel.ERROR, strOutput);
            }
        }

        private void CommandCameraVect3(string a, float x, float y, float z)
        {
            GameObject goCamera = GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 initPosition = goCamera.transform.position;
            Vector3 initRotation = goCamera.transform.eulerAngles;
            string strOutput;

            if (a.Equals("pos"))
            {
                goCamera.transform.position = new Vector3(x, y, z);
                strOutput = $"Move camera to position ({x}, {y}, {z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                PrintConsole(LogLevel.SUCCESS, strOutput);
            }
            else if (a.Equals("rot"))
            {
                goCamera.transform.eulerAngles = new Vector3(x, y, z);
                strOutput = $"Rotate camera to angle ({x}, {y}, {z}).\nLast rotation: ({initRotation.x}, {initRotation.y}, {initRotation.z})";
                PrintConsole(LogLevel.SUCCESS, strOutput);
            }
            else
            {
                strOutput = $"Argument 1 ({a}) caused an error during command interpretation.\nUse 'pos' or 'rot' instead";
                PrintConsole(LogLevel.ERROR, strOutput);
            }
        }

        private void CommandCameraAxis(string a, float x)
        {
            GameObject goCamera = GameObject.FindGameObjectWithTag("MainCamera");
            Vector3 initPosition = goCamera.transform.position;
            Vector3 initRotation = goCamera.transform.eulerAngles;
            string strOutput;

            switch(a)
            {
                case "up":
                    goCamera.transform.position += Vector3.up * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "down":
                    goCamera.transform.position += Vector3.down * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "left":
                    goCamera.transform.position += Vector3.left * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "right":
                    goCamera.transform.position += Vector3.right * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "forward":
                    goCamera.transform.position += Vector3.forward * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "back":
                    goCamera.transform.position += Vector3.back * x;
                    strOutput = $"Move camera to position ({goCamera.transform.position.x}, {goCamera.transform.position.y}, {goCamera.transform.position.z}).\nLast position: ({initPosition.x}, {initPosition.y}, {initPosition.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;
                    
                case "pitch":
                    goCamera.transform.eulerAngles += Vector3.right * x;
                    strOutput = $"Rotate camera to angle ({goCamera.transform.eulerAngles.x}, {goCamera.transform.eulerAngles.y}, {goCamera.transform.eulerAngles.z}).\nLast rotation: ({initRotation.x}, {initRotation.y}, {initRotation.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "roll":
                    goCamera.transform.eulerAngles += Vector3.forward * x;
                    strOutput = $"Rotate camera to angle ({goCamera.transform.eulerAngles.x}, {goCamera.transform.eulerAngles.y}, {goCamera.transform.eulerAngles.z}).\nLast rotation: ({initRotation.x}, {initRotation.y}, {initRotation.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                case "yaw":
                    goCamera.transform.eulerAngles += Vector3.up * x;
                    strOutput = $"Rotate camera to angle ({goCamera.transform.eulerAngles.x}, {goCamera.transform.eulerAngles.y}, {goCamera.transform.eulerAngles.z}).\nLast rotation: ({initRotation.x}, {initRotation.y}, {initRotation.z})";
                    PrintConsole(LogLevel.SUCCESS, strOutput);
                    break;

                default:
                    strOutput = $"Argument 1 ({a}) caused an error during command interpretation.\nUse 'up', 'down', 'left', 'right', 'forward', 'back', 'pitch', 'roll' or 'yaw' instead";
                    PrintConsole(LogLevel.ERROR, strOutput);
                    break;
            }
        }

        #endregion
    }
}
