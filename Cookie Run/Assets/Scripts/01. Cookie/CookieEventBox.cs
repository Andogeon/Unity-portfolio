using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------
// ÀÚ¼® ÄíÅ°, °õÁ©¸® ÆÄÆ¼ ¹Ú½º Å¬·¡½º
//-----------------------------------------
public class CookieEventBox : MonoBehaviour
{
    private CPoolManaged m_pPoolManaged = null;

    private CSceneManaged m_pSceneManaged = null;

    private CGlobalEnum.COOKIE_SKILL m_eEventBoxType = CGlobalEnum.COOKIE_SKILL.COOKIE_NONE;

    private Cookie m_pCookie = null;

    private float m_fMagnetCoolTime = 0.0f;

    private float m_fJellyChangeTime = 0.0f;

    public CGlobalEnum.COOKIE_SKILL GetEventType { get => m_eEventBoxType; }

    private void Start()
    {
        m_pCookie = FindObjectOfType<Cookie>();

        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        m_pSceneManaged = CTopManager.GetInstance().GetSceneManaged;
    }

    public void SetCookieEventType(CGlobalEnum.COOKIE_SKILL _eEventBoxType)
    {
        if (_eEventBoxType < CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY)
            return;

        m_eEventBoxType = _eEventBoxType;
    }

    private void Update()
    {
        m_fMagnetCoolTime += Time.deltaTime;

        if (m_fMagnetCoolTime >= 5.0f)
        {
            gameObject.SetActive(false);
            m_eEventBoxType = CGlobalEnum.COOKIE_SKILL.COOKIE_NONE;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(m_eEventBoxType)
        {
            case CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY:
                ChangeJelly(collision.gameObject);
                break;
            case CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET:
                MoveingJelly(collision.gameObject);
                break;
            case CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNETANDCHANGEJELLY:
                ChangeJelly(collision.gameObject);
                MoveingJelly(collision.gameObject);
                break;

        }
    }

    private void ChangeJelly(GameObject _pGameObject)
    {
        if (_pGameObject == null || !_pGameObject.activeSelf || _pGameObject.name != "Blue Jelly")
            return;

        Vector2 _vJellyPosition = _pGameObject.transform.localPosition;

        string _strJellyName = string.Empty;

        switch(m_eEventBoxType)
        {
            case CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY:
                _strJellyName = "Nomral Bear Jelly";
                break;
        }

        _pGameObject.SetActive(false);
        GameObject _pGameObjects = m_pPoolManaged.pop("Jelly Change Effect", _vJellyPosition, _pGameObject.transform.parent);
        _pGameObjects = m_pPoolManaged.pop(_strJellyName, _vJellyPosition, _pGameObject.transform.parent);
    }

    private void MoveingJelly(GameObject _pJelly)
    {
        Vector3 _vDirection = m_pCookie.transform.position - _pJelly.transform.position;
        _vDirection.Normalize();

        float _fSpeed = (m_pSceneManaged.GetGameScene.GetGameSceneSpeed != 15.0f) ? 10.0f : 20.0f;

        _pJelly.transform.position += _vDirection * _fSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        ResetMagnetTime();

        ResetJellyChangeTime();
    }


    public void ResetMagnetTime()
    {
        m_fMagnetCoolTime = 0.0f;
    }

    public void ResetJellyChangeTime()
    {
        m_fJellyChangeTime = 0.0f;
    }
}
