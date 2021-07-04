using UnityEngine;

public class NextScene : MonoBehaviour
{
    [SerializeField] string _NextSceneName = null;

    public void MovingNextScene()
    {
        if (null == _NextSceneName)
            return;

        Loading.LoadScene(_NextSceneName);
    } 
}
