using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreenDisplay : MonoBehaviour
{
    private CanvasGroup cg;
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        GameEvents.self.OnDeathScreen += Display;
    }

    public void Display(bool display)
    {
        if (display)
            cg.alpha = 1.0f;
        else
            cg.alpha = 0.0f;
    }
}
