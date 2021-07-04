using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SELECTMOTION
{
    SELECT_PART,
    SELECT_ITEM,
    SELECT_ANIMATION
}

public class TEXT : MonoBehaviour
{
    [SerializeField] private string[] _StringName = null;

    [SerializeField] private PART[] _AvatarFaces = null;

    [SerializeField] private GameObject _ChangeObject = null;

    [SerializeField] private SELECTMOTION _Select = SELECTMOTION.SELECT_PART;

    private SpriteRenderer m_pSpriteRenderer = null;

    private PART m_pPart = null;

    private Transform m_pTransform = null;

    private Text m_pText = null;

    private int m_iIndex = 0;

    private int m_iCount = 0;

    public int GetCount
    {
        get { return m_iCount; }
    }

    public int SetIndex
    {
        get { return m_iIndex; }

        set { m_iIndex = value; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (null != _ChangeObject)
        {
            m_pSpriteRenderer = _ChangeObject.GetComponent<SpriteRenderer>();

            m_pTransform = _ChangeObject.transform;

            m_pPart = _ChangeObject.GetComponent<PART>();
        }

        m_pText = GetComponent<Text>();

        m_iCount = _StringName.Length;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_Select)
        {
            case SELECTMOTION.SELECT_PART:
                SelectPart();
                break;
            case SELECTMOTION.SELECT_ITEM:
                SelectItem();
                break;
            case SELECTMOTION.SELECT_ANIMATION:
                SelectAnimation();
                break;
        }

        m_pText.text = _StringName[m_iIndex];
    }

    private void SelectPart()
    {
        if(m_pPart is Hair == true)
        {
            Hair _Hair = m_pPart as Hair;

            _Hair.AccessHair = _AvatarFaces[m_iIndex] as HAIR;
        }

        m_pSpriteRenderer.sprite = _AvatarFaces[m_iIndex].GetSprite();

        m_pTransform.localPosition = _AvatarFaces[m_iIndex].transform.localPosition;
    }

    private void SelectItem()
    {
        ITEM _SelectItem = _AvatarFaces[m_iIndex] as ITEM;

        _SelectItem.AccessItemModeType = ITEMMODETYPE.ITEM_SELETEMODE; ////////

        m_pPart.SetItem(_SelectItem);
    }

    private void SelectAnimation()
    {
        Body _Body = m_pPart as Body;

        if (_Body == null)
            return;

        _Body.GetAvatarState = (AVATARSTATES)m_iIndex;
    }
}