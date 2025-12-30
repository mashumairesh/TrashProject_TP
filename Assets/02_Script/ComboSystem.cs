using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tmpCombo;
    [SerializeField] private Image resetGage;

    [SerializeField] private int ComboAmount;

    private IEnumerator cor_RestCombo;

    private void Awake()
    {
        if(tmpCombo==null)
            GetComponent<TextMeshProUGUI>();
    }



    public void GetUpdate()
    {
        CountingGage();
        if (cor_RestCombo != null)
            StopCoroutine(cor_RestCombo);
        cor_RestCombo = corFunc_RestCombo();
        StartCoroutine(cor_RestCombo);
    }

    public void RESTCOMBO()
    {
        ComboAmount = 0;
        if (cor_RestCombo != null)
            StopCoroutine(cor_RestCombo);
        UpdateVisual();
    }

    private void CountingGage()
    {
        resetGage.DOKill(false);
        resetGage.fillAmount = 1f;
        resetGage.DOFillAmount(0f, 2f);
    }

    private void UpdateVisual()
    {
        tmpCombo.text = ComboAmount.ToString();
    }

    //°¡ºñÁö´Â ±¦ÂúÀº°¡?
    private IEnumerator corFunc_RestCombo()
    {
        ComboAmount++;
        UpdateVisual();
        yield return new WaitForSeconds(2f);
        ComboAmount = 0;
        UpdateVisual();

    }



}
