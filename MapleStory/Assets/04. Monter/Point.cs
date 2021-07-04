using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private GameObject _CreateMonter = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private GameObject m_pMonterObject = null;

    private float m_fTimeAcc = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_pGameObjectManager.OriginalGamgObjectInsert(_CreateMonter.name, _CreateMonter);

        m_pMonterObject = m_pGameObjectManager.GameObejctPooling(_CreateMonter.name, Vector3.zero, transform.position, _CreateMonter.transform.rotation);

        MONTER _Monter = m_pMonterObject.GetComponent<MONTER>();

        _Monter.AccessMonterName = _CreateMonter.name;
    }

    private void LateUpdate()
    {
        if (null == m_pMonterObject)
            return;

        if (m_pMonterObject.activeSelf == false)
        {
            m_fTimeAcc += Time.deltaTime;

            if (m_fTimeAcc >= 1.5f)
            {
                m_fTimeAcc = 0.0f;

                m_pMonterObject = m_pGameObjectManager.GameObejctPooling(_CreateMonter.name, Vector3.zero, transform.position, Quaternion.identity);

                return;
            }
        }
    }
}
