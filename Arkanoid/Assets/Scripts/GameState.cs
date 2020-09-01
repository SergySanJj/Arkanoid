using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int bricksLeft { get; set; } = 1;
    public int points { get; set; } = 0;

    public static GameState self = null;
    private GameState()
    {
    }
    void Awake()
    {
        if (self == null)
        {
            self = this;
        }
        else if (self == this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameEvents.self.OnIncreaseScore += IncreacePoints;    
    }

    public void IncreacePoints(int byValue)
    {
        points += byValue;
        GameEvents.self.UpdateUI();
    }

    public void BrickDestroyed()
    {
        bricksLeft--;
        RespawnIfNeeded();
    }

    public void RespawnIfNeeded()
    {
        Debug.Log("Remaining " + bricksLeft);
        if (bricksLeft == 0)
        {
            GameEvents.self.IncreaceDifficulty();
            GameEvents.self.SpawnField();
            GameEvents.self.ReturnToStart();
        }
    }
}
