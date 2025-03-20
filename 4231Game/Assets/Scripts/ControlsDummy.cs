using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsDummy : MonoBehaviour
{
    public Animator animator;
    public List<Attack> attackList;
    public int[] Attack1IDList; 
    private int[] selectedAttacks = new int[3];

    private int animID_attack;
    private int animID_attackID;
    private bool isAttacking = false;
    private int attackIndex = 0;

    private void Start()
    {
        
        animID_attack = Animator.StringToHash("Attack");
        animID_attackID = Animator.StringToHash("AttackID");
    }

    public void SetAttackOrder(int attack1, int attack2, int attack3)
    {
        selectedAttacks[0] = Attack1IDList[attack1];
        selectedAttacks[1] = Attack1IDList[attack2];
        selectedAttacks[2] = Attack1IDList[attack3];

        Debug.Log($" Attack Order Set: {selectedAttacks[0]}, {selectedAttacks[1]}, {selectedAttacks[2]}");
    }

    public void StartAttackSequence()
    {
        if (!isAttacking)
        {
            Debug.Log(" Attack sequence started!");
            attackIndex = 0;
            StartCoroutine(PlayAttacksInOrder());
        }
    }

    private IEnumerator PlayAttacksInOrder()
    {
        isAttacking = true;

        while (attackIndex < selectedAttacks.Length)
        {
            int attackID = selectedAttacks[attackIndex];

            Debug.Log($" Executing attack ID: {attackID}");

            animator.SetTrigger(animID_attack);
            animator.SetInteger(animID_attackID, attackID);

            yield return new WaitForSeconds(GetAnimationDuration(attackID));

            attackIndex++;
        }

        Debug.Log(" Attack sequence complete!");
        isAttacking = false;
    }

    private float GetAnimationDuration(int attackID)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name.Contains($"Attack{attackID}"))
            {
                return clip.length;
            }
        }

        return 1f; 
    }
}