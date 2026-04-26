using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialoge : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialogueBox;
    public float typingSpeed = 0.05f;

    private Queue<string> sentences = new Queue<string>();
    private Coroutine typingCoroutine;
    [SerializeField] Animator animator;
    public bool isEnded = false;
    public GameObject npc;

    public void StartDialogue(string[] lines)
    {
        dialogueBox.SetActive(true);
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DisplayNextSentence();
        }
    }
    public void DisplayNextSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        animator.SetTrigger("Disappear");
        
        if (isEnded)
        {
            npc.GetComponent<Collider2D>().enabled = false; // NPC collider kikapcsolása
        }
    }

   
}
