using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{

    private static SceneTransition instance;
    private static bool shouldPlayOpeningAnimation = false;
    private static bool isLoading;

    [SerializeField] GameObject buttonsBlocker;

    private Animator componentAnimator;
    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        if (!isLoading)
        {
            instance.buttonsBlocker.SetActive(true);
            isLoading = true;
            instance.componentAnimator.SetTrigger("SceneEnd");

            instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);

            // Чтобы сцена не начала переключаться пока играет анимация closing:
            instance.loadingSceneOperation.allowSceneActivation = false;
        }
    }

    private void Start()
    {
        instance = this;

        componentAnimator = GetComponent<Animator>();

        instance.buttonsBlocker.SetActive(false);
        if (shouldPlayOpeningAnimation)
        {
            componentAnimator.SetTrigger("SceneStart");

            // Чтобы если следующий переход будет обычным SceneManager.LoadScene, не проигрывать анимацию opening:
            shouldPlayOpeningAnimation = false;
        }
        StartCoroutine(DestroyCoroutine());
    }
    public void OnAnimationOver()
    {
        isLoading = false;
        // Чтобы при открытии сцены, куда мы переключаемся, проигралась анимация opening:
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }


    IEnumerator DestroyCoroutine()
    {

        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(true);
    }
}