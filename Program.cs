using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DBX2_MsgCreator
{
    class Program
    {
        const string DIR_EN_MSG = "PcEnMsg";
        const string DIR_JA_MSG = "CsJaMsg";
        const string DIR_OUTPUT = "output\\data\\msg";
        const string DIR_TEMP = "output\\tmp";
        const string MSG_TOOL = "Dragon_Ball_Xenoverse_2_MSG_Tool.exe";
        const string ERROR = "error.log";
        static void Main(string[] args)
        {
            // フォルダ生成
            Directory.CreateDirectory(DIR_EN_MSG);
            Directory.CreateDirectory(DIR_JA_MSG);
            Directory.CreateDirectory(DIR_OUTPUT);

            // msgTool存在チェック
            if (!File.Exists(MSG_TOOL)) {
                Console.WriteLine($"{MSG_TOOL} is not found.");
                Console.WriteLine();
                Console.WriteLine("-- please push any key --");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // tempフォルダを再生成
            if (Directory.Exists(DIR_TEMP))
            {
                Directory.Delete(DIR_TEMP, true);
                // 実行完了待ち
                while (Directory.Exists(DIR_TEMP))
                {
                    Thread.Sleep(1);
                }
            }
            Directory.CreateDirectory(DIR_TEMP);

            // errorログ削除
            if (File.Exists(ERROR)) {
                File.Delete(ERROR);
            }

            // msgTool設定
            var msgTool = new ProcessStartInfo();
            msgTool.FileName = MSG_TOOL;
            msgTool.CreateNoWindow = true;
            msgTool.UseShellExecute = false;

            var jaFiles = Directory.GetFiles(DIR_JA_MSG, "*_ja.msg");
            var currents = 1;
            var errors = 0;

            foreach (string jaPath in jaFiles) {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{currents++}/{jaFiles.Length}");

                // msgTool実行(Export)
                var jaMsg = Path.GetFileName(jaPath);
                var txtPath = Path.Join(DIR_TEMP, Path.ChangeExtension(jaMsg, "txt"));
                msgTool.Arguments = String.Join(" ", "-e", jaPath, txtPath);
                Process.Start(msgTool).WaitForExit();

                // メッセージからNullを除外
                var data = "";
                using (var sr = new StreamReader(txtPath))
                {
                    data = sr.ReadToEnd();
                }
                using (var sw = new StreamWriter(txtPath))
                {
                    sw.Write(data.Replace("\0", ""));
                }

                // 英語メッセージ存在チェック
                var enMsg = jaMsg.Replace("_ja", "_en");
                var enPath = Path.Join(DIR_EN_MSG, enMsg);
                if (!File.Exists(enPath))
                {
                    continue;
                }

                // msgTool実行(Import)
                msgTool.Arguments = String.Join(" ", "-i", enPath, txtPath);
                Process.Start(msgTool).WaitForExit();

                // 出力ファイルを移動
                var newPath = enPath + ".NEW";
                var resultPath = Path.Join(DIR_OUTPUT, enMsg);
                if (File.Exists(newPath))
                {
                    File.Move(newPath, resultPath, true);
                }
                else
                {
                    errors++;
                    // エラーログ出力
                    using (var sw = new StreamWriter(ERROR, true))
                    {
                        sw.WriteLine(enMsg);
                    }
                }
            }

            // tempフォルダを削除
            Directory.Delete(DIR_TEMP, true);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"{jaFiles.Length - errors} files created.");
            if(errors > 0)
            {
                Console.WriteLine($"{errors} files failed to create.");
            }
            Console.WriteLine();
            Console.WriteLine("-- please push any key --");
            Console.ReadKey();
        }
    }
}
