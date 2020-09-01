using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnable = null;
    [Range(0.1f, 10.0f)]
    [SerializeField] private float offset = 1f;

    [Range(0.0f, 0.3f)]
    [SerializeField] private float marginTop = 0.1f;
    [Range(0.3f, 0.7f)]
    [SerializeField] private float marginBot = 0.3f;
    [Range(0.0f, 0.5f)]
    [SerializeField] private float marginLeft = 0.1f;
    [Range(0.0f, 0.5f)]
    [SerializeField] private float marginRight = 0.1f;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float probability = 1.0f;


    void Start()
    {
        StartCoroutine(FillCorutine());
        Fill();
        GameEvents.self.OnSpawnField += Fill;
        GameEvents.self.OnIncreaceDifficulty += IncreaceDifficulty;
    }

    public IEnumerator FillCorutine()
    {
        yield return new WaitForFixedUpdate();
        Fill();
    }

    public void Fill()
    {
        float height = SceneBoundaries.self.topLeft.y - SceneBoundaries.self.bottomLeft.y;
        float width = SceneBoundaries.self.topRight.x - SceneBoundaries.self.topLeft.x;
        int colCount = (int)(width * (1.0 - marginLeft - marginRight) / (2.0f * offset));
        int rowCount = (int)(height * (1.0 - marginTop - marginBot) / (2.0f * offset));

        GameState.self.bricksLeft = 0;


        float yStart = SceneBoundaries.self.topLeft.y - height * marginTop - offset;
        float xStart = SceneBoundaries.self.topLeft.x + width * marginLeft + offset;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                SpawnWithProbability(new Vector3(xStart + 2 * j * offset, yStart - 2 * i * offset, 0.0f));
            }
        }
    }

    private void SpawnWithProbability(Vector3 pos)
    {
        if (Random.Range(0.0f, 1.0f) <= probability)
        {
            Spawn(pos);
            GameState.self.bricksLeft += 1;
        }
    }

    private void Spawn(Vector3 pos)
    {
        var go = Instantiate(spawnable, pos, Quaternion.identity);
        go.GetComponent<Brick>()?.SetHealth((int)Random.Range(1f, 5f));
    }

    public void IncreaceDifficulty()
    {
        marginBot -= 0.2f;
        if (marginBot > 0.7f)
        {
            marginBot = 0.7f;
        }
    }
}
