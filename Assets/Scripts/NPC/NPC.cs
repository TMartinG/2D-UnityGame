using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string[] dialogueLines;
    [SerializeField] Dialoge dialogueManager;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Appear");
            dialogueManager.StartDialogue(dialogueLines);
        }
    }
}
