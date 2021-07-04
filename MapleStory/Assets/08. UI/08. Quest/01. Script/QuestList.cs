using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour // 퀘스트 포기 버튼을 하나 만들어주자 !!
{
    private QuestSlot[] m_pQuestSlots = null;

    private Questsub_Window m_pSubWindow = null;

    private int m_iEnableCount = 0;

    // Start is called before the first frame update
    void Awake() // 내용이 틀어진다 !! 
    {
        // 퀘스트 창에서 이걸 해결 본다 

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

        //m_pSubWindow.gameObject.SetActive(false);
    }   

    public void InsertQuest(QUEST _InsertQuest) // 여기서 손을 봐야 한다 ?? 만든다 ?? 라고 가야 되는데 ??
    {
        if (null == m_pQuestSlots)
            return;

        for (int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (m_pQuestSlots[i].AccessQuest == null)
            {
                m_pQuestSlots[i].gameObject.SetActive(true);

                m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;

                m_pQuestSlots[i].AccessQuest = _InsertQuest;

                _InsertQuest.AccessQuestStart = true;

                m_pQuestSlots[i].AccessText.text = _InsertQuest.AccessQuestSlotName;

                ++m_iEnableCount;

                return;
            }
        }
    }

    public void RemoveQuest(QUEST _RemoveQuest) // 수정 해야 된다 !! 
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

    public void SortQusetList(QuestSlot _QusetSlot) // 100프로 버그 있을꺼임 
    {
        // 정렬순서 확인 할것 !!

        if (null == _QusetSlot)
            return;

        for (int i = 0; i < m_pQuestSlots.Length; ++i)
        {
            if (_QusetSlot.gameObject.activeSelf == false)
                continue;

            int iIndex = i;

            if(m_pQuestSlots[i] == _QusetSlot)
            {
                iIndex += 1;

                if (iIndex >= m_iEnableCount)
                    iIndex = m_iEnableCount;

                m_pQuestSlots[i].AccessQuest = null;

                m_pQuestSlots[i].AccessQuest = m_pQuestSlots[iIndex].AccessQuest;

                m_pQuestSlots[iIndex].AccessQuest = null;

                m_pQuestSlots[i].AccessText.text = m_pQuestSlots[iIndex].AccessText.text;
            }
        }

        int iIndexs = m_iEnableCount - 1;

        if (iIndexs <= 0)
            iIndexs = 0;

        m_pQuestSlots[iIndexs].gameObject.SetActive(false);

        --m_iEnableCount;

        if (m_iEnableCount <= 0)
            m_iEnableCount = 0;

        //m_pQuestSlots[iIndexs].gameObject.SetActive(false);

        //--m_iEnableCount;

        //if (m_iEnableCount <= 0)
        //    m_iEnableCount = 0;
    }

    private void OnDestroy()
    {
        m_pQuestSlots = null;

        m_pSubWindow = null;
    }
}




//public class QuestList : MonoBehaviour
//{
//    [SerializeField] private GameObject _QuestSlot = null;

//    private QuestSlot[] m_pQuestSlots = null;

//    private Questsub_Window m_pSubWindow = null;

//    //private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    // Start is called before the first frame update
//    void Awake()
//    {
//        // 퀘스트 창에서 이걸 해결 본다 

//        if (null != m_pQuestSlots)
//            return;

//        GameObject _ParentGameObject = transform.parent.gameObject;

//        m_pSubWindow = _ParentGameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

//        m_pSubWindow.gameObject.SetActive(false);

//        //GameObject[] _GameObjects = new GameObject[transform.childCount];

//        //m_pQuestSlots = new QuestSlot[transform.childCount];

//        //for (int i = 0; i < transform.childCount; ++i)
//        //{
//        //    _GameObjects[i] = transform.GetChild(i).gameObject;

//        //    m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();

//        //    m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;
//        //}

//        //_GameObjects = null;
//    }

//    public void InsertQuest(QUEST _InsertQuest) // 여기서 손을 봐야 한다 ?? 만든다 ?? 라고 가야 되는데 ??
//    {
//        if (null == m_pQuestSlots)
//            return;






//        //for(int i = 0; i < m_pQuestSlots.Length; ++i)
//        //{
//        //    if(m_pQuestSlots[i].AccessQuest == null)
//        //    {
//        //        m_pQuestSlots[i].gameObject.SetActive(true);

//        //        m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;

//        //        m_pQuestSlots[i].AccessQuest = _InsertQuest;

//        //        _InsertQuest.AccessQuestStart = true;

//        //        m_pQuestSlots[i].AccessText.text = _InsertQuest.AccessQuestSlotName;

//        //        break;
//        //    }
//        //}
//    }

//    public void RemoveQuest(QUEST _RemoveQuest) // 수정 해야 된다 !! 
//    {
//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//            {
//                if (m_pQuestSlots[i].AccessQuest == _RemoveQuest)
//                {
//                    m_pQuestSlots[i].AccessQuest = null;

//                    m_pQuestSlots[i].gameObject.SetActive(false);

//                    _RemoveQuest.AccessQuestStart = false;

//                    m_pSubWindow.gameObject.SetActive(false); // nullptr

//                    break;
//                }
//            }
//        }
//    }

//    public void QuestListUpdate()
//    {
//        if (null == m_pQuestSlots)
//            return;

//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//                m_pQuestSlots[i].AccessQuest.QuestUpdate();
//        }
//    }

//    public void DisableStart()
//    {
//        if (null != m_pQuestSlots)
//            return;

//        GameObject[] _GameObjects = new GameObject[transform.childCount];

//        m_pQuestSlots = new QuestSlot[transform.childCount];

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            _GameObjects[i] = transform.GetChild(i).gameObject;

//            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();
//        }

