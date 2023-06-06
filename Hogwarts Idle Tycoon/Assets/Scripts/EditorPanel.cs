using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorPanel : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] TMP_Text lessonDurationText;
    [SerializeField] TMP_Text lessonDuration;
    [SerializeField] TMP_Text lessonExperienceText;
    [SerializeField] TMP_Text lessonExperience;
    [SerializeField] TMP_Text areaName;
    [SerializeField] TMP_Text UpgradeCost;
    [SerializeField] TMP_Text areaCapacity;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    public void SetEditorPanel(string name, float xp,float lessonD, string lessonDText, string xpText, string UpgradeText,int capacity)
    {
        areaName.text = name;
        lessonExperience.text = xp.ToString();
        lessonExperienceText.text = xpText;
        lessonDuration.text = lessonD.ToString()+ " Seconds" ;
        lessonDurationText.text = lessonDText;
        UpgradeCost.text = UpgradeText;
        areaCapacity.text = capacity.ToString()+"/9";
        ShowHide(true);

    }
    public void ShowHide(bool active)
    {
        anim.SetBool("Active", active);
    }
}
