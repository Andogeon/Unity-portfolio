using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAniTile : Tile
{
    protected override void Awake()
    {
        base.Awake();

        ComponentInitialize();
    }

    private void ComponentInitialize()
    {
        m_pAnimator = GetComponent<Animator>();
    }

    private float GetGameSceneRangePosition(BoxCollider2D _pGameSceneBox)
    {
        if (_pGameSceneBox == null)
            return 0.0f;

        float _fGameRangeValue = 0.0f;
        float _fGameBGBoxHalfSize = (_pGameSceneBox.size.x * _pGameSceneBox.transform.localScale.x) / 2;

        switch (m_SceneRange)
        {
            case CGlobalEnum.SCENE_RANGE.RANGE_CENTER:
                _fGameRangeValue = _pGameSceneBox.transform.position.x;
                break;
            case CGlobalEnum.SCENE_RANGE.RANGE_LEFT:
                _fGameRangeValue = _pGameSceneBox.transform.position.x - _fGameBGBoxHalfSize;
                break;
            case CGlobalEnum.SCENE_RANGE.RANGE_LEFTMID:
                _fGameRangeValue = _pGameSceneBox.transform.position.x - _fGameBGBoxHalfSize;
                _fGameRangeValue += _fGameBGBoxHalfSize / 2;
                break;
            case CGlobalEnum.SCENE_RANGE.RANGE_RIGHT:
                _fGameRangeValue = _pGameSceneBox.transform.position.x + _fGameBGBoxHalfSize;
                break;
            case CGlobalEnum.SCENE_RANGE.RANGE_RIGHTMID:
                _fGameRangeValue = _pGameSceneBox.transform.position.x + _fGameBGBoxHalfSize / 2;
                break;
        }

        return _fGameRangeValue;
    }

    public override void TileAnimationChecking(BoxCollider2D _pGameSceneBox)
    {
        if (m_pAnimator == null)
            return;

        if (m_SceneRange == CGlobalEnum.SCENE_RANGE.RANGE_NONE)
        {
            m_pAnimator.SetTrigger("Play");
            
            return;
        }

        float _fTileLeftPosition = GetTileLeftPostion();

        float _fGameRangePosition = GetGameSceneRangePosition(_pGameSceneBox);

        if (_fTileLeftPosition <= _fGameRangePosition)
        {
            m_pAnimator.SetTrigger("Play");

            if(m_Number == 1)
                StartCoroutine(PlayAnimations());
        }
    }

    public override void ResetMyAnimation()
    {
        if (m_pAnimator == null)
            return;

        if (!m_pAnimator.enabled)
            m_pAnimator.enabled = true;

        m_pAnimator.SetTrigger("Reset");
    }

    private IEnumerator PlayAnimations()
    {
        yield return null;

        m_pAnimatorControler = m_pAnimator.runtimeAnimatorController;

        AnimationClip[] _pAnimationClips = m_pAnimator.runtimeAnimatorController.animationClips;

        string _strAnimationPlay = string.Empty;

        for(int i = 0; i < _pAnimationClips.Length; ++i)
        {
            if (!_pAnimationClips[i].name.Contains("Play"))
                continue;

            _strAnimationPlay = _pAnimationClips[i].name;

            break;
        }

        //while (m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName(_pAnimationClips[1].name) &&
        //    m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        //    yield return null;

        while (m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName(_strAnimationPlay) &&
            m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        m_pAnimator.enabled = false;
    }

    private Animator                    m_pAnimator = null;

    private RuntimeAnimatorController m_pAnimatorControler = null;

    [SerializeField] private CGlobalEnum.SCENE_RANGE     m_SceneRange = CGlobalEnum.SCENE_RANGE.RANGE_CENTER;

    [SerializeField] private int m_Number = 0;
}