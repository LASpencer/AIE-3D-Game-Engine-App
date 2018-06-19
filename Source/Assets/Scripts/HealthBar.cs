using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class HealthBar : MonoBehaviour {

    public float health;
    public float maxHealth;
    public float damage = 0;
    public float drainRate = 0.5f;

    [SerializeField]
    Image backgroundSprite;
    [SerializeField]
    Image damageSprite;
    [SerializeField]
    Image healthSprite;

    [SerializeField]
    Animator anim;

	// Use this for initialization
	void Start () {
        backgroundSprite.fillAmount = 1;
        damageSprite.fillAmount = 1;
        healthSprite.fillAmount = 1;

        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO only appear after damage, then fade away

        float healthProportion = health / maxHealth;
        float damageProportion = damage / maxHealth;

        anim.SetFloat("HealthProportion", healthProportion);
        anim.SetFloat("DamageProportion", damageProportion);

        healthSprite.fillAmount = healthProportion;
        damageSprite.fillAmount = healthProportion + damageProportion;

	}

    public void InflictDamage(float amount)
    {
        damage += amount;
        health -= amount;
        anim.SetTrigger("Damaged");
    }
}
