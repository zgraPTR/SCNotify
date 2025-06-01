
namespace SCNotify
{
    internal static class Program
    {
        /// <summary>
        /// メイン関数、起動時に実行
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            Console.Title = "SCNotify";                     // CSのタイトル指定
            Console.WriteLine("Ctrl+C キーを入力でアプリを終了します");

            LogWatcher logWatcher = new LogWatcher();       // ログの監視クラスのインスタンス生成
            logWatcher.RaisingEvents = true;                // ログの開始監視

            using ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim();
            Console.CancelKeyPress += (sender, e) =>        // Ctrl+C キーの入力イベント
            {
                logWatcher.RaisingEvents = false;           // ログの監視を停止
                e.Cancel = true;                            // 終了しない
                manualResetEventSlim.Set();                 // タスク用
            };

            manualResetEventSlim.Wait();                    // Ctrl+C キーの入力待ち
        }

    }
}