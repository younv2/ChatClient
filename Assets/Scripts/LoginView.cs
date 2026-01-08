using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Network;
using UnityEngine.UI;
using TMPro;

public class LoginView : MonoBehaviour
{
    private Socket m_sock;
    
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private Button m_Btn;
    public void ConnectServer()
    {
        var client = new TcpClient("127.0.0.1", 9000);
        NetworkStream stream = client.GetStream();
        m_sock = client.Client;
    }
    void Start()
    {
        ConnectServer();
        ChatManager chatManager = new ChatManager();
        m_Btn.onClick.AddListener(() =>
        {

            string nickname = m_InputField.text;
            chatManager.SetNickname(nickname);
            chatManager.ReqSetNickname(m_sock, nickname);
        });
        /*
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                chatManager.ACKChatMessage(m_sock);
            }
        });
        thread.Start();
        while (true)
        {
            Console.Write("메시지를 입력하세요: ");
            string message = Console.ReadLine() ?? "null";
            chatManager.ReqSetMessage(m_sock, message);
        }*/
    }
}
public class ChatManager
{
    private ChatData m_ChatData;

    public ChatManager()
    {
        m_ChatData = new ChatData();
    }
    public void ReqSetNickname(Socket _socket, string _nickname)
    {
        ReqChatDataPacket packet = new ReqChatDataPacket();

        packet.Header.Size = ByteConverter.GetMarshalTypeSize(packet);
        packet.Header.Protocol = ChatProtocol.SetNickname;
        packet.ChatData.Nickname = _nickname;
        packet.TimeStamp = DateTime.Now.Ticks;

        var bytes = ByteConverter.StructureToBytes(packet);
        NetworkSystem.Send(_socket, bytes);
    }
    public void SetNickname(string _nickname)
    {
        m_ChatData.Nickname = _nickname;
    }
    public void ReqSetMessage(Socket _socket, string _message)
    {
        ReqChatDataPacket packet = new ReqChatDataPacket();

        packet.Header.Size = ByteConverter.GetMarshalTypeSize(packet);
        packet.Header.Protocol = ChatProtocol.Message;
        packet.ChatData.Nickname = m_ChatData.Nickname;
        packet.ChatData.Msg = _message;
        packet.TimeStamp = DateTime.Now.Ticks;

        var bytes = ByteConverter.StructureToBytes(packet);
        NetworkSystem.Send(_socket, bytes);
    }

    public void ACKChatMessage(Socket _socket)
    {
        if (NetworkSystem.TryPeekHeader(_socket, out PacketHeader header) == false)
        {
            return;
        }
        if (header.Protocol != ChatProtocol.Message)
        {
            return;
        }
        ACKChatMessageDataPacket _packet = NetworkSystem.ReadPacket<ACKChatMessageDataPacket>(_socket);

        Console.WriteLine($"{_packet.ChatData.Nickname} : {_packet.ChatData.Msg}");
    }

}
