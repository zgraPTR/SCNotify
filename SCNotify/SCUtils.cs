namespace SCNotify
{
    /// <summary>
    /// 正規表現などの定数を書くクラス
    /// </summary>
    internal class SCUtils
    {
        public static readonly string AppName = "SCN";      // アプリ名
        public static readonly string LogFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", "VRChat", "VRChat");
        // VRCのログのフォルダパス

        public static readonly int BaseFuelNum = 4;         // 基本ガソリン数
        public static readonly string GameStartStr = "SLASHCO Game setup.";                 // ゲーム開始時のログテキスト
        public static readonly string GameEndStr = "Generators reset again.";               // ゲーム終了時のログテキスト
        public static readonly string slasherMatch = @"Initialized the Slasher as (\w+)";   // スラッシャー名の正規表現
        public static readonly string playerCntMatch = @" SLASHCO Fuel Init. For a game of (\d+) players";      // プレイヤー数の正規表現
    }
}
