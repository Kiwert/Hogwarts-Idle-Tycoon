using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockPanel : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] TMP_Text areaName;
    [SerializeField] TMP_Text unlockText;
    [SerializeField] TMP_Text areacost;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetUnlockPanel(string name,float cost )
    {
        areaName.text = name;
        areacost.text = cost.ToString();
        unlockText.text = "Unlock the " + name + " for :";
        ShowHide(true);

    }
    public void ShowHide(bool active)
    {
        anim.SetBool("Active", active);
    }
}
