using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    int health = 2;
    int value = 2;

    void Update()
    {
        
    }

    public void ReceiveHit(int power)
    {
        health -= power;
        if (!IsAlive())
            Die();
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        GameEvents.self.IncreasedScore(value);
        Destroy(this.gameObject);
    }
}
