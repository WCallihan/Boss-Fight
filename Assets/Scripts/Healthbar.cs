using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

    [SerializeField] private Health health;
    [SerializeField] private Image damageTakenBar;
    [SerializeField] private Image currentHealthBar;

    private void OnEnable() {
        health.TookDamage += UpdateBar;
    }

    private void OnDisable() {
        health.TookDamage -= UpdateBar;
    }

    private void UpdateBar(int currentHealth) {
        //update the current health bar
        currentHealthBar.fillAmount = (float)currentHealth / (float)health.MaxHealth;
        //update the damage taken bar
        StartCoroutine(LerpDamageTakenBar());
    }

    private IEnumerator LerpDamageTakenBar() {
        //wait a second so the damage amount is easy to see
        yield return new WaitForSeconds(1);

        float timeElapsed = 0;
        float lerpDuration = 1.5f;
        //have the damage taken bar catch up with the current health bar over time
        while(timeElapsed < lerpDuration) {
            damageTakenBar.fillAmount = Mathf.Lerp(damageTakenBar.fillAmount, currentHealthBar.fillAmount, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //set the bars equal at the end just in case
        damageTakenBar.fillAmount = currentHealthBar.fillAmount;
    }
}