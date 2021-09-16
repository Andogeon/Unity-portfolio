using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 관련 퀘스트 클래스입니다.

public class MonterQuest : QUEST
{
    //[SerializeField] private MONTER _QuestMonter = null;

    [SerializeField] private Sprite _QuestMonterSprite = null;

    private int m_iDeleteConut = 0;

    public Sprite AccessQuestMonterSprite
    {
        get { return _QuestMonterSprite; }

        set { _QuestMonterSprite = value; }
    }

    public int AccessDeleteCount
    {
        get { return m_iDeleteConut; }

        set 
        {
            m_iDeleteConut = value;

            if(m_iDeleteConut >= _Count)
                m_bIsComple = true;
        }
    }

    private void Awake()
    {
        m_bIsComple = false;
    }
}
