using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem _Effect = null;

    private GameobjectManager _Gameobjectmanager = GameobjectManager.GetInstance();

    private void OnEnable()
    {
        _Effect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_Effect.isPlaying == false)
            _Gameobjectmanager.GameObjectDisabled("Dash Effect", gameObject);
    }
}
