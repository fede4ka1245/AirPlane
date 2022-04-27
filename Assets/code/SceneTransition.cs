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

            // ����� ����� �� ������ ������������� ���� ������ �������� closing:
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

            // ����� ���� ��������� ������� ����� ������� SceneManager.LoadScene, �� ����������� �������� opening:
            shouldPlayOpeningAnimation = false;
        }
        StartCoroutine(DestroyCoroutine());
    }
    public void OnAnimationOver()
    {
        isLoading = false;
        // ����� ��� �������� �����, ���� �� �������������, ����������� �������� opening:
        shouldPlayOpeningAnimation = true;

        loadingSceneOperation.allowSceneActivation = true;
    }


    IEnumerator DestroyCoroutine()
    {

        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(true);
    }
}