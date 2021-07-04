using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    //[SerializeField] private AudioClip _ClickSound = null;

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

    //private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private string m_strMessage = string.Empty;

    private char[] m_string = null;

    private float m_fIndex = 0;

    public string AccessMessage
    {
        get { return m_strMessage; }

        set 
        { 
            m_strMessage = value;

            m_string = new char[m_strMessage.Length + 1];
        }
    }

    public Image AccessMainImage
    {
        get 
        {
            if (null == m_pMainTexture)
                m_pMainTexture = transform.Find("NPC Main Texture").GetComponent<Image>();

            return m_pMainTexture; 
        }
    }

    public NPC AccessNpc
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

    public float AccessIndex
    {
        set { m_fIndex = value; }
    }

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

        //m_pSoundManager.AddSound("Click Sound", _ClickSound, SoundType.Sound_Effect);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 _WorldMousePosition = Input.mousePosition;

        //Ray2D _Ray2D = new Ray2D();

        //_Ray2D.origin = _WorldMousePosition;

        //_Ray2D.direction = Vector2.zero;

        //RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

        //if (_Hit2D.collider != null && Input.GetMouseButton(0))
        //    m_pRectTransform.position = Input.mousePosition;

        StringUpdate();
    }

    private void StringUpdate()
    {
        if (m_strMessage == string.Empty || (int)m_fIndex >= m_strMessage.Length)
            return;

        m_fIndex += _TextSpeed * Time.deltaTime; // 총 길이 

        for (int i = 0; i < (int)m_fIndex; ++i)
            m_string[i] = m_strMessage[i];

        string _Outstring = new string(m_string);

        m_pText.text = _Outstring;
    }

    public void ClearText()
    {
        m_pText.text = string.Empty;

        m_fIndex = 0.0f;
    }

    public void DisableButton()
    {
        if (null == m_pPrevButton || null == m_pNextButton)
        {
            m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

            m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();
        }

        m_pPrevButton.gameObject.SetActive(false);

        m_pNextButton.gameObject.SetActive(false);
    }

    public void EnabledPrevButton()
    {
        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == true)
            return;

        m_pPrevButton.gameObject.SetActive(true);
    }

    public void EnabledNextButton()
    {
        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == true)
            return;

        m_pNextButton.gameObject.SetActive(true);
    }

    public void DisablePrevButton()
    {
        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == false)
            return;

        m_pPrevButton.gameObject.SetActive(false);
    }

    public void DisableNextButton()
    {
        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == false)
            return;

        m_pNextButton.gameObject.SetActive(false);
    }

    public void EnabledQuestButton()
    {
        if (null == m_pYesButton || m_pNoButton == null)
            return;

        m_pYesButton.gameObject.SetActive(true);

        m_pNoButton.gameObject.SetActive(true);
    }

    public void DisableQuestButton()
    {
        if (null == m_pYesButton || m_pNoButton == null)
            return;

        m_pYesButton.gameObject.SetActive(false);

        m_pNoButton.gameObject.SetActive(false);
    }

    public void ChangeYesButton()
    {
        if (null == m_pYesButton || m_pYesButton.gameObject.activeSelf == false)
            return;

        m_pYesButton.ChangeButton();
    }

    public void ResetYesButton()
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















//public class MessageBox : MonoBehaviour
//{
//    [SerializeField] private float _TextSpeed = 0.0f;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    private Text m_pText = null;

//    private Image m_pMainTexture = null;

//    private RectTransform m_pRectTransform = null;

//    private MessageButton m_pPrevButton = null;

//    private MessageButton m_pNextButton = null;

//    private MessageButton m_pYesButton = null;

//    private MessageButton m_pNoButton = null;

//    private NPC m_pNpc = null;

//    private string m_strMessage = string.Empty;

//    private char[] m_string = null;

//    private float m_fIndex = 0;

//    public string AccessMessage
//    {
//        get { return m_strMessage; }

//        set
//        {
//            m_strMessage = value;

//            m_string = new char[m_strMessage.Length + 1];
//        }
//    }

//    public Image AccessMainImage
//    {
//        get
//        {
//            if (null == m_pMainTexture)
//                m_pMainTexture = transform.Find("NPC Main Texture").GetComponent<Image>();

