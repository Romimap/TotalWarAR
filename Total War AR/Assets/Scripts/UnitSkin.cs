using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkin : MonoBehaviour {
    private Vector3 _previousPos; 
    private float _traveledDistance = 0;
    private static GameObject s_camera = null;
    // Start is called before the first frame update
    void Start() {
        if (s_camera == null) {
            s_camera = GameObject.Find("AR Camera");
        }
        _previousPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        BillboardY();
        Animate();
    }

    void BillboardY() {
        transform.LookAt(transform.position + s_camera.transform.rotation * Vector3.forward, s_camera.transform.rotation * Vector3.up);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.z = 0;
        eulerAngles.x = 0;
        transform.eulerAngles = eulerAngles;
    }

    void Animate() {
        _traveledDistance += (_previousPos - transform.position).magnitude * 100;
        _previousPos = transform.position;
        int frame = (int)_traveledDistance % 3;
        float orientation = Vector3.Dot(transform.parent.forward, transform.forward);

        int intOrientation = 0;

        if (orientation > 0.7) {
            intOrientation = 0;
        } else if (orientation > -0.7) {
            if (Vector3.Dot(transform.parent.right, transform.forward) < 0) {
                intOrientation = 1;
            } else {
                intOrientation = 2;
            }
        } else {
            intOrientation = 3;
        }

        //Per instance properties
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        
        props.SetColor("_Color", Color.white);
        props.SetInt("_Frame", frame);
        props.SetInt("_Orientation", intOrientation);

        GetComponent<MeshRenderer>().SetPropertyBlock(props);
    }
}
