using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private const ushort                    m_movementCount         = 4;

    public  PlayerController                player                  = null;
    public  bool                            randomizeInputs         = true;

    private ushort                          m_axisCount             = 4;
    private ushort                          m_dualAxisCount         = 2;
    private bool[]                          m_isAxisDown;
    private bool[]                          m_isMovementSwapped;
    private Dictionary<ushort, ICommand>    m_commands              = new Dictionary<ushort, ICommand>();
    private List<string>                    m_inputs                = new List<string>(new string[] {   "HorizontalDPad", "VerticalDPad",
                                                                                                        "LeftTrigger", "RightTrigger", 
                                                                                                        "A", "B", "X", "Y", 
                                                                                                        "LeftButton", "RightButton" });

    /// INPUT ID                |       COMMAND ID                          
    /// Axis                    |       Axis
    /// 0   HorizontalDPad      |       0   Negative HorizontalDPad
    /// 1   VerticalDPad        |       1   Negative VerticalDPad
    ///                         |       2   Positive HorizontalDPad
    ///                         |       3   Positive VerticalDPad
    ///                         |       
    /// 2   LeftTrigger         |       4   LeftTrigger
    /// 3   RightTrigger        |       5   RightTrigger
    ///                         |       
    /// Buttons                 |       Buttons
    /// 4   A                   |       6   A
    /// 5   B                   |       7   B
    /// 6   X                   |       8   X
    /// 7   Y                   |       9   Y
    ///                         |       
    /// 8  LeftButton           |       10  LeftButton
    /// 9  RightButton          |       11  RightButton
    /// 

    InputManager()
    {
        m_isAxisDown            = new bool[m_axisCount];
        m_isMovementSwapped    = new bool[m_movementCount];
    }

    private void UpdateCommands()
    {
        System.Array.Clear(m_isAxisDown, 0, m_isAxisDown.Length);
        if (randomizeInputs)
        { 
            m_commands.Clear();
            System.Reflection.Assembly assembly     = typeof(ICommand).Assembly;
            List<System.Type> types                 = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand))).ToList();
            foreach (System.Type type in types)
            {
                ushort commandID = (ushort)Random.Range(0, m_inputs.Count + m_dualAxisCount);
                while(m_commands.ContainsKey(commandID))
                {
                    commandID = (ushort)Random.Range(0, m_inputs.Count + m_dualAxisCount);
                }

                if (commandID < m_dualAxisCount)
                {
                    Debug.Log("Command: " + type.Name + " bound to Input: " + m_inputs[commandID] + " with index: " + commandID);
                }
                else
                {
                    Debug.Log("Command: " + type.Name + " bound to Input: " + m_inputs[commandID - m_dualAxisCount] + " with index: " + commandID);
                }
                m_commands.Add(commandID, (ICommand)System.Activator.CreateInstance(type));
            }
        }
        else
        {
            m_commands.Add(0,  new MoveLeftCommand());
            m_commands.Add(1,  new MoveBackwardCommand());
            m_commands.Add(2,  new MoveRightCommand());
            m_commands.Add(3,  new MoveForwardCommand());

            m_commands.Add(6,  new DashBackwardCommand());
            m_commands.Add(7,  new DashRightCommand());
            m_commands.Add(8,  new DashLeftCommand());
            m_commands.Add(9,  new DashForwardCommand());

            m_commands.Add(10,  new LightSwitchCommand());
        }
    }

    void Start()
    {
        UpdateCommands();
    }

    [ContextMenu("Reset Movements")]
    void ResetMovements()
    {
        for(short i = 0; i < m_isMovementSwapped.Length; ++i)
        { 
            if (m_isMovementSwapped[i])
            {
                SwapMovement(i);
            }
            Debug.Log("Movement " + i + " swapped: " + m_isMovementSwapped[i]);
        }
    }

    [ContextMenu("Swap Movements")]
    public void SwapRandomMovement()
    {
        short movement = (short)(Random.Range(0, 4));
        SwapMovement(movement);

        for (short i = 0; i < m_isMovementSwapped.Length; ++i)
        {
            Debug.Log("Movement " + i + " swapped: " + m_isMovementSwapped[i]);
        }
    }

    void SwapMovement(short p_movement)
    {

        switch (p_movement)
        {
            case 0:
            { 
                var forwardPair = m_commands.First(x => x.Value.GetType() == typeof(MoveForwardCommand));
                var backwardPair = m_commands.First(x => x.Value.GetType() == typeof(MoveBackwardCommand));
                m_commands[forwardPair.Key] = backwardPair.Value;
                m_commands[backwardPair.Key] = forwardPair.Value;
                break;
            }
            case 1:
            { 
                var forwardPair = m_commands.First(x => x.Value.GetType() == typeof(MoveRightCommand));
                var backwardPair = m_commands.First(x => x.Value.GetType() == typeof(MoveLeftCommand));
                m_commands[forwardPair.Key] = backwardPair.Value;
                m_commands[backwardPair.Key] = forwardPair.Value;
                break;
            }
            case 2:
            { 
                var forwardPair = m_commands.First(x => x.Value.GetType() == typeof(DashForwardCommand));
                var backwardPair = m_commands.First(x => x.Value.GetType() == typeof(DashBackwardCommand));
                m_commands[forwardPair.Key] = backwardPair.Value;
                m_commands[backwardPair.Key] = forwardPair.Value;
                break;
            }
            case 3:
            { 
                var forwardPair = m_commands.First(x => x.Value.GetType() == typeof(DashRightCommand));
                var backwardPair = m_commands.First(x => x.Value.GetType() == typeof(DashLeftCommand));
                m_commands[forwardPair.Key] = backwardPair.Value;
                m_commands[backwardPair.Key] = forwardPair.Value;
                break;
            }
            default:
            {
                throw new UnassignedReferenceException("Invalid Movement");
            }
        }
        m_isMovementSwapped[p_movement] = !m_isMovementSwapped[p_movement];
    }

    void Update()
    {
        for (ushort inputID = 0; inputID < m_inputs.Count; ++inputID)
        {
            ushort commandID = inputID;
            if (inputID < m_dualAxisCount)
            {
                float value = Input.GetAxis(m_inputs[inputID]);
                if(value == 0.0f)
                {
                    m_isAxisDown[inputID] = false;
                }
                else if(value < 0 && !m_isAxisDown[inputID] && m_commands.ContainsKey(commandID))
                {
                    m_commands[commandID].Execute(player);
                    m_isAxisDown[inputID] = true;
                }
                else if (value > 0 && !m_isAxisDown[inputID] && m_commands.ContainsKey((ushort)(commandID + m_dualAxisCount)))
                {
                    m_commands[(ushort)(commandID + m_dualAxisCount)].Execute(player);
                    m_isAxisDown[inputID] = true;
                }
            }
            else
            {
                commandID += m_dualAxisCount;
                if (inputID >= m_dualAxisCount && inputID < m_axisCount)
                {
                    float value = Input.GetAxis(m_inputs[inputID]);
                    if (value < 0.0f)
                    {
                        throw new System.Exception("Dual axis not handled");
                    }
                    if (value == 0.0f)
                    {
                        m_isAxisDown[inputID] = false;
                    }
                    else if (!m_isAxisDown[inputID] && m_commands.ContainsKey(commandID))
                    {
                        m_commands[commandID].Execute(player);
                        m_isAxisDown[inputID] = true;
                    }
                }
                else if (inputID >= m_axisCount && inputID < m_inputs.Count)
                {
                    if(Input.GetButtonDown(m_inputs[inputID]) && m_commands.ContainsKey(commandID))
                    {
                        m_commands[commandID].Execute(player);
                    }
                }
                else
                {
                    throw new System.IndexOutOfRangeException("Unhandled input");
                }
            }
        }
    }
}
