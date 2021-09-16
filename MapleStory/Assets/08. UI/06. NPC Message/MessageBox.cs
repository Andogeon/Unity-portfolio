using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour // 실제 NPC 클릭시 생성 되는 메세지 박스 클래스입니다.
{
    [SerializeField] private float _TextSpeed = 0.0f;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private Image _NameImage = null;

    [SerializeField] private Image _NameBarImage = null;

    private Text m_pText = null;

    private Image m_pMainTexture = null;

    private RectTransform m_pRectTransform = null;

    private MessageButton m_pPrevButton = null;

    private MessageButton m_pNextButton = null;

    private MessageButton m_pYesButton = null;

    private MessageButton m_pNoButton = null;

    private NPC m_pNpc = null;

    private NPC m_pNextQuestNpc = null;

    private string m_strMessage = string.Empty;

    private char[] m_string = null;

    private float m_fIndex = 0;

    public string AccessMessage // 메세지 박스에 출력될 메세지를 전달 받는 프로퍼티입니다.
    {
        get { return m_strMessage; }

        set 
        { 
            m_strMessage = value; // 메세지에 출력될 문자

            m_string = new char[m_strMessage.Length + 1]; // 실제로 메세지 박스에 출력될 문자의 할당
        }
    }

    // 메세지 박스에 출력될 NPC 스프라이트 이미지를 외부로 넘겨주는 프로퍼티입니다.

    public Image AccessMainImage
    {
        get 
        {
            if (null == m_pMainTexture)
                m_pMainTexture = transform.Find("NPC Main Texture").GetComponent<Image>();

            return m_pMainTexture; 
        }
    }

    public NPC AccessNpc // 메세지 박스에 출력할 NPC 설정과 버튼에게 NPC를 넘겨주는 프로퍼티입니다
    {
        get { return m_pNpc; }

        set 
        {
            m_pNpc = value;

            if(null == m_pPrevButton || null == m_pNextButton || m_pYesButton == null)
            {
                m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

                m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();

                m_pYesButton = transform.Find("Yes Button").gameObject.GetComponent<MessageButton>();

                m_pNoButton = transform.Find("No Button").gameObject.GetComponent<MessageButton>();
            }

            m_pYesButton.AccessNpc = m_pPrevButton.AccessNpc = m_pNextButton.AccessNpc = m_pNpc;
        }
    }

    public NPC AccessNextQuestNpc
    {
        get { return m_pNextQuestNpc; }

        set
        {
            m_pNextQuestNpc = value;

            if (m_pYesButton == null)
                m_pYesButton = transform.Find("Yes Button").gameObject.GetComponent<MessageButton>();

            m_pYesButton.AccessNextQuestNPC = m_pNextQuestNpc;
        }
    }

    public float AccessIndex // 텍스트 메세지 속도를 제어하는 프로퍼티입니다.
    {
        set { m_fIndex = value; }
    }


    // NPC의 이름, 그리고 이름 받침대를 메세지 박스에 넣어주기 위한 함수입니다.
    public void SetNameImage(Sprite _NameSprite, Sprite _NameBarSprite)
    {
        if (null == _NameSprite || null == _NameBarSprite)
            return;

        _NameImage.sprite = _NameSprite;

        _NameBarImage.sprite = _NameBarSprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_pText = GetComponentInChildren<Text>();

        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform _ChildObject = transform.GetChild(i);

            if(_ChildObject.name == "NPC Main Texture")
            {
                m_pMainTexture = _ChildObject.GetComponent<Image>();

                break;
            }
        }

        m_pRectTransform = GetComponent<RectTransform>();

        m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

        m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();

        m_pYesButton = transform.Find("Yes Button").gameObject.GetComponent<MessageButton>();

        m_pNoButton = transform.Find("No Button").gameObject.GetComponent<MessageButton>();

        m_pYesButton.AccessNpc = m_pPrevButton.AccessNpc = m_pNextButton.AccessNpc = m_pNpc;
    }

    // Update is called once per frame
    void Update()
    {
        StringUpdate();
    }

    private void StringUpdate() // 텍스트 한 문자 씩 출력하기 위해 구성한 함수입니다.
    {
        if (m_strMessage == string.Empty || (int)m_fIndex >= m_strMessage.Length)
            return;

        m_fIndex += _TextSpeed * Time.deltaTime; // 총 길이 

        for (int i = 0; i < (int)m_fIndex; ++i)
            m_string[i] = m_strMessage[i];

        string _Outstring = new string(m_string);

        m_pText.text = _Outstring;
    }

    public void ClearText() // 텍스트를 모두 초기화 하는 함수입니다.
    {
        m_pText.text = string.Empty;

        m_fIndex = 0.0f;
    }

    public void DisableButton() // 텍스트 이동과 관련된 모든 오브젝트를 비활성화 하는 함수입니다.
    {
        if (null == m_pPrevButton || null == m_pNextButton)
        {
            m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

            m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();
        }

        m_pPrevButton.gameObject.SetActive(false);

        m_pNextButton.gameObject.SetActive(false);
    }

    public void EnabledPrevButton() // 텍스트를 이전으로 넘기는 버튼을 활성화 하는 함수입니다.
    {
        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == true)
            return;

        m_pPrevButton.gameObject.SetActive(true);
    }

    public void EnabledNextButton() // 텍스트를 다음으로 넘기는 버튼을 활성화 하는 함수입니다.
    {
        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == true)
            return;

        m_pNextButton.gameObject.SetActive(true);
    }

    public void DisablePrevButton() // 텍스트를 이전으로 넘기는 버튼을 비활성화 하는 함수입니다.
    {
        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == false)
            return;

        m_pPrevButton.gameObject.SetActive(false);
    }

    public void DisableNextButton() // 텍스트를 다음으로 넘기는 버튼을 비활성화 하는 함수입니다.
    {
        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == false)
            return;

        m_pNextButton.gameObject.SetActive(false);
    }

    public void EnabledQuestButton() // 퀘스트의 수락 여부를 활성화 하는 함수입니다.
    {
        if (null == m_pYesButton || m_pNoButton == null)
            return;

        m_pYesButton.gameObject.SetActive(true);

        m_pNoButton.gameObject.SetActive(true);
    }

    public void DisableQuestButton() // 퀘스트의 수락 여부를 비활성화 하는 함수입니다.
    {
        if (null == m_pYesButton || m_pNoButton == null)
            return;

        m_pYesButton.gameObject.SetActive(false);

        m_pNoButton.gameObject.SetActive(false);
    }

    public void ChangeYesButton() // 퀘스트 수락버튼 기능을 바꾸는 함수입니다.
    {
        if (null == m_pYesButton || m_pYesButton.gameObject.activeSelf == false)
            return;

        m_pYesButton.ChangeButton();
    }

    public void ResetYesButton() // 다시 퀘스트 수락 버튼을 초기화 하는 함수입니다.
    {
        if (null == m_pYesButton || m_pYesButton.gameObject.activeSelf == false)
            return;

        m_pYesButton.ResetButton();
    }

    private void OnDestroy()
    {
        m_pText = null;

        m_pMainTexture = null;

        m_pRectTransform = null;

        m_pPrevButton = null;

        m_pNextButton = null;

        m_pYesButton = null;

        m_pNoButton = null;

        m_pNpc = null;

        m_strMessage = string.Empty;
    }
}
