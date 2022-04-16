using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public Animator Animator;
    public bool Selected;
    public bool HasCursors;
    public GameObject Cursors;

    private void OnEnable()
    {
        Animator = GetComponent<Animator>();

        if (Selected)
            SelectItem();
    }


    public void SelectItem()
    {
        Animator.SetBool("Selected", true);
        if(HasCursors)
        {
            Cursors.SetActive(true);
        }
    }

    public void DeselectItem()
    {
        Animator.SetBool("Selected", false);
        if (HasCursors)
        {
            Cursors.SetActive(false);
        }
    }

}
