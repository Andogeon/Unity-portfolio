using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 중 소비창 인벤토리 아이템의 위치 맞교환 및 아이콘의 이동, 그리고 외부에서 물약을 사용할 수 있게 하는 클래스입니다.
public class Consumption_Inventory : Inventory // 소비창 인벤토리 클래스입니다.
{
    //public void InsertItem(GameObject _InstanceObject)
    //{
    //    if (m_pSlots == null)
    //        m_pSlots = GetComponentsInChildren<Slot>();

    //    ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

    //    for (int i = 0; i < m_pSlots.Length; ++i)
    //    {
    //        if (m_pSlots[i].AccessICon != null)
    //        {
    //            if (m_pSlots[i].AccessICon.name == _InstanceICon.name) // 같은 아이콘 오브젝트라면  
    //            {
    //                m_pSlots[i].AccessICon.AccessIconCount += 1; // 오브젝트를 생성하지 않고 숫자만 늘린다.

    //                break;
    //            }
    //        }
    //        else if (m_pSlots[i].AccessICon == null) // 슬롯에 아이템이 존재하지 않는다면 
    //        {
    //            _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY; // 인자로 들어오는 아이콘의 상태를 인벤토리로 변경 

    //            _InstanceICon.AccessMySlot = m_pSlots[i]; // 아이콘의 슬롯정보를 현재 슬롯에 정보로 넣어준다

    //            m_pSlots[i].AccessICon = _InstanceICon; // 해당 슬롯에 아이템을 추가한다!

    //            return;
    //        }
    //    }
    //}


    [SerializeField] private Player _Player = null; 

    [SerializeField] private LayerMask _DivisionLayer = 0;

    private bool m_bIsMoving = false;

    private bool m_bIsClick = false;

    private RectTransform m_pRectTransform = null;

    private ICON m_pMouseSelectICon = null; // 한 번 클릭시 아이콘 복사 오브젝트가 생성될 변수입니다.

    private void Awake()
    {
        m_pSlots = GetComponentsInChildren<Slot>();

        m_pRectTransform = transform.parent.parent.GetComponent<RectTransform>();
    }

    // 마우스의 위치값 및 레이캐스트를 검사하여 인벤토리의 창의 이동, 복사 오브젝트의 생성을 하게 구현했습니다.

    private void Update()
    {
        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, _DivisionLayer);

        if (_HitCast.collider != null && Input.GetMouseButton(0))
        {
            if (_HitCast.collider.CompareTag("Inventory") == true) // 현재 레이케스트로 충돌 감지된 오브젝트 테그가 인벤토리라면
                m_bIsMoving = true; // 인벤토리 창 이동을 활성화한다.
        }

        if (m_bIsMoving == true) // 인벤토리 창 이동이 활성화 되었다면 
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_bIsMoving = false; // 인벤토리 창 이동을 비활성화 한다.

