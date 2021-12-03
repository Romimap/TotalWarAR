using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour {

    //Armor < 2 AP : 200% damage
    //Armor <   AP : 150% damage
    //Armor =   AP : 100% damage
    //Armor >   AP : 75% damage
    //Armor > 2 AP : 50% damage

    public static List<Unit> s_units = new List<Unit>();

    public float _maxHP = 100;
    public float _armor = 2;
    public float _speed = 5;
    public float _mass = 1;
    public float _turnSpeed = 2;
    public float _damage = 10;
    public bool _ranged = false;
    public float _range = 0.3f;
    public float _armorPenetration = 2;

    public float _windupSpeed = 1f;
    public float _recoverSpeed = 1f;

    public int _team = 0;

    public bool Alive { get { return _currentHP > 0; } }

    private Animator _animator;
    private Vector3 _previousPosition;
    private Unit _target = null;
    private float _currentHP;
    private Rigidbody _rigidbody;

    private static int s_id = 0;

    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("WindupSpeed", _windupSpeed);
        _animator.SetFloat("RecoverSpeed", _recoverSpeed);
        _rigidbody = GetComponentInParent<Rigidbody>();
        _rigidbody.mass = _mass;
        _previousPosition = transform.position;
        _currentHP = _maxHP;
        //_team = s_id++;

        s_units.Add(this);
    }

    // Update is called once per frame
    void FixedUpdate() {
        //Set Speed
        float currentSpeed = ((_previousPosition - transform.position).magnitude / _speed);
        _previousPosition = transform.position;
        _animator.SetFloat("CurrentSpeed", currentSpeed);

        if (Alive) {
            //Set Target
            if (_target != null && _target.Alive) {
                float distanceToTarget = (transform.position - _target.transform.position).magnitude - _range;
                _animator.SetBool("TargetAlive", true);
                _animator.SetFloat("TargetDistance", distanceToTarget);

            } else {
                _animator.SetBool("TargetAlive", false);

                //Find a new Target (Alive & Not in the same team)
                float minDistance = float.MaxValue;
                Unit closestUnit = null;
                foreach(Unit u in s_units) {
                    if (u.Alive && u._team != _team) {
                        float sqrDistance = (transform.position - u.transform.position).sqrMagnitude;
                        if (sqrDistance < minDistance) {
                            minDistance = sqrDistance;
                            closestUnit = u;
                        }
                    }
                }
                _target = closestUnit;

            }

            if (_target != null && _target.Alive) {

                //Turn To Target
                Transform lookAtTarget = transform;
                lookAtTarget.LookAt(_target.transform, Vector3.up);

                Vector3 eulerAngles = lookAtTarget.eulerAngles;
                eulerAngles.z = 0;
                eulerAngles.x = 0;
                lookAtTarget.eulerAngles = eulerAngles;

                transform.rotation = Quaternion.Lerp(transform.rotation, lookAtTarget.rotation, _turnSpeed * Time.fixedDeltaTime);

                float distance = (_target.transform.position - transform.position).magnitude;
                if (distance < _range) { //Stop Moving
                    _rigidbody.AddForce(-_rigidbody.velocity * _speed * _mass * 100);
                } else { //Move To Target
                    float speedMult = Vector3.Dot(transform.forward, lookAtTarget.forward);
                    speedMult = Mathf.Clamp01(speedMult);
                    //We add a force to the direction we are facing, the force is inversely proportional to the velocity
                    _rigidbody.AddForce(transform.forward * _mass * map(currentSpeed, 0, _speed, _speed, 0));
                    _rigidbody.AddForce(-transform.right * Vector3.Dot(transform.right, _rigidbody.velocity));
                }

            }
        } else {
            _rigidbody.AddForce(-_rigidbody.velocity * _speed * _mass * 100);
        }
    }

    float map (float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void Attack () {
        if (_target != null && _target.Alive) {
            _target.Damage(_damage);
        }
    }

    public void Damage (float d) {
        Debug.Log("HIT !");
        _currentHP -= d;
        if (_currentHP <= 0) {
            Debug.Log("DEAD CALL !");
            _currentHP = 0;
            _animator.SetBool("Dead", true);
        }
    }

    public void Die () {
        Debug.Log("DEAD ! ");
        s_units.Remove(this);
        Destroy(transform.parent.gameObject);
    }
}
