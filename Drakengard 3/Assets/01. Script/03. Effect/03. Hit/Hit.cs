using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private ParticleSystem _HitEffect = null;

    private GameobjectManager _GameobjectManager = GameobjectManager.GetInstance();

    private void OnEnable()
    {
        _HitEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_HitEffect.isPlaying == false)
            _GameobjectManager.GameObjectDisabled("Hit Effect", gameObject);
    }
}
