using XSSocket.Models;

namespace SCNotify
{
    using System.Net.WebSockets;
    using XSSocket;

    /// <summary>
    /// XSOverlay に通知を行うクラス
    /// </summary>
    internal class Notification
    {
        private XSSocket connector;

        public Notification()
        {                           // コンストラクター
            connector = new XSSocket(SCUtils.AppName);     // XSOverlay クラスインスタンス生成
            Task.Run(Connection);   // XSOverlayに別スレッドで接続
        }

        public void Connection()
        {
            connector.ConnectAsync();

            while (connector.State == WebSocketState.Connecting)
            {                       // 接続中
                OutputCS(content: "接続中...");
                Task.Delay(4000).Wait();   // 4秒待機
            }
            if (connector.State == WebSocketState.Open)
            {                       // 接続済
                OutputCS(content: "接続しました!");
            }
            else
            {
                OutputCS(content: $"接続失敗 : {connector.State}");
            }

            OutputCS(content: "テスト通知を送信");
            Send(content: "テスト通知\n");
        }

        /// <summary>
        /// CSに出力する
        /// </summary>
        /// <param name="title">[タイトル]</param>
        /// <param name="content">内容</param>
        public void OutputCS(string title = "Notify", string content = "")
        {
            Console.WriteLine($"[{title}] {content}");
        }

        /// <summary>
        /// XSOverlay に通知を送信 + CSに出力
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="content">内容</param>
        /// <param name="timeout">表示時間</param>

        public void Send(string title = "SCN", string content = "", float timeout = 5f)
        {
            XSNotificationObject notificationObject = new XSNotificationObject()
            {
                title = title,                  // タイトル
                content = content,              // 内容
                height = 125f,
                timeout = timeout,              // 表示時間
                sourceApp = SCUtils.AppName     // デバッグ用アプリ名
            };

            Task.Run(() => connector.SendNotification(notificationObject));     // 通知を送信
            OutputCS(title, content);           // CSに出力
        }
    }
}
