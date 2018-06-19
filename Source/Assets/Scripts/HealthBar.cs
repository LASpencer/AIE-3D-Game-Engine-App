using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RectTransform))]
public class HealthBar : MonoBehaviour {

    public float health;
    public float maxHealth;
    public float damage = 0;
    public float drainRate = 0.5f;

    bool deathFinished = false;
    public bool DeathFinished {  get { return deathFinished; } }

    [SerializeField]
    Image backgroundSprite;
    [SerializeField]
    Image damageSprite;
    [SerializeField]
    Image healthSprite;

    [SerializeField]
    ParticleSystem deathEffect;

    [SerializeField]
    ParticleSystem hitEffect;

    [SerializeField]
    Animator anim;

    RectTransform rect;

	// Use this for initialization
	void Start () {
        backgroundSprite.fillAmount = 1;
        damageSprite.fillAmount = 1;
        healthSprite.fillAmount = 1;

        anim = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
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
        SpawnHitEffect(health / maxHealth);
        damage += Mathf.Min(amount, health);
        health = Mathf.Max(0, health - amount);
        anim.SetTrigger("Damaged");
    }

    public void OnDeath()
    {
        deathEffect.Play();
    }

    public void OnFinishDeath()
    {
        deathFinished = true;
        deathEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void SpawnHitEffect(float barProportion)
    {
        Vector3 pos = new Vector3(rect.rect.width * (barProportion - 0.5f), 0, 0);
        GameObject hitClone = GameObject.Instantiate(hitEffect.gameObject, this.transform);
        hitClone.transform.localPosition = pos;
        GameObject.Destroy(hitClone, 1);
    }
}
