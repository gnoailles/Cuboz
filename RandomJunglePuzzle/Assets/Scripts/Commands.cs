using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{ 
    void Execute(PlayerController p_gridMovement);
}

public class MoveForwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveForward();
    }
}

public class MoveBackwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveBackward();
    }
}

public class MoveLeftCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveLeft();
    }
}

public class MoveRightCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveRight();
    }
}
public class DashForwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashForward();
    }
}

public class DashBackwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashBackward();
        Debug.Log("Dash Backward");
    }
}

public class DashLeftCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashLeft();
    }
}

public class DashRightCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashRight();
    }
}


public class LightSwitchCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        Light[] lights = GameObject.FindObjectsOfType<Light>();
        foreach(Light light in lights)
        {
            light.enabled = !light.enabled;
        }
    }
}