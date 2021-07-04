using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private static string _NextScene = null;

    //[SerializeField] private Image _LoadingImage = null;

    public static bool LoadScene(string _ChaneSceceName)
    {
        if (_ChaneSceceName == null)
            return false;

        _NextScene = _ChaneSceceName;

        SceneManager.LoadScene("Game Loading");

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation _Op = SceneManager.LoadSceneAsync(_NextScene);

        _Op.allowSceneActivation = false;


        while(!_Op.isDone)
        {
            if(_Op.progress >= 0.9f)
            {
                _Op.allowSceneActivation = true;

                yield break;
            }

            yield return null;
        }

        

        //float _TimeAcc = 0.0f;

        //while(!_Op.isDone)
        //{
        //    yield return null;

        //    if (_Op.progress < 0.9f)
        //    {
        //        _TimeAcc += Time.deltaTime;

        //        _LoadingImage.fillAmount = Mathf.Lerp(_LoadingImage.fillAmount, _Op.progress, _TimeAcc);

        //        if (_LoadingImage.fillAmount >= _Op.progress)
        //            _TimeAcc = 0.0f;
        //    }
        //    else
        //    {
        //        _LoadingImage.fillAmount = Mathf.Lerp(_LoadingImage.fillAmount, 1f, _TimeAcc);

        //        if (_LoadingImage.fillAmount == 1.0f)
        //        {
        //            _Op.allowSceneActivation = true;

        //            yield break;
        //        }
        //    }
        //}
    }
}