                return;
            }

            m_pRectTransform.position = _PickingPosition; // 인벤토리 위치값을 마우스 위치값으로 변경한다.
        }

        if (_HitCast.collider != null && Input.GetMouseButtonDown(0)) // 레이 캐스트가 아이콘을 클릭 했을 떄
        {
            if (m_bIsClick == false)
            {
                InstacneCopyICon(_HitCast); // 복사본 아이콘을 생성한다.

                m_bIsClick = true; // 한 번 클릭

                m_pSoundManager.PlaySound("Start Drop Click Sound");

                return;
            }

            m_bIsClick = false; // 클릭을 비활성화한다.

            UseItem(_HitCast); // 더블클릭 시 물약을 사용한다.
        }
    }

    // 인자로 들어오는 레이캐스트로 게임오브젝트를 찾아 아이콘의 복사 오브젝트를 생성하게 하는 함수입니다.

    private void InstacneCopyICon(RaycastHit2D _HitCast)
    {
        GameObject _GameObject = _HitCast.collider.gameObject;

        if (null == _GameObject)
            return;

        Vector2 _MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 _MyPosition = _GameObject.transform.position; 

        Vector2 _Direction = _MyPosition - _MousePosition;

        if (_Direction.magnitude <= 20.0f && null == m_pMouseSelectICon) // 마우스의 복사 생성본이 존재하지 않는다면 
        {
            Slot _CollisionSolt = _GameObject.GetComponent<Slot>();

            if (null == _CollisionSolt)
                return;

            ICON _CollisionICon = _CollisionSolt.AccessICon; // 슬롯의 아이콘을 검색 

            if (null == _CollisionICon)
                return;

            GameObject _InstanceCopyIconObject = GameObject.Instantiate(_CollisionICon.gameObject); // 충돌된 아이콘의 복사 오브젝트 생성

            _InstanceCopyIconObject.layer = LayerMask.NameToLayer("Copy");

            _InstanceCopyIconObject.transform.SetParent(transform.parent);

            ICON _CopyICon = _InstanceCopyIconObject.GetComponent<ICON>();

            if (null == _CopyICon)
            {
                Destroy(_InstanceCopyIconObject);

                return;
            }

            _CopyICon.AccessMySlot = _CollisionSolt;

            _CopyICon.AccessMovingICon = true;

            _CopyICon.AccessItem = _CollisionICon.AccessItem; // 기존 값들을 그대로 복사 

            m_pMouseSelectICon = _CopyICon; // 현재 복사 오브젝트의 참조변수 대입
        }
    }

    //인자로 들어오는 게임오브젝트를 삽입하는 함수입니다.

    public void InsertItem(GameObject _InstanceObject)
    {
        if (m_pSlots == null)
            m_pSlots = GetComponentsInChildren<Slot>();

        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i].AccessICon != null)
            {
                if (m_pSlots[i].AccessICon.name == _InstanceICon.name) // 같은 아이콘 오브젝트라면  
                {
                    m_pSlots[i].AccessICon.AccessIconCount += 1; // 오브젝트를 생성하지 않고 숫자만 늘린다.

                    break;
                }
            }
            else if (m_pSlots[i].AccessICon == null) // 슬롯에 아이템이 존재하지 않는다면 
            {
                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

                _InstanceICon.AccessMySlot = m_pSlots[i];

                m_pSlots[i].AccessICon = _InstanceICon; // 해당 슬롯에 아이템을 추가한다!

                m_pSlots[i].AccessICon.OriginalItem.AccessIcon = m_pSlots[i].AccessICon;

                return;
            }
        }
    }

    // 인자로 들어오는 레이캐스트로 아이템의 위치 교환 및 사용하게 될 시 호출 되는 함수입니다.

    public void UseItem(RaycastHit2D _HitCast)
    {
        if (_HitCast.collider == null || m_pMouseSelectICon == null)
            return;

        GameObject _CollisionObject = _HitCast.collider.gameObject;

        if (m_pMouseSelectICon.AccessMySlot.gameObject == _CollisionObject)
        {
            if (m_pMouseSelectICon.AccessItem != null)
            {
                m_pMouseSelectICon.UsePotion(null); // 회복한다 !! 

                Destroy(m_pMouseSelectICon.gameObject);

                m_pMouseSelectICon = null;

                m_bIsClick = false;
            }
        }
        else
        {
            for (int i = 0; i < m_pSlots.Length; ++i)
            {
                if (m_pSlots[i].gameObject == _CollisionObject) // 같은 슬롯이면 
                {
                    if (m_pSlots[i].AccessICon == null) // 해당 슬롯에 아이콘이 존재 하지 않는다
                    {
                        ICON _ICon = m_pMouseSelectICon.AccessMySlot.AccessICon; // 마우스 복사 오브젝트의 아이콘을 찾고 

                        m_pSlots[i].AccessICon = _ICon; // 해당 슬롯에 넣는다 

                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot(); // 이전 마우스 복사 오브젝트 슬롯을 비워 둔다

                        Destroy(m_pMouseSelectICon.gameObject); // 복사 오브젝트의 삭제 

                        m_pMouseSelectICon = null; // 복사 오브젝트 참조 변수 초기화 

                        break;
                    }
                    else // 슬롯에 아이콘이 존재 한다면 
                    {
                        ICON _FirstICon = m_pMouseSelectICon.AccessMySlot.AccessICon; // 마우스 복사 오브젝트의 슬롯 아이콘 정보

                        ICON _SecondICon = m_pSlots[i].AccessICon; // 현재 아이콘의 슬롯 아이콘 정보 

                        m_pSlots[i].ClearPartSlot(); // 현재 아이콘의 슬롯 정보를 비운다 

                        m_pSlots[i].AccessICon = _FirstICon; // 현재 슬롯에 마우스 슬롯 아이콘을 넣어준다.

                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot(); // 마우스 복사 오브젝트가 있던 슬롯을 비운다

                        m_pMouseSelectICon.AccessMySlot.AccessICon = _SecondICon; // 현재 아이콘 슬롯의 정보를 넣어서 

                        // 2개의 오브젝트를 슬롯 위치 정보를 바꾸어준다.

                        Destroy(m_pMouseSelectICon.gameObject); // 기존 마우스 복사 아이콘 정보를 파괴

                        m_pMouseSelectICon = null; // 마우스 복사 참조변수의 초기화 

                        break;
                    }
                }
            }
        }
    }

    // 실제 플레이어가 물약을 복용시 호출될 함수입니다.
    public void UsePostion(Player _Players)
    {
        if (null == _Player)
            return;

        if(m_pSlots == null)
            m_pSlots = GetComponentsInChildren<Slot>();

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i] == null) // 슬롯이 없다면 
                continue; // 넘어간다
            else if (m_pSlots[i].AccessICon == null) // 슬롯에 아이콘이 존재하지 않는다면
                continue; // 넘어간다
            else if (m_pSlots[i].AccessICon.name == "HP Portion ICon(Clone)") // 아이콘의 이름이 존재한다면 
            {
                m_pSlots[i].AccessICon.OriginalItem.UsePotion(_Player, m_pSlots[i].AccessICon); // 해당 물약 오브젝트의 사용

                return;
            }
        }
    }

    // UI 스프라이트 클릭시 활성화 될 때 호출 되는 함수입니다.
    public override void EnableInventory()
    {
        gameObject.SetActive(true);
    }


    // UI 스프라이트 클릭시 비활성화 될 때 호출 되는 함수입니다.
    public override void DisabledInventory()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_pSlots.Length; ++i)
            m_pSlots[i] = null;

        m_pRectTransform = null;

        m_pMouseSelectICon = null;
    }
}