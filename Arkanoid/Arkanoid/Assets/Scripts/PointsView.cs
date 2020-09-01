using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsView : MonoBehaviour
{

    private TextMeshProUGUI textMesh;


    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        GameEvents.self.OnUpdateUI += UpdatePointsView;
    }

    public void UpdatePointsView()
    {
        textMesh.SetText(GameState.self.points.ToString());
    }
}
