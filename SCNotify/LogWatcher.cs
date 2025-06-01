namespace SCNotify
{
    /// <summary>
    /// ファイルのイベントを取得を取得して監視を行うクラス
    /// </summary>
    internal class LogWatcher
    {
        private string logPath;                 // 読み込み中のログ名
        private long txtPosition;               // 読み込み済の内容の位置
        
        private FileSystemWatcher watcher;      // ファイルの監視を行うクラス
        private SCLog scLog;                    // スラシュコのログを管理するクラス

        public LogWatcher()
        {                       // コンストラクター

            logPath = "";       // ログのファイルパスを初期化

            watcher = new FileSystemWatcher(SCUtils.LogFolderPath);     // クラスインスタンス生成
            scLog = new SCLog();                // クラスインスタンス生成

            watcher.NotifyFilter = NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Size;                  // 通知のフィルターを設定

            watcher.Changed += OnChanged;       // ファイル生成時のイベントハンドラ
            watcher.Filter = "output_*.txt";    // ログファイルをフィルターに設定
        }

        public bool RaisingEvents
        {                       // ファイルのイベントを監視するか
            get
            {
                return watcher.EnableRaisingEvents;
            }
            set
            {                   // ログ監視イベントの有無
                watcher.EnableRaisingEvents = value;
            }
        }

        /// <summary>
        /// ファイル内容 変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;       // イベントのファイルのパス
            if (logPath != filePath)
            {                                   // 読み込んでいたファイルパスと別
                logPath = filePath;             // 新しいログのファイルパスを指定
                txtPosition = 0;                // 読み込み済の位置をリセット
            }

            ReadLog();      // ログを読み込む
        }


        /// <summary>
        /// ログのファイルの内容を一行ずつ読み込む
        /// </summary>
        private void ReadLog()
        {
            using FileStream fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            // ファイルストリームを開く
            using StreamReader sr = new StreamReader(fs);   // 内容を読み込むストリームを生成
            SeekOrigin seekOrigin = SeekOrigin.Begin;       // ファイルを読み込み始める位置
            string? line = "";                   // 1行の内容

            if (txtPosition == 0)
            {                                   // 初めてログファイルを読み込んだ
                seekOrigin = SeekOrigin.End;    // 最後から読み込むように指定
            }

            fs.Seek(txtPosition, seekOrigin);   // 指定した場所に移動

            while ((line = sr.ReadLine()) is not null)
            {                                   // 1行ずつ末尾まで内容を読み込む
                scLog.CheckLog(line);           // スラシュコのログか内容を確認する関数
            }

            txtPosition = fs.Position;          // 読み込み済の位置を保存
        }

    }
}
