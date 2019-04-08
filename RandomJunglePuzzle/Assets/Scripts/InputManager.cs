using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{

    private Dictionary<string, ICommand>    m_axisCommands;
    private Dictionary<string, ICommand>    m_buttonCommands;
    public  List<ICommand>                  m_commands = new List<ICommand>();
    

    private void UpdateCommands()
    {
        m_commands.Clear();
        System.Reflection.Assembly assembly     = typeof(ICommand).Assembly;
        List<System.Type> types                 = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand))).ToList();

        foreach (System.Type type in types)
        {
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
