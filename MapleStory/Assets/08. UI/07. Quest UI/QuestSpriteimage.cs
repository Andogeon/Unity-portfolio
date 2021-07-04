using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSpriteimage : MonoBehaviour
{
    [SerializeField] private Sprite[] _SpriteImage = null;

    [SerializeField] private float _Speed = 0.0f;

    private float m_fIndex = 0;

    private Image m_pImage = null;

    // Start is called before the first frame update
    void Start()
    {
        m_pImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_fIndex += _Speed * Time.deltaTime;

        if (m_fIndex > _SpriteImage.Length - 1)
            m_fIndex = 0.0f;

        m_pImage.sprite = _SpriteImage[(int)m_fIndex];
    }
}
