using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour // 퀘스트 목록을 구현한 클래스입니다.
{
    private QuestSlot[] m_pQuestSlots = null; // 수행할 퀘스트 슬롯입니다.

    private Questsub_Window m_pSubWindow = null; // 퀘스트 세부사항을 알려주는 서브 윈도우 창에 참조변수입니다.

    private List<QUEST> m_pFinalQuest = new List<QUEST>(); // 완료된 퀘스트 목록 변수입니다.

    private int m_iEnableCount = 0; // 퀘스트 완료 후 정렬하기 위한 변수입니다.

    public List<QUEST> GetFinalQuestList
    {
        get { return m_pFinalQuest; }

        set { m_pFinalQuest = value; }
    }

    // Start is called before the first frame update
    void Awake() 
    {
        GameObject _ParentGameObject = transform.parent.gameObject;

        m_pSubWindow = _ParentGameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

        GameObject[] _GameObjects = new GameObject[transform.childCount];

        m_pQuestSlots = new QuestSlot[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _GameObjects[i] = transform.GetChild(i).gameObject;

            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();

            m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;
        }

        _GameObjects = null;
    }   

    // 인자로 들어오는 퀘스트를 슬롯에 추가하는 함수입니다.

    public void InsertQuest(QUEST _InsertQuest) 
    {
        if (null == m_pQuestSlots) // 슬롯이 존재 하지 않는 다면
            return; // 종료 

        for (int i = 0; i < m_pQuestSlots.Length; ++i) // 슬롯을 하나씩 순회하면서 
        {
            if (m_pQuestSlots[i].AccessQuest == null) // 슬롯에 퀘스트가 존재하지 않는다면 
            {
                m_pQuestSlots[i].gameObject.SetActive(true); // 슬롯에 오브젝트를 활성화 하고 

                m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow; // 슬롯에 퀘스트 세부사항을 알려주는 창을 추가한다.

                m_pQuestSlots[i].AccessQuest = _InsertQuest; // 퀘스트를 넣어주고 

                _InsertQuest.AccessQuestStart = true; // 시작한다.

                m_pQuestSlots[i].AccessText.text = _InsertQuest.AccessQuestSlotName; // 퀘스트의 주제목을 넣어준다.

                ++m_iEnableCount; // 그리고 활성화 된 숫자를 늘린다.

                return;
            }
        }
    }

    // 인자로 넘어오는 퀘스트를 완료 후 호출 되는 함수입니다.

    // 유니티 씬 뷰에서 확인한 결과 버그가 없었으나 포트폴리오 동영상 촬영 직전 .exe 파일로 확인 해봤는데 

    // 각각 다른 씬에서 NPC 간 퀘스트 완료 여부를 알 수 없어 퀘스트가 진행 되지 않는 문제가 발생했습니다.

    // 그래서 어짜피 Player 클래스가 씬 전환시 해당 퀘스트 리스트가 삭제 되지 않는다는 점을 이용하여 

    // m_pFinalQuest 즉 이미 퀘스트가 종료된 목록 중 찾아 NPC마다 static 변수로 퀘스트를 제어했습니다.
    public void ClearRemoveQuest(QUEST _RemoveQuest)
    {
        for (int i = 0; i < m_pQuestSlots.Length; ++i) // 퀘스트 슬롯을 순회하면서 
        {
            if (m_pQuestSlots[i].AccessQuest != null) // 퀘스트가 존재하고 
            {
                if (m_pQuestSlots[i].AccessQuest == _RemoveQuest) // 퀘스트 중 인자로 넘어온 클리어한 퀘스트가 존재한다면 
                {
                    m_pFinalQuest.Add(m_pQuestSlots[i].AccessQuest); // 퀘스트 완료 변수에 넣어준다

                    m_pQuestSlots[i].AccessQuest = null; // 그리고 해당 진행중인 퀘스트 슬롯에서 퀘스트를 비워주고 

                    m_pQuestSlots[i].gameObject.SetActive(false); // 해당 퀘스트 슬롯을 비활성화 한다

                    _RemoveQuest.AccessQuestStart = false; // 완료된 퀘스트를 시작을 끈다

                    if (m_pSubWindow == null) // 서브 퀘스트 창이 존재한다면 
                        m_pSubWindow = transform.parent.gameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

                    if (m_pSubWindow.gameObject.activeSelf == true) // 서브 퀘스트가 열려 있다면 
                        m_pSubWindow.gameObject.SetActive(false); // 바로 비활성화 하여 끈다.

                    --m_iEnableCount; // 활성화 된 퀘스트 숫자중 하나를 감수하고 

                    if (m_iEnableCount < 0) // 혹시 모를 음수가 나올수 있으니 
                        m_iEnableCount = 0; // 0으로 고정하고 

                    break; // 순회를 종료한다.
                }
            }
        }
    }

    // 위와 같이 인자로 넘어온 퀘스트를 지우는 함수입니다.

    // UI 퀘스트 포기창을 누를 시 호출 되는 함수입니다.

    public void RemoveQuest(QUEST _RemoveQuest)
    {
        for (int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (m_pQuestSlots[i].AccessQuest != null)
            {
                if (m_pQuestSlots[i].AccessQuest == _RemoveQuest)
                {
                    m_pQuestSlots[i].AccessQuest = null;

                    m_pQuestSlots[i].gameObject.SetActive(false);

                    _RemoveQuest.AccessQuestStart = false;

                    if(m_pSubWindow == null)
                        m_pSubWindow = transform.parent.gameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

                    if (m_pSubWindow.gameObject.activeSelf == true) // null
                        m_pSubWindow.gameObject.SetActive(false); // nullptr

                    --m_iEnableCount;

                    if (m_iEnableCount < 0)
                        m_iEnableCount = 0;

                    break;
                }
            }
        }
    }

    // 진행중인 퀘스트 중 해당 퀘스트가 있는지 찾는 함수입니다.

    public QUEST FindQuest(QUEST _Quset)
    {
        if (null == _Quset)
            return null;

        for(int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (_Quset == m_pQuestSlots[i].AccessQuest)
                return m_pQuestSlots[i].AccessQuest;
        }

        return null;
    }

    // 퀘스트 슬롯들을 순회하면서 퀘스트 갱신을 위해 호출되는 함수입니다.

    public void QuestListUpdate()
    {
        if (null == m_pQuestSlots)
            return;

        for (int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (m_pQuestSlots[i].AccessQuest != null)
                m_pQuestSlots[i].AccessQuest.QuestUpdate();
        }
    }

    // 해당 함수를 호출 시 퀘스트 리스트를 초기화 하는 함수입니다.
    public void DisableStart()
    {
        if (null != m_pQuestSlots)
            return;

        GameObject[] _GameObjects = new GameObject[transform.childCount];

        m_pQuestSlots = new QuestSlot[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _GameObjects[i] = transform.GetChild(i).gameObject;

            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();
        }

        _GameObjects = null;
    }

    // 2개 이상의 퀘스트가 있을 시 슬롯을 다시 정렬할 때 호출 되는 함수입니다/

    public void SortQusetList(QuestSlot _QusetSlot)
    {
        if (null == _QusetSlot)
            return;

        for (int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (_QusetSlot.gameObject.activeSelf == false) // 비활성화된 슬롯 오브젝트라면 
                continue; // 다음으로 넘김 

            int iIndex = i; 

            if(m_pQuestSlots[i] == _QusetSlot)
            {
                iIndex += 1; 

                if (iIndex >= m_iEnableCount) // 해당 인덱스의 값이 활성화 된 퀘스트 숫자보다 높거나 같다면 
                    iIndex = m_iEnableCount; // 인덱스의 숫자를 활성화 된 퀘스트 숫자로 맞춘다.

                m_pQuestSlots[i].AccessQuest = null; // 해당 인덱스의 퀘스트 정로를 비운다

                m_pQuestSlots[i].AccessQuest = m_pQuestSlots[iIndex].AccessQuest; // 해당 다음 인덱스 퀘스트 정보를 이전 인덱스 퀘스트 정보로 이동

                m_pQuestSlots[iIndex].AccessQuest = null; // 다음 인덱스 퀘스트 정보를 비운다

                m_pQuestSlots[i].AccessText.text = m_pQuestSlots[iIndex].AccessText.text; // 동일하게 퀘스트 주 제목정보 또한 다음 인덱스 퀘스트 정보에서 

                // 이전 인덱스 정보로 이동한다.
            }
        }

        int iIndexs = m_iEnableCount - 1;

        if (iIndexs <= 0) // 혹시 인덱스의 접근이 음수가 나올시 
            iIndexs = 0; // 0으로 고정 

        m_pQuestSlots[iIndexs].gameObject.SetActive(false); // 해당 인덱스의 슬롯 오브젝트를 비활성화 한다

        --m_iEnableCount; // 감소시킨다.

        if (m_iEnableCount <= 0) // 음수가 나온다면 
            m_iEnableCount = 0; // 0으로 고정
    }

    private void OnDestroy()
    {
        m_pQuestSlots = null;

        m_pSubWindow = null;
    }
}