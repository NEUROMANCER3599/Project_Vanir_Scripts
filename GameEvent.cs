using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listOfGameEventListener = new();
    public void Raise()
    {
        foreach (var listener in listOfGameEventListener)
        {
            listener.OnEventRaised();
        }
    }
    public void RegisterListener(GameEventListener gameEventListener)
    {
        listOfGameEventListener.Add(gameEventListener);
    }
    public void UnregisterListener(GameEventListener gameEventListener)
    {
        listOfGameEventListener.Remove(gameEventListener);
    }
}
