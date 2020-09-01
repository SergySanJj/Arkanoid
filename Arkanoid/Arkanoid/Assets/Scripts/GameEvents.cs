using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents self = null;
    private GameEvents()
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

    public delegate void IncreaseScore(int byValue);
    public event IncreaseScore OnIncreaseScore;
    public void IncreasedScore(int byValue)
    {
        OnIncreaseScore?.Invoke(byValue);
    }

    public event Action OnUpdateUI;
    public void UpdateUI()
    {
        OnUpdateUI?.Invoke();
    }

    public delegate void DeathScreen(bool display);
    public event DeathScreen OnDeathScreen;
    public void ShowDeathScreen(bool display)
    {
        OnDeathScreen?.Invoke(display);
    }

    public event Action OnStartGame;
    public void Startgame()
    {
        OnStartGame?.Invoke();
    }
}
