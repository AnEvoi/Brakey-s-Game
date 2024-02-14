using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthSprite;

    [SerializeField] private float reducedSpeed = 2;
    [SerializeField] private float target = 1;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        if(currentHealth <= 0){
            target = 0f;
        }else{
            target = currentHealth / maxHealth;
        }
    }
    
    void Update() 
    {
        healthSprite.fillAmount = Mathf.MoveTowards(healthSprite.fillAmount, target, reducedSpeed * Time.deltaTime);
    }
}
