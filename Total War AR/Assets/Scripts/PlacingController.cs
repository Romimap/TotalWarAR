using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacingController : MonoBehaviour {
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public ARPlaneManager planeManager;

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

    void Place (Vector2 screenPosition, GameObject prefab) {
        if (anchor == null) {
            Debug.LogError("Set anchor first!");
            return;
        }

        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPosition, results)) {
            foreach (ARRaycastHit result in results) {
                if (result.trackableId == anchor.trackableId) {
                    GameObject instance = Instantiate(prefab);
                    instance.transform.parent = anchor.transform;
                    instance.transform.position = result.pose.position;
                    break;
                }
            }
        }
    }

    void SetAnchor (Vector2 screenPosition) {
        if (anchor != null) {
            Debug.LogError("Anchor already set !");
            return;
        }

        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPosition, results)) {
            ARTrackable trackable = results[0].trackable;
            Pose pose = results[0].pose;
            if (trackable is ARPlane) {
                anchor = anchorManager.AttachAnchor((ARPlane)trackable, pose);
                planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None; //Stop plane detection as soon as we have our anchor
            }
        }
    }
}
