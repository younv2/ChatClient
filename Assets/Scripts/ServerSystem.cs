
using System.Net.Sockets;

public class ServerSystem : MonoSingleton<ServerSystem>
{
    public Socket CurSocket { get; private set; }
    public void ConnectServer()
    {
        var client = new TcpClient("127.0.0.1", 9000);
        NetworkStream stream = client.GetStream();
        CurSocket = client.Client;
    }
    void Start()
    {
        ConnectServer();
        ChatManager chatManager = ChatManager.Instance;

        UIManager.Instance.Show<SetNicknamePopup>("SetNicknamePopup");
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