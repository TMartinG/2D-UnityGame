using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSunny : MonoBehaviour
{
    public Animator animator;
    public FadeController fadeController;
    public GameObject end;

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        animator.SetTrigger("jump");
        yield return new WaitForSeconds(2f);
        end.SetActive(true);
    }

}
