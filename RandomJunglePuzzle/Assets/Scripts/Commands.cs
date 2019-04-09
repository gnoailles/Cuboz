using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{ 
    void Execute(GridMovement p_gridMovement);
}

public class MoveForwardCommand : ICommand
{
    void ICommand.Execute(GridMovement p_gridMovement)
    {
        p_gridMovement.MoveForward();
        Debug.Log("Move Forward");
    }
}

public class MoveBackwardCommand : ICommand
{
    void ICommand.Execute(GridMovement p_gridMovement)
    {
        //p_gridMovement.MoveForward();
        Debug.Log("Move Backward");
    }
}