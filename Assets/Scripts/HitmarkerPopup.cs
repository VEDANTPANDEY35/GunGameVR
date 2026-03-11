using UnityEngine;
using System.Collections;

public class HitmarkerPopup : MonoBehaviour
{
    public float showTime = 0.25f;

    public void ShowHitmarker()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(showTime);
        gameObject.SetActive(false);
    }
}