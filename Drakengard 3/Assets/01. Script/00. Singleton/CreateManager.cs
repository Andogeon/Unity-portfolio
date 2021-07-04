using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    [SerializeField] private GameObject _CreateGameObject = null;

    private GameobjectManager _InstanceManager = GameobjectManager.GetInstance();

    private List<Swordknight> m_Swordknights = new List<Swordknight>();

    // Start is called before the first frame update
    void Start()
    {
        Transform[] _CreatePositions = GetComponentsInChildren<Transform>();

        for (int i = 1; i < _CreatePositions.Length; ++i)
        {
            Vector3 _Createposition = _CreatePositions[i].position;

            _InstanceManager.CreateobjectToInsert("Sword Knight", _CreateGameObject, _Createposition, Quaternion.identity, 1);
        }

        List<GameObject> _DontInstanceGameObject = _InstanceManager.GetDontInstacneGameObject("Sword Knight");

        if (null == _DontInstanceGameObject)
            return;

        for(int i = 0; i < _DontInstanceGameObject.Count; ++i)
        {
            Swordknight _Swordknight = _DontInstanceGameObject[i].GetComponent<Swordknight>();

            if (null == _Swordknight)
                return;

            _Swordknight.SetPositions = _CreatePositions;

            _Swordknight.SetSwordknightState = SwordknightState.REMOVE_POSITION;

            _InstanceManager.GameObjectActivation("Sword Knight", _CreatePositions[i].position);
        }
    }
}