//        _GameObjects = null;
//    }

//    private void OnDestroy()
//    {
//        m_pQuestSlots = null;

//        m_pSubWindow = null;

//        //m_pGameObjectManager = null;
//    }
//}
















//public class QuestList : MonoBehaviour
//{
//    private QuestSlot[] m_pQuestSlots = null;

//    private Questsub_Window m_pSubWindow = null;

//    // Start is called before the first frame update
//    void Awake()
//    {
//        // 퀘스트 창에서 이걸 해결 본다 

//        if (null != m_pQuestSlots)
//            return;

//        GameObject _ParentGameObject = transform.parent.gameObject;

//        m_pSubWindow = _ParentGameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

//        m_pSubWindow.gameObject.SetActive(false);

//        GameObject[] _GameObjects = new GameObject[transform.childCount];

//        m_pQuestSlots = new QuestSlot[transform.childCount];

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            _GameObjects[i] = transform.GetChild(i).gameObject;

//            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();

//            m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;
//        }

//        _GameObjects = null;
//    }

//    public void InsertQuest(QUEST _InsertQuest) // 여기서 손을 봐야 한다 ?? 만든다 ?? 라고 가야 되는데 ??
//    {
//        if (null == m_pQuestSlots)
//            return;

//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest == null)
//            {
//                m_pQuestSlots[i].gameObject.SetActive(true);

//                m_pQuestSlots[i].AccessQuest = _InsertQuest;

//                _InsertQuest.AccessQuestStart = true;

//                m_pQuestSlots[i].AccessText.text = _InsertQuest.AccessQuestSlotName;

//                break;
//            }
//        }
//    }

//    public void RemoveQuest(QUEST _RemoveQuest)
//    {
//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//            {
//                if (m_pQuestSlots[i].AccessQuest == _RemoveQuest)
//                {
//                    m_pQuestSlots[i].AccessQuest = null;

//                    m_pQuestSlots[i].gameObject.SetActive(false);

//                    _RemoveQuest.AccessQuestStart = false;

//                    m_pSubWindow.gameObject.SetActive(false); // nullptr

//                    break;
//                }
//            }
//        }
//    }

//    public void QuestListUpdate()
//    {
//        if (null == m_pQuestSlots)
//            return;

//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//                m_pQuestSlots[i].AccessQuest.QuestUpdate();
//        }
//    }

//    public void DisableStart()
//    {
//        if (null != m_pQuestSlots)
//            return;

//        GameObject[] _GameObjects = new GameObject[transform.childCount];

//        m_pQuestSlots = new QuestSlot[transform.childCount];

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            _GameObjects[i] = transform.GetChild(i).gameObject;

//            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();
//        }

//        _GameObjects = null;
//    }

//    private void OnDestroy()
//    {
//        m_pQuestSlots = null;

//        m_pSubWindow = null;
//    }
//}






//public class QuestList : MonoBehaviour
//{
//    private QuestSlot[] m_pQuestSlots = null;

//    private Questsub_Window m_pSubWindow = null;

//    // Start is called before the first frame update
//    void Awake()
//    {
//        // 퀘스트 창에서 이걸 해결 본다 

//        if (null != m_pQuestSlots)
//            return;

//        GameObject _ParentGameObject = transform.parent.gameObject;

//        m_pSubWindow = _ParentGameObject.transform.Find("Quest Sub Window").GetComponent<Questsub_Window>();

//        m_pSubWindow.gameObject.SetActive(false);

//        GameObject[] _GameObjects = new GameObject[transform.childCount];

//        m_pQuestSlots = new QuestSlot[transform.childCount];

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            _GameObjects[i] = transform.GetChild(i).gameObject;

//            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();

//            m_pQuestSlots[i].AccessQuestSubWindow = m_pSubWindow;
//        }

//        _GameObjects = null;
//    }

//    public void InsertQuest(QUEST _InsertQuest)
//    {
//        if (null == m_pQuestSlots)
//            return;

//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest == null)
//            {
//                m_pQuestSlots[i].gameObject.SetActive(true);

//                m_pQuestSlots[i].AccessQuest = _InsertQuest;

//                _InsertQuest.AccessQuestStart = true;

//                m_pQuestSlots[i].AccessText.text = _InsertQuest.AccessQuestSlotName;

//                break;
//            }
//        }
//    }

//    public void RemoveQuest(QUEST _RemoveQuest)
//    {
//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//            {
//                if (m_pQuestSlots[i].AccessQuest == _RemoveQuest)
//                {
//                    m_pQuestSlots[i].AccessQuest = null;

//                    m_pQuestSlots[i].gameObject.SetActive(false);

//                    _RemoveQuest.AccessQuestStart = false;

//                    m_pSubWindow.gameObject.SetActive(false);

//                    break;
//                }
//            }
//        }
//    }

//    public void QuestListUpdate()
//    {
//        if (null == m_pQuestSlots)
//            return;

//        for (int i = 0; i < m_pQuestSlots.Length; ++i)
//        {
//            if (m_pQuestSlots[i].AccessQuest != null)
//                m_pQuestSlots[i].AccessQuest.QuestUpdate();
//        }
//    }

//    public void DisableStart()
//    {
//        if (null != m_pQuestSlots)
//            return;

//        GameObject[] _GameObjects = new GameObject[transform.childCount];

//        m_pQuestSlots = new QuestSlot[transform.childCount];

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            _GameObjects[i] = transform.GetChild(i).gameObject;

//            m_pQuestSlots[i] = _GameObjects[i].GetComponent<QuestSlot>();
//        }

//        _GameObjects = null;
//    }

//    private void OnDestroy()
//    {
//        m_pQuestSlots = null;

//        m_pSubWindow = null;
//    }
//}