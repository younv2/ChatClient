
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePopup : BaseUI
{
    [SerializeField] private Button m_CloseBtn;

    private void Awake()
    {
        
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnInit()
    {
        if (m_CloseBtn != null)
        {
            m_CloseBtn.onClick.RemoveAllListeners();
            m_CloseBtn.onClick.AddListener(OnClose);
        }
        else
            Debug.LogWarning($"[로그]{gameObject.name} - CloseBtn 할당 필요");

        OnPopupInit();
    }

    public abstract void OnPopupInit();
}
