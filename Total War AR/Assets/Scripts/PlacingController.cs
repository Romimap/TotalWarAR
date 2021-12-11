using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;



public class PlacingController : MonoBehaviour {

    public static PlacingController Singleton;
    public ARRaycastManager raycastManager;
    public ARAnchorManager anchorManager;
    public Camera arcamera;

    public GameObject anchorPrefab;

    ARAnchor anchor = null;
    ARPlane trackedPlane = null;

    GameObject[] grabbed;
    public GameObject ToPlace = null;

    public GameObject UIAnchor;
    public GameObject UIAfterAnchor;

    UnityEngine.XR.ARSubsystems.TrackableId GetPlaneID () {
        ARPlane p = trackedPlane;
            while (p.subsumedBy != null) {
                p = p.subsumedBy;
            }
        trackedPlane = p;
        return trackedPlane.trackableId;
    }

    // Start is called before the first frame update
    void Start() {
        Singleton = this;
        grabbed = new GameObject[100];
    }

    private bool IsPointerOverUIObject (Vector2 screenPoint) {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPoint;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Update is called once per frame
    void Update() {
        if (Input.touchCount > 0) {
            foreach (Touch t in Input.touches) {
                Vector2 screenPoint = t.position;
                int id = t.fingerId;
                if (!IsPointerOverUIObject(screenPoint)) {
                    Debug.Log("Finger " + id + " @ " + screenPoint);
                    if (t.phase == TouchPhase.Began) {
                        if (anchor == null) {
                            PlaceAnchor(screenPoint); //Place Anchor
                        } else {
                            grabbed[id] = Grab(screenPoint); //Grab
                            if (grabbed[id] == null) { 
                                Place(screenPoint, ToPlace); //Place if nothing grabed
                            }
                        }
                    } else if (t.phase == TouchPhase.Ended) { 
                        grabbed[id] = null; //Release
                    } else if (t.phase == TouchPhase.Moved && grabbed[id] != null) {
                        Move(screenPoint, grabbed[id]); //Move
                    }
                }
            }
        }
    }

    GameObject Grab(Vector2 screenPoint) {
        Debug.Log("GRAB");
        Ray ray = arcamera.ScreenPointToRay(screenPoint);

        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPoint, results, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            ARRaycastHit hit = results[0];
            if (hit.trackableId.Equals(GetPlaneID())) {
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit)) {
                    if (raycastHit.collider.gameObject.GetComponent<Grabable>() != null) {
                        return raycastHit.collider.gameObject;
                    }
                }
            }
        }

        return null;
    }

    void Move (Vector2 screenPoint, GameObject gameObject) {
        Debug.Log("MOVE");
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPoint, results, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            foreach(ARRaycastHit hit in results) {
                if (hit.trackableId.Equals(GetPlaneID())) {
                    gameObject.transform.position = hit.pose.position;
                    break;
                }
            }
        }
    }

    void PlaceAnchor(Vector2 screenPoint) {
        Debug.Log("PLACE ANCHOR");
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPoint, results, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            ARRaycastHit hit = results[0];
            if (anchor != null) Destroy(anchor.gameObject);
            trackedPlane = (ARPlane)hit.trackable;
            anchor = anchorManager.AttachAnchor(trackedPlane, hit.pose);

            //Animate UI
            StartCoroutine("AnimateUI");

            GameObject go = Instantiate(anchorPrefab);
            go.transform.parent = anchor.transform;
            go.transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator AnimateUI () {
        PanelViewer.HideAll();
        yield return new WaitForSeconds(1);
        UIAnchor.SetActive(false);
        UIAfterAnchor.SetActive(true);
        yield return new WaitForEndOfFrame();
        PanelViewer.OpenAnimationAll();
    }

    void Place(Vector2 screenPoint, GameObject gameObject) {
        if (gameObject == null) return;
        Debug.Log("PLACE");
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPoint, results, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            ARRaycastHit hit = results[0];
            if (hit.trackableId.Equals(GetPlaneID())) {
                GameObject toPlace = Instantiate(gameObject);
                toPlace.transform.parent = anchor.transform;
                toPlace.transform.position = hit.pose.position;
            }
            
        }
    }

    public void SetObjectToPlace (GameObject go) {
        ToPlace = go;
    }

    public void ResetAll () {
        Destroy(anchor.gameObject);
        anchor = null;
        StartCoroutine("AnimateUIReset");
        Unit.s_units.Clear();
    }


    IEnumerator AnimateUIReset () {
        PanelViewer.HideAll();
        yield return new WaitForSeconds(1);
        UIAnchor.SetActive(true);
        UIAfterAnchor.SetActive(false);
        yield return new WaitForEndOfFrame();
        PanelViewer.OpenAnimationAll();
    }
}
