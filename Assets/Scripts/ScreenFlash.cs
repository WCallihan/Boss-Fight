using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour {

    [SerializeField] private Health playerHealth;
    [SerializeField] private Image panel;
    [SerializeField] private float totalFlashTime;

    private void OnEnable() {
        playerHealth.TookDamage += StartFlash;
    }

    private void OnDisable() {
        playerHealth.TookDamage -= StartFlash;
    }

    private void StartFlash(int currentHealth) {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash() {
        float flashAlpha = 0.1176471f; //0.1176471 == 30

        //fade flash in
        float fadeTime = totalFlashTime / 2;
        float timer = 0;
        Color tempColor = panel.color;
        while (panel.color.a < flashAlpha) {
            tempColor.a = Mathf.Lerp(0, flashAlpha, timer / fadeTime);
            panel.color = tempColor;
            timer += Time.deltaTime;
            yield return null;
        }

        //fade flash out
        timer = 0;
        tempColor = panel.color;
        while(panel.color.a > 0) {
            tempColor.a = Mathf.Lerp(0, flashAlpha, timer / fadeTime);
            panel.color = tempColor;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}