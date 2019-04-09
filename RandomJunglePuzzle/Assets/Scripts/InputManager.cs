using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{

    public  GridMovement                    player          = null;
    private ushort                          m_axisCount     = 4;
    private ushort                          m_dualAxisCount = 2;
    private bool[]                          m_axisDown;
    private Dictionary<ushort, ICommand>    m_commands      = new Dictionary<ushort, ICommand>();
    private List<string>                    m_inputs        = new List<string>(new string[] {   "HorizontalDPad", "VerticalDPad",
                                                                                                "LeftTrigger", "RightTrigger", 
                                                                                                "A", "B", "X", "Y", 
                                                                                                "LeftButton", "RightButton" });
    

    private void UpdateCommands()
    {
        m_commands.Clear();
        System.Reflection.Assembly assembly     = typeof(ICommand).Assembly;
        List<System.Type> types                 = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand))).ToList();
        foreach (System.Type type in types)
        {
            ushort inputIndex = (ushort)Random.Range(0, m_inputs.Count + m_dualAxisCount - 1);
            while(m_commands.ContainsKey(inputIndex))
            {
                inputIndex = (ushort)Random.Range(0, m_inputs.Count + m_dualAxisCount - 1);
            }

            if (inputIndex < m_dualAxisCount)
            {
                Debug.Log("Command: " + type.Name + " bound to Input: " + m_inputs[inputIndex] + " with index: " + inputIndex);
            }
            else
            {
                Debug.Log("Command: " + type.Name + " bound to Input: " + m_inputs[inputIndex - m_dualAxisCount] + " with index: " + inputIndex);
            }
            m_commands.Add(inputIndex, (ICommand)System.Activator.CreateInstance(type));
        }
    }

    void Start()
    {
        m_axisDown = new bool[m_axisCount];
        UpdateCommands();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (ushort i = 0; i < m_inputs.Count; ++i)
        {
            if(i < m_axisCount)
            {
                float value = Input.GetAxis(m_inputs[i]);
                if(value == 0.0f)
                {
                    m_axisDown[i] = false;
                }
                else if(value > 0 && !m_axisDown[i] && m_commands.ContainsKey(i))
                {
                    Debug.Log("Input: " + m_inputs[i] + " with index: " + i);
                    m_commands[i].Execute(player);
                    m_axisDown[i] = true;
                }
                else if (value < 0 && !m_axisDown[i] && m_commands.ContainsKey((ushort)(i * 2)))
                {
                    m_commands[(ushort)(i * 2)].Execute(player);
                    m_axisDown[i] = true;
                }
            }
            else
            {
                if(Input.GetButtonDown(m_inputs[i]) && m_commands.ContainsKey(i))
                {
                    Debug.Log(m_inputs[i] + " pressed!");
                    m_commands[i].Execute(player);
                }
            }
        }
    }
}
