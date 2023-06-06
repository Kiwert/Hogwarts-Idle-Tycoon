using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentUI : MonoBehaviour
{
    [SerializeField] GameObject fillPanel;
    [SerializeField] Image fillBar;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadingBar(float duration)
    {
        StartCoroutine(FillActionBar(duration));
    }
    IEnumerator FillActionBar(float fillDuration)
    {
        yield return new WaitForSeconds(.3f);
        fillBar.fillAmount = 0;
        fillPanel.SetActive(true);
        float elapsedTime = 0f;
        float startFillAmount = fillBar.fillAmount;
        float targetFillAmount = 1f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fillDuration);
            float filledAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);

            fillBar.fillAmount = filledAmount;

            yield return null;
        }


        fillBar.fillAmount = targetFillAmount;
        fillPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
