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

// 케릭터 설정 커스터마이징 헤어, 얼굴, 아이템의 대한 텍스트와 케릭터가 실제 변경될 파츠 데이터를 관리하는 클래스입니다.

public class TEXT : MonoBehaviour
{
    [SerializeField] private string[] _StringName = null; // 케릭터 설정 각종 파츠들의 이름 

    [SerializeField] private PART[] _AvatarFaces = null; // 적용 될 파츠 데이터 

    [SerializeField] private GameObject _ChangeObject = null; // 변경 될 파츠 오브젝트

    // 파츠를 적용 할 것인지 아니면 아이템에 대해 적용 할 것인지 선택하는 변수

    [SerializeField] private SELECTMOTION _Select = SELECTMOTION.SELECT_PART; 

    private SpriteRenderer m_pSpriteRenderer = null; // 실제 적용될 파츠의 스프라이트 랜더러 

    private PART m_pPart = null; // 아이템의 대해 적용될 파츠정보 

    private Transform m_pTransform = null; // 실제 적용될 파츠마다 로컬 좌표

    private Text m_pText = null; // 케릭터 설정 커스터마아징 텍스트를 프로젝트의 적용하기 위한 변수 

    private int m_iIndex = 0; // Button 클래스에서 클릭할 때마다 _AvatarFaces 배열값에 접근하기 위한 인덱스 값

    private int m_iCount = 0; // 케릭터 커스터마이징 이름의 개수, 인덱스가 케릭터 커스터마이징의 개수보다 높을 시 

    // 배열 접근을 막기 위한 변수 

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

    // 텍스트 게임오브젝트마다 인스펙터에서 지정한 열거형 값에 따라 케릭터 설정 커스터마이징을 설정할 수 있게 

    // 함수기능을 분기별로 나누었습니다.
    void Update()
    {
        switch (_Select) // 인스펙터에서 지정한 열거형의 따라서
        {
            case SELECTMOTION.SELECT_PART: // 해당 게임오브젝트 클래스가 얼굴, 헤어 항목일 경우 
                SelectPart(); // 몸체 스프라이트 변경 함수 실행
                break;
            case SELECTMOTION.SELECT_ITEM: // 해당 게임오브젝트 클래스가 아이템일 경우 
                SelectItem(); // 아이템 착용 함수 실행
                break;
        }

        m_pText.text = _StringName[m_iIndex]; // 텍스트를 인스펙터에서 지정한 문장으로 적용
    }

    //// 텍스트 게임오브젝트마다 인스펙터에서 지정한 열거형 값에 따라 케릭터 설정 커스터마이징을 설정할 수 있게 

    //// 함수기능을 분기별로 나누었습니다.
    //void Update()
    //{
    //    switch(_Select)
    //    {
    //        case SELECTMOTION.SELECT_PART:
    //            SelectPart(); // 몸체 파츠별로
    //            break;
    //        case SELECTMOTION.SELECT_ITEM:
    //            SelectItem(); // 아이템
    //            break;
    //        //case SELECTMOTION.SELECT_ANIMATION:
    //        //    SelectAnimation();
    //        //    break;
    //    }

    //    m_pText.text = _StringName[m_iIndex];
    //}

    // 각 몸체의 파츠 별로 스프라이트와 위치정보를 적용하는 함수입니다.
    private void SelectPart()
    {
        if(m_pPart is Hair == true) // 변경할 오브젝트의 파츠가 캐스팅시 헤어라면 
        {
            Hair _Hair = m_pPart as Hair; // 참조변수를 헤어로 형변환하여 

            _Hair.AccessHair = _AvatarFaces[m_iIndex] as HAIR; // 헤어로 넣어준다
        }

        m_pSpriteRenderer.sprite = _AvatarFaces[m_iIndex].GetSprite(); // 해당 스프라이트를 적용하고 

        m_pTransform.localPosition = _AvatarFaces[m_iIndex].transform.localPosition; // 로컬의 위치값 또한 변경
    }

    // 커스터마이징 케릭터 아이템 적용에 관련 함수입니다.
    private void SelectItem()
    {
        ITEM _SelectItem = _AvatarFaces[m_iIndex] as ITEM; // 아이템으로 형변환한다

        _SelectItem.AccessItemModeType = ITEMMODETYPE.ITEM_SELETEMODE; 

        m_pPart.SetItem(_SelectItem); // 해당 파츠에 아이템을 적용한다.
    }


    // 기존에 플레이어의 애니메이션을 적용하고자 했지만 UI 스프라이트 및 폰트가 존재하지 않아 따로 적용하지 않았습니다.

    // 해당 함수는 UI 버튼에서 클릭 할 때마다 커스터마이징 케릭터의 애니메이션을 움직일 수 있게 적용하는 함수입니다
    private void SelectAnimation()
    {
        Body _Body = m_pPart as Body;

        if (_Body == null)
            return;

        _Body.GetAvatarState = (AVATARSTATES)m_iIndex;
    }
}