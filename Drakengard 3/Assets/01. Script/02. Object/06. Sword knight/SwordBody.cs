using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBody : MonoBehaviour
{
    private BoxCollider m_ZeroWeapon = null;

    private Swordknight _Swordknight = null;
    private void Start()
    {
        _Swordknight = GetComponentInParent<Swordknight>();

        m_ZeroWeapon = GameObject.Find("Weapon").GetComponent<BoxCollider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
            _Swordknight.GetSwordAnimations.AccessSwordAnimations = SW_Animations.SW_HIT;
    }
}
