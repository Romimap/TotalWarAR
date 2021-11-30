using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Unit : MonoBehaviour {

    //Armor < 2 AP : 200% damage
    //Armor <   AP : 150% damage
    //Armor =   AP : 100% damage
    //Armor >   AP : 75% damage
    //Armor > 2 AP : 50% damage

    public float maxHP = 100;
    public float armor = 2;
    public float speed = 5;
    public float damage = 10;
    public bool ranged = false;
    public float armorPenetration = 2;

    public float attackTime = 0.1f;
    public float recoverTime = 0.5f;

    public int team = 0;

    public enum UnitState {
        IDLE, FOLLOWING, ATTACK, RECOVER
    }
    private UnitState _unitState = UnitState.IDLE;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (_unitState == UnitState.IDLE) {
            
        }
    }
}
