using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame () {
        foreach (Unit u in Unit.s_units) {
            u.OnGameStart();
        }
        PlacingController.Singleton.enabled = false;
    }

    public void StopGame () {
        foreach (Unit u in Unit.s_units) {
            u.OnGameStop();
        }
        PlacingController.Singleton.enabled = true;
    }
}
