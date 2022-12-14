using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukkiMelee : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    [SerializeField] private Animator pukkiAnimator;
    private bool isAttackPressed = false;
    public int attackDamage = 30;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swingSound;


    private void Awake()
    {
        pukkiAnimator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttackPressed = true;

            playAttack();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            isAttackPressed = false;
        }
    }

    void Attack()
    {
        audioSource.PlayOneShot(swingSound);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Boss") 
            {
                enemy.GetComponent<BossHpController>().TakeDamage(attackDamage);
            }
            else if(enemy.tag == "Enemy")
            {
                enemy.GetComponent<EnemyHPController>().TakeDamage(attackDamage);
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {

        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void playAttack()
    {
        Invoke("Attack", .2f);
        pukkiAnimator.SetTrigger("MeleeAttack");

        if (isAttackPressed)
        {
            Invoke("AutoAttack", .2f);
        }
    }
    private void AutoAttack()
    {
        if (isAttackPressed)
        {
            Invoke("playAttack", .2f);
        }
    }
}