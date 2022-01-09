using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static GameObject s_camera = null;

    public AnimationCurve _heightCurve = new AnimationCurve();
    public float _projectileSpeed = 1f;
    public Unit _target;
    public float _damage;
    private float _timer = 0;
    private Vector3 _startPosition;
    private Vector3 _previousPosition;
    private GameObject _skin;

    // Start is called before the first frame update
    void Start () {
        _startPosition = transform.position;
        _skin = transform.Find("Skin").gameObject;

        if (s_camera == null) {
            s_camera = GameObject.Find("AR Camera");
        }
    }

    // Update is called once per frame
    void Update() {
        _previousPosition = transform.position;

        _timer += Time.deltaTime * _projectileSpeed;
        _timer = Mathf.Clamp01(_timer);

        Vector3 p = _target.transform.position * _timer + _startPosition * (1 - _timer);
        //p.y += _heightCurve.Evaluate(_timer);
        transform.position = p;

        Vector3 direction = (transform.position - _previousPosition).normalized;

        if (_timer >= 1) {
            _target.Damage(_damage);
            Destroy(gameObject);
        }

        transform.LookAt(transform.position + direction, Vector3.up);

    }
}
