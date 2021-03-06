using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{
    [Header("Melee Enemy Settings")]
    public Loot[] loots;
    public int HP = 100;
    public Animator animator;
    public bool isInBossIsland;
    public ParticleSystem particle;

    [SerializeField] Transform[] wayPoints;

    private int enemyHP;
    private bool wasInBossIsland = false;

    private void Start()
    {
        enemyHP = HP;

        if (particle)
        {
            if (isInBossIsland)
            {
                particle.Play();
                wasInBossIsland = true;
            }
            else
            {
                particle.Stop();
            }
        }

    }

    private void Update()
    {
        if (isInBossIsland)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().CheckPlayerHasEverySkulls())
            {
                particle.Stop();
                isInBossIsland = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("melee enemy collision: " + collision.collider.name);
    }

    public Transform[] GetWayPoints()
    {
        return wayPoints;
    }

    public void MustKillEnemyForCheatCode()
    {
        isInBossIsland = false;
        StartCoroutine(DelayedDead(animator.GetCurrentAnimatorStateInfo(0).length));
        GameObject.FindGameObjectWithTag("TreasureBox").GetComponent<TreasureBox>().KillZombieEnemy();
    }

    //Add this function to player's slingshot
    public void TakeDamage(int damageAmount)
    {
        enemyHP -= damageAmount;
        
        if(enemyHP <= 0)
        {
            //Play Death Animation

            animator.SetTrigger("Death");
            
            if(isInBossIsland == true)
            {
                if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().CheckPlayerHasEverySkull())
                {
                    //die
                    StartCoroutine(DelayedDead(animator.GetCurrentAnimatorStateInfo(0).length));
                    isInBossIsland = false;
                    GameObject.FindGameObjectWithTag("TreasureBox").GetComponent<TreasureBox>().KillZombieEnemy();

                    return;
                }

                StartCoroutine(DelayedAnimation(animator.GetCurrentAnimatorStateInfo(0).length));
                animator.SetTrigger("Rebirth");
                enemyHP = HP;
            }
            else
            {
                //die
                if(wasInBossIsland)
                {
                    GameObject.FindGameObjectWithTag("TreasureBox").GetComponent<TreasureBox>().KillZombieEnemy();
                }
                StartCoroutine(DelayedDead(animator.GetCurrentAnimatorStateInfo(0).length));
            }
        }
        else
        {
            //Play Damage Animation
            animator.SetTrigger("Damage");
        }
    }

    IEnumerator DelayedAnimation(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
    }

    IEnumerator DelayedDead(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        Destroy(particle);
        DropItem();
    }

    public void DropItem()
    {
        bool dropSuccess = false;

        foreach (Loot loot in loots)
        {
            float spawnPercentage = Random.Range(-0.01f, 100f);

            if (spawnPercentage <= loot.dropRate)
            {
                dropSuccess = true;
                Instantiate(loot.item, transform.position, Quaternion.identity);
                break;
            }
        }

        if(dropSuccess == false)
        {
            Instantiate(loots[0].item, transform.position, Quaternion.identity);
        }

    }
}