//            return m_pMainTexture;
//        }
//    }

//    public NPC AccessNpc
//    {
//        get { return m_pNpc; }

//        set { m_pNpc = value; }
//    }

//    public float AccessIndex
//    {
//        set { m_fIndex = value; }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pText = GetComponentInChildren<Text>();

//        for (int i = 0; i < transform.childCount; ++i)
//        {
//            Transform _ChildObject = transform.GetChild(i);

//            if (_ChildObject.name == "NPC Main Texture")
//            {
//                m_pMainTexture = _ChildObject.GetComponent<Image>();

//                break;
//            }
//        }

//        m_pRectTransform = GetComponent<RectTransform>();

//        m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

//        m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();

//        m_pYesButton = transform.Find("Yes Button").gameObject.GetComponent<MessageButton>();

//        m_pNoButton = transform.Find("No Button").gameObject.GetComponent<MessageButton>();

//        m_pYesButton.AccessNpc = m_pPrevButton.AccessNpc = m_pNextButton.AccessNpc = m_pNpc;

//        //if (m_pNpc != null)
//        //    m_pYesButton.AccessNpc = m_pPrevButton.AccessNpc = m_pNextButton.AccessNpc = m_pNpc;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 _WorldMousePosition = Input.mousePosition;

//        Ray2D _Ray2D = new Ray2D();

//        _Ray2D.origin = _WorldMousePosition;

//        _Ray2D.direction = Vector2.zero;

//        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

//        //if (_Hit2D.collider != null && Input.GetMouseButton(0))
//        //    m_pRectTransform.position = Input.mousePosition;

//        StringUpdate();
//    }

//    private void StringUpdate()
//    {
//        if (m_strMessage == string.Empty || (int)m_fIndex >= m_strMessage.Length)
//            return;

//        m_fIndex += _TextSpeed * Time.deltaTime; // 총 길이 

//        for (int i = 0; i < (int)m_fIndex; ++i)
//            m_string[i] = m_strMessage[i];

//        string _Outstring = new string(m_string);

//        m_pText.text = _Outstring;
//    }

//    public void ClearText()
//    {
//        m_pText.text = string.Empty;

//        m_fIndex = 0.0f;
//    }

//    public void DisableButton()
//    {
//        if (null == m_pPrevButton || null == m_pNextButton)
//        {
//            m_pPrevButton = transform.Find("Prev Button").gameObject.GetComponent<MessageButton>();

//            m_pNextButton = transform.Find("Next Button").gameObject.GetComponent<MessageButton>();
//        }

//        m_pPrevButton.gameObject.SetActive(false);

//        m_pNextButton.gameObject.SetActive(false);
//    }

//    public void EnabledPrevButton()
//    {
//        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == true)
//            return;

//        m_pPrevButton.gameObject.SetActive(true);
//    }

//    public void EnabledNextButton()
//    {
//        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == true)
//            return;

//        m_pNextButton.gameObject.SetActive(true);
//    }

//    public void DisablePrevButton()
//    {
//        if (null == m_pPrevButton || m_pPrevButton.gameObject.activeSelf == false)
//            return;

//        m_pPrevButton.gameObject.SetActive(false);
//    }

//    public void DisableNextButton()
//    {
//        if (null == m_pNextButton || m_pNextButton.gameObject.activeSelf == false)
//            return;

//        m_pNextButton.gameObject.SetActive(false);
//    }

//    public void EnabledQuestButton()
//    {
//        if (null == m_pYesButton || m_pNoButton == null)
//            return;

//        m_pYesButton.gameObject.SetActive(true);

//        m_pNoButton.gameObject.SetActive(true);
//    }

//    public void DisableQuestButton()
//    {
//        if (null == m_pYesButton || m_pNoButton == null)
//            return;

//        m_pYesButton.gameObject.SetActive(false);

//        m_pNoButton.gameObject.SetActive(false);
//    }

//    private void OnDestroy()
//    {
//        m_pText = null;

//        m_pMainTexture = null;

//        m_pRectTransform = null;

//        m_pPrevButton = null;

//        m_pNextButton = null;

//        m_pYesButton = null;

//        m_pNoButton = null;

//        m_pNpc = null;

//        m_strMessage = string.Empty;
//    }
//}
