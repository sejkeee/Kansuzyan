using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Sprite[] enemySprites = new Sprite[18];
    

    private GameObject nowEnemy;
    private Animator mainCharAnim;

    public MenuCell attackCell;
    public MenuCell helpersCell;
    public MenuCell gainCell;
    public MenuCell miningCell;

    private const float multiplier = 1.1f;

    public int attackModifier = 1;
    public float attackModifierBase = 20;
    public float attackModifierCost = 20;
    public int helperModifier = 0;
    public float helperModifierBase = 20;
    public float helperModifierCost = 20;
    public int goldModifier = 1;
    public float goldModifierBase = 20;
    public float goldModifierCost = 20;
    public int miningModifier = 0;
    public float miningModifierBase = 20;
    public float miningModifierCost = 20;

    public float nowEnemyHealthMax;
    public float nowEnemyHealthMin;
    public float nowEnemyHealthCalculated;
    public float nowEnemyHealth;
    public float attack;

    public int gold = 0;
    public int goldInc = 10;

    public int damagePerSec = 0;
    public int goldPerSec = 0;

    public int wave = 1;
    public int mobsSlayed = 0;
    public int mobsinvawe = 10;

    public AudioSource attacking;

    private void Start()
    {
        mainCharAnim = mainCharacter.GetComponent<Animator>();
        attacking = GetComponent<AudioSource>();

        nowEnemyHealthMax = 120;
        nowEnemyHealthMin = 80;
        nowEnemyHealthCalculated = (int)Mathf.Round(Random.Range(nowEnemyHealthMin, nowEnemyHealthMax));
        nowEnemyHealth = nowEnemyHealthCalculated;
        attack = 5;

        attackCell.priceInt = (int)Mathf.Round(attackModifierCost);
        attackCell.scale = (int)Mathf.Ceil(attack / 10);
        attackCell.stat = (int)Mathf.Round(attack);
        helpersCell.priceInt = (int)Mathf.Round(helperModifierCost);
        helpersCell.scale = 5;
        helpersCell.stat = (int)Mathf.Round(damagePerSec);
        gainCell.priceInt = (int)Mathf.Round(goldModifierCost);
        gainCell.scale = 5;
        gainCell.stat = (int)Mathf.Round(goldInc);
        miningCell.priceInt = (int)Mathf.Round(miningModifierCost);
        miningCell.scale = 5;
        miningCell.stat = (int)Mathf.Round(goldPerSec);

        SpawnEnemies();
        StartCoroutine(ActionPerSec());
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
            DealDamage();
            attacking.Play();
        }

        if(nowEnemy != null && nowEnemyHealth <=0)
        {
            Die();
        }

        
    }

    private void Attack()
    {
        if(mainCharAnim != null)
        {
            mainCharAnim.SetTrigger("Attack");
        }
    }

    private void SpawnEnemies()
    {
        nowEnemy = Instantiate(enemy);
        nowEnemy.transform.parent = enemySpawner.transform;
        nowEnemy.GetComponent<SpriteRenderer>().sprite = enemySprites[Random.Range(0, enemySprites.Length)];
        nowEnemyHealth = nowEnemyHealthCalculated ;
    }

    private void DealDamage()
    {
        if(nowEnemy != null)
        {
            nowEnemy.GetComponentInChildren<Animator>().SetTrigger("TakeDamage");
            nowEnemyHealth -= attack;
        } 
    }

    private void Die()
    {
        gold += goldInc;
        mobsSlayed++;
        Destroy(nowEnemy);
        if (mobsSlayed >= mobsinvawe)
        {
            mobsSlayed -= mobsinvawe;
            wave++;
            nowEnemyHealthMin = nowEnemyHealthMin * Mathf.Pow(multiplier, wave);
            nowEnemyHealthMax = nowEnemyHealthMax * Mathf.Pow(multiplier, wave);
        }
        nowEnemyHealthCalculated = (int)Mathf.Round(Random.Range(nowEnemyHealthMin, nowEnemyHealthMax));

        SpawnEnemies();
    }

    public void UpgradeAttack()
    {
        if(gold >= attackModifierCost)
        {
            gold -= (int)Mathf.Round(attackModifierCost);
            attack += (int)Mathf.Ceil(attack / 10);
            attackModifier++;
            attackModifierCost = attackModifierBase * Mathf.Pow(multiplier, attackModifier);
            attackCell.priceInt = (int)Mathf.Round(attackModifierCost);
            attackCell.scale = (int)Mathf.Ceil(attack / 10); 
            attackCell.stat = (int)Mathf.Round(attack);
        }
    }

    public void UpgradeHelpers()
    {
        if(gold >= helperModifierCost)
        {
            gold -= (int)Mathf.Round(helperModifierCost);
            damagePerSec += 5;
            helperModifier++;
            helperModifierCost = helperModifierBase * Mathf.Pow(multiplier, helperModifier);
            helpersCell.priceInt = (int)Mathf.Round(helperModifierCost);
            helpersCell.scale = 5;
            helpersCell.stat = (int)Mathf.Round(damagePerSec);
        }
    }

    public void UpgradeGain()
    {
        if(gold >= goldModifierCost)
        {
            gold -= (int)Mathf.Round(goldModifierCost);
            goldInc += 5;
            goldModifier++;
            goldModifierCost = goldModifierBase * Mathf.Pow(multiplier, goldModifier);
            gainCell.priceInt = (int)Mathf.Round(goldModifierCost);
            gainCell.scale = 5;
            gainCell.stat = (int)Mathf.Round(goldInc);
        }
    }
    public void UpgradeMining()
    {
        if(gold >= miningModifierCost)
        {
            gold -= (int)Mathf.Round(miningModifierCost);
            goldPerSec += 5;
            miningModifier++;
            miningModifierCost = attackModifierBase * Mathf.Pow(multiplier, miningModifier);
            miningCell.priceInt = (int)Mathf.Round(miningModifierCost);
            miningCell.scale = 5;
            miningCell.stat = (int)Mathf.Round(goldPerSec);
        }
    }

    public IEnumerator ActionPerSec()
    {
        yield return new WaitForSeconds(1f);
        if (nowEnemy != null && damagePerSec!=0)
        {
            nowEnemy.GetComponentInChildren<Animator>().SetTrigger("TakeDamage");
            nowEnemyHealth -= damagePerSec;
        }
        gold += goldPerSec;
        StartCoroutine(ActionPerSec());
    }
}
