using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveTile : Tile
{
    protected override void Awake()
    {
        base.Awake();

        m_fResetYPosition = transform.localPosition.y;
    }

    public override void TileAnimationChecking(BoxCollider2D _pGameSceneBox)
    {
        if (m_SceneRange == CGlobalEnum.SCENE_RANGE.RANGE_NONE && m_pMoveCorotine == null)
        {
            m_pMoveCorotine = StartCoroutine(StartMove());

            return;
        }
        else
        {
            if (m_pMoveCorotine != null)
                return;

            float _fTileLeftPosition = GetTileLeftPostion();

            float _fGameRangePosition = GetGameSceneRangePosition(_pGameSceneBox);

            if (_fTileLeftPosition <= _fGameRangePosition)
                m_pMoveCorotine = StartCoroutine(StartMove());
        }
    }
    
    public override void ResetMyAnimation()
    {
        StopCoroutine(m_pMoveCorotine);

        m_pMoveCorotine = null;

        transform.localPosition = new Vector3(transform.localPosition.x, m_fResetYPosition, 0.0f);
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

    private IEnumerator StartMove()
    {
        while(true)
        {
            float _YPosition = transform.localPosition.y - m_Speed * Time.deltaTime;

            transform.localPosition = new Vector3(transform.localPosition.x, _YPosition);

            if (_YPosition <= -5.0f)
                yield break;

            yield return null;
        }
    }

    [SerializeField] private CGlobalEnum.SCENE_RANGE m_SceneRange = CGlobalEnum.SCENE_RANGE.RANGE_NONE;

    [SerializeField] private float m_Speed = 0.0f;

    private float m_fResetYPosition = 0.0f;

    private Coroutine m_pMoveCorotine = null;
}
