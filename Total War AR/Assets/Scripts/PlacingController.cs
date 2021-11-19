using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacingController : MonoBehaviour {
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;

    ARAnchor anchor = null;

    void Input() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Place (Vector2 screenPosition) {
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPosition, results)) {

        }
    }
}
