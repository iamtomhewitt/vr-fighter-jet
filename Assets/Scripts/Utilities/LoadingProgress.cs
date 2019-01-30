using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Utilities
{
public class LoadingProgress : MonoBehaviour
{
    public TextMesh progress;

    AsyncOperation async = null;
    public Object scene;

    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        async = SceneManager.LoadSceneAsync(scene.name);

        while (!async.isDone)
        {
            progress.text = (Mathf.Round((async.progress / 0.9f)*100f)).ToString("000") + "%";
            yield return null;
        }

        yield return async;
    }
}
}
