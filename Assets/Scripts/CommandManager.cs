using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    private List<ICommand> commands = new List<ICommand>();
    private int currentCommand = 0;

    public void Redo()
    {
        if (currentCommand + 1 <= commands.Count){
            currentCommand ++;
            commands[currentCommand-1].Execute();
        } else {
            Debug.Log("no command to redo");
        }
    }

    public void Undo()
    {
        if (currentCommand > 0){
            currentCommand --;
            commands[currentCommand-1].Undo();
        } else {
            Debug.Log("no command to undo");
        }
    }

    public void Execute(ICommand c){
        if (currentCommand <= commands.Count){
            List<ICommand> tempCommands = new List<ICommand>();
            for (int i =0; i<currentCommand; i++){
                tempCommands.Add(commands[i]);
            }
            commands = tempCommands;
        }
        currentCommand++;
        c.Execute();
        commands.Add(c);
    }

}