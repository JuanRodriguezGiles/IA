using System.Collections.Generic;

using UnityEngine;

public class FSM
{
    #region PRIVATE_FIELDS
    private int currentState;
    private int[,] relations;
    private Dictionary<int, List<FSMAction>> behaviours;
    #endregion

    public FSM(int states, int flags)
    {
        currentState = -1;

        relations = new int[states, flags];
        for (int i = 0; i < states; i++)
        {
            for (int j = 0; j < flags; j++)
            {
                relations[i, j] = -1;
            }
        }

        behaviours = new Dictionary<int, List<FSMAction>>();
    }

    public void ForceCurrentState(int state)
    {
        currentState = state;
    }

    public void SetRelation(int sourceState, int flag, int destinationState)
    {
        relations[sourceState, flag] = destinationState;
    }

    public void SetFlag(int flag)
    {
        if (relations[currentState, flag] != -1)
        {
            currentState = relations[currentState, flag];
        }
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    public void SetBehaviour(int state, FSMAction behaviour)
    {
        List<FSMAction> newBehaviours = new List<FSMAction> { behaviour };

        if (behaviours.ContainsKey(state))
        {
            behaviours[state] = newBehaviours;
        }
        else
        {
            behaviours.Add(state, newBehaviours);
        }
    }

    public void AddBehaviour(int state, FSMAction behaviour)
    {
        if (behaviours.ContainsKey(state))
        {
            behaviours[state].Add(behaviour);
        }
        else
        {
            List<FSMAction> newBehaviours = new List<FSMAction> { behaviour };
            behaviours.Add(state, newBehaviours);
        }
    }

    public void Update()
    {
        if (behaviours.ContainsKey(currentState))
        {
            List<FSMAction> actions = behaviours[currentState];
            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] != null)
                    {
                        actions[i].Execute();
                    }
                }
            }
        }
    }
}