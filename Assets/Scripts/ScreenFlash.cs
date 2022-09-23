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

    private void StartFlash(int damage) {
        StartCoroutine(Flash(damage));
    }

    private IEnumerator Flash(int damage) {
        //0.1176471 == 30
        float flashAlpha = 0.1176471f * damage; //IMPORTANT: this assumes that all received damage values are either 1 or 2

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