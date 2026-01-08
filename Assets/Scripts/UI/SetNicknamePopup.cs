
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetNicknamePopup : BasePopup
{
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private Button m_Btn;

    private ChatManager m_chatMgr;
    private ServerSystem m_ServerSys;

    public void OnEnter()
    {
        string nickname = m_InputField.text;
        m_chatMgr.SetNickname(nickname);
        m_chatMgr.ReqSetNickname(m_ServerSys.CurSocket, nickname);
    }

    public override void OnPopupInit()
    {
        m_Btn.onClick.AddListener(OnEnter);

        m_chatMgr = ChatManager.Instance;
        m_ServerSys = ServerSystem.Instance;
    }
}
