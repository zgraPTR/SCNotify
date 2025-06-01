using System.Text.RegularExpressions;

namespace SCNotify
{
    /// <summary>
    /// ログの内容の取得などを行う関数
    /// </summary>
    internal class SCLog
    {
        private bool inGame;            // ゲーム内にいるか
        private bool sentGameInfo;      // 情報送信済フラグ

        private int playerNum = 0;                      // プレイヤー数
        private int needFuelNum = 0;                    // 必要ガソリン数
        private string slasherName = string.Empty;      // スラッシャー名

        Notification notification;      // XSOverlayに通知を送信するクラス

        public SCLog()
        {                               //　コンストラクター
            notification = new Notification();          // 初期化
            ResetGameData();            // ゲーム情報をリセットする
        }


        /// <summary>
        /// ゲーム情報をリセットする
        /// </summary>
        private void ResetGameData()
        {
            inGame = false;             // ゲーム中フラグ
            sentGameInfo = false;       // 情報送信済フラグ
            playerNum = 0;              // プレイヤー数
            needFuelNum = 0;            // 必要ガソリン数
            slasherName = string.Empty; // スラッシャー名
        }

        /// <summary>
        /// ログの内容からゲーム情報を取得する
        /// </summary>
        /// <param name="line">ログの1行の内容</param>
        public void CheckLog(string line)
        {
            if ((inGame == false) && line.EndsWith(SCUtils.GameStartStr))
            {                           // ゲーム開始開始時
                inGame = true;          // ゲーム中フラグ
                notification.Send(content: "ゲーム開始");
            }
            else if ((inGame == true) && line.EndsWith(SCUtils.GameEndStr))
            {                           // ゲーム終了時
                ResetGameData ();       // 変数をリセットする
                notification.Send(content: "ゲーム終了\n");
            }

            if ((sentGameInfo == true) || (inGame == false))
            {                           // ゲーム情報送信済 or ゲーム外
                return;
            }

            Match playerCntMatch = Regex.Match(line, SCUtils.playerCntMatch);
            Match slasherNameMatch = Regex.Match(line, SCUtils.slasherMatch);

            if (playerCntMatch.Success)
            {                           // プレイヤー数を検知
                playerNum = int.Parse(playerCntMatch.Groups[1].Value);
                needFuelNum = Math.Min((SCUtils.BaseFuelNum + (playerNum - 1)), 8);     // ガソリン数の計算
                // 必要ガソリン数 = (4 + (プレイヤーの人数 - 1)  (上限8個)
            }
            else if (slasherNameMatch.Success)
            {                           // スラッシャー名を検知
                slasherName = slasherNameMatch.Groups[1].Value;
            }

            if ((playerNum != 0) && (slasherName != string.Empty))
            {                           // ゲーム情報の取得完了
                notification.Send(slasherName, $"Player: {playerNum}人    ガソリン: {needFuelNum}個", 10f);      // CSに出力
                sentGameInfo = true;    // 通知送信済フラグ
            }
        }
    }
}
