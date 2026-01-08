using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    private bool isInit = false;

    public virtual void Show()
    {
        Init();

        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Init()
    {
        if (isInit)
        {
            return;
        }
        OnInit();
    }

    public abstract void OnInit();
}
