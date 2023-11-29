using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private List<UIButton> buttonGroup;
    [SerializeField] private AudioSource audioOver = default;
    [SerializeField] private AudioSource audioClick = default;
    private Animator animator = default;

    [SerializeField] private UnityEvent OnClickEvent;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
        if (audioOver != null)
        {
            audioOver.Play();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool("Pressed", true);
        if (audioClick != null)
        {
            audioClick.Play();
        }
        animator.SetBool("Hover", false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonSelect();
        animator.SetBool("Selected", true);
        animator.SetBool("Pressed", false);
    }

    public void OnButtonSelect()
    {
        foreach (UIButton button in buttonGroup)
        {
            button.OnButtonDeselect();
        }
        OnClickEvent.Invoke();
    }
    public void OnButtonDeselect()
    {
        animator.SetBool("Selected", false);
    }
    public void OnButtonDisable()
    {
        animator.SetBool("Disabled", true);
    }
    public void OnButtonEnable()
    {
        animator.SetBool("Disabled", false);
    }
}
