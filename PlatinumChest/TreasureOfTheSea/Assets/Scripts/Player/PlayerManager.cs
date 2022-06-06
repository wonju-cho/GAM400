using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//death, combat 

public class PlayerManager : MonoBehaviour
{
    public Animator animator;
    private CharacterController controller;
    private PlayerController playerController;

    public Image HealthBar;

    private bool isPlayerDead = false;
    private bool triggerOnce = false;
    private float fillSpeed = 2f;

    Vector3 playerSpawnPosition;

    [SerializeField]
    float playerHP = 10;
    private float currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = playerHP;
        playerSpawnPosition = transform.position;

        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();

        if (!controller)
            Debug.Log("There is no controller in the PlayerManager script");

        if (!HealthBar)
            Debug.Log("There is no healthbar in the playermanger script");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1) && isPlayerDead)
        {
            PlayerRespawn();
        }

        //test health bar ui
        if(Input.GetMouseButtonDown(1))
        {
            currentHP--;
        }

        if (currentHP <= 0)
        {
            isPlayerDead = true;
        }

        if (isPlayerDead)
        {
            PlayerDeath();
        }
        
        HealthBarFill();
    }

    float GetCurrentHP() { return currentHP; }

    void TakeDamge(int damage) { currentHP -= damage; }

    void HealthBarFill()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (currentHP / playerHP), fillSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Sea"))
        {
            isPlayerDead = true;
            //isPlayerDeadSea = true;
        }
    }

    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    void PlayerDeath()
    {
        playerController.enabled = false;
        animator.SetBool("IsIdle", false);

        if(isPlayerDead && triggerOnce == false)
        {
            triggerOnce = true;
            animator.ResetTrigger("Death");
            animator.SetTrigger("Death");
        }
    }

    IEnumerator WaitForSec(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void PlayerRespawn()
    {
        playerController.enabled = true;

        currentHP = playerHP;
        animator.SetBool("IsIdle", true);

        controller.enabled = false;
        controller.transform.position = playerSpawnPosition;
        controller.enabled = true;

        isPlayerDead = false;
        triggerOnce = false;

    }
}