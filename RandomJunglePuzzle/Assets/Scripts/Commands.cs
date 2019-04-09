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
        Debug.Log("Move Forward");
    }
}

public class MoveBackwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveBackward();
        Debug.Log("Move Backward");
    }
}

public class MoveLeftCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveLeft();
        Debug.Log("Move Left");
    }
}

public class MoveRightCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.MoveRight();
        Debug.Log("Move Right");
    }
}
public class DashForwardCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashForward();
        Debug.Log("Dash Forward");
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
        Debug.Log("Dash Left");
    }
}

public class DashRightCommand : ICommand
{
    void ICommand.Execute(PlayerController p_gridMovement)
    {
        p_gridMovement.DashRight();
        Debug.Log("Dash Right");
    }
}