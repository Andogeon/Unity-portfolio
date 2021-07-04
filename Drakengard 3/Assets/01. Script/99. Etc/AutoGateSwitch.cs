using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGateSwitch : MonoBehaviour
{
    [SerializeField] private AutogateDoor[] autogateDoors = null;

    private bool m_bIsOffDoor = false;

    public bool GetOffDoor
    {
        get { return m_bIsOffDoor; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (autogateDoors != null)
            {
                for (int i = 0; i < autogateDoors.Length; ++i)
                    autogateDoors[i].SetSwitchDoor = true;
            }

            m_bIsOffDoor = true;

            Destroy(gameObject);
        }
    }
}
