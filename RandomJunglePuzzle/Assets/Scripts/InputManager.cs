using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{

    private List<string>        m_inputs;
    private List<ICommand>      m_commands      = new List<ICommand>();
    private ushort              m_axisCount     = 3;
    

    private void UpdateCommands()
    {
        m_commands.Clear();
        System.Reflection.Assembly assembly     = typeof(ICommand).Assembly;
        List<System.Type> types                 = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand))).ToList();
        //m_commands[0] = (ICommand)System.Activator.CreateInstance(types[0]);
        foreach (System.Type type in types)
        { 
            int inputIndex = Random.Range(0, m_inputs.Count - 1);
            m_commands.Add((ICommand)System.Activator.CreateInstance(type));
        }
    }

    void Start()
    {
        UpdateCommands();
        
        foreach (ICommand command in m_commands)
        {

            Debug.Log(command.GetType().Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
