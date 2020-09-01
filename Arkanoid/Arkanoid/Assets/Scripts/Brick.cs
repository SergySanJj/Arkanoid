using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    int health = 2;
    int points = 2;

    [SerializeField] private List<Material> materials = new List<Material>();

    private void Start()
    {
        ChooseMaterial();
    }

    public void SetHealth(int value)
    {

        if (value > materials.Count)
        {
            health = materials.Count;
        }
        else
        {
            health = value;
        }
        points = health;
        ChooseMaterial();
    }

    public void ChooseMaterial()
    {
        if (health > materials.Count)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[0];
        } else
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[health-1];
        }
    }


    void Update()
    {
        
    }

    public void ReceiveHit(int power)
    {
        health -= power;
        if (!IsAlive())
            Die();
        else
        {
            ChooseMaterial();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        GameEvents.self.IncreasedScore(points);
        GameState.self.BrickDestroyed();
        Destroy(this.gameObject);
    }
}
