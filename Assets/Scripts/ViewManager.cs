using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

public abstract class View : MonoBehaviour
{
    public UnityEvent OnShow;
    public UnityEvent OnHide;


    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow?.Invoke();
    }

    public virtual void Hide()
    {
        OnHide?.Invoke();
        gameObject.SetActive(false);
    }
}


public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    public List<View> Views;

    private Dictionary<Type, View> _views;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _views = new Dictionary<Type, View>();
        foreach (var view in Views)
        {
            _views[view.GetType()] = view;
        }
    }

    public void Show<T>(bool isHidingAll = true) where T : View
    {
        if (isHidingAll) HideAll();
        _views[typeof(T)].Show();
    }

    public void Hide<T>() where T : View
    {
        _views[typeof(T)].Hide();
    }

    public void HideAll()
    {
        foreach (var view in Views)
        {
            view.Hide();
        }
    }

    public void ShowPreparationView() => Show<PreparationView>();

    public void ShowTutorialView() => Show<TutorialView>();

}
