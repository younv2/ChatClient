using Network;
using System;
using System.Net.Sockets;

public class ChatManager : Singleton<ChatManager>
{
    private ChatData m_ChatData;

    public void ReqSetNickname(Socket _socket, string _nickname)
    {
        ReqChatDataPacket packet = new ReqChatDataPacket();

        packet.Header.Size = ByteConverter.GetMarshalTypeSize(packet);
        packet.Header.Protocol = ChatProtocol.SetNickname;
        packet.ChatData.Nickname = _nickname;
        packet.TimeStamp = DateTime.Now.Ticks;

        var bytes = ByteConverter.StructureToBytes(packet);
        NetworkUtils.Send(_socket, bytes);
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
        NetworkUtils.Send(_socket, bytes);
    }

    public void ACKChatMessage(Socket _socket)
    {
        if (NetworkUtils.TryPeekHeader(_socket, out PacketHeader header) == false)
        {
            return;
        }
        if (header.Protocol != ChatProtocol.Message)
        {
            return;
        }
        ACKChatMessageDataPacket _packet = NetworkUtils.ReadPacket<ACKChatMessageDataPacket>(_socket);

        Console.WriteLine($"{_packet.ChatData.Nickname} : {_packet.ChatData.Msg}");
    }

}
