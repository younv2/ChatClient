using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Dictionary<string,BasePopup> m_Popups;

    protected override void Awake()
    {
        base.Awake();
        m_Popups = new();
        var popups = GetComponentsInChildren<BasePopup>(true);
        foreach (var popup in popups)
        {
            if (m_Popups.ContainsKey(popup.gameObject.name))
                continue;
            m_Popups.Add(popup.gameObject.name, popup);
        }
    }

    public bool Show<T>(string _key) where T : BasePopup
    {
        if(!m_Popups.TryGetValue(_key,out var basePopup))
            return false;

        T popup = basePopup as T;

        if (popup == null)
            return false;

        popup.Show();
        return true;
    }
}
