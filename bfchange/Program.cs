using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AddText
{
    internal class fbchange
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ファイルパスを入力してください。");
            string filepath = Console.ReadLine();

            var config = ConfigurationManager.AppSettings;

            // 入力されていないなら処理をしない
            if (filepath == null || filepath == "")
            {
                Console.WriteLine("ファイルパスが入力されていません。");
                return;
            }

            // ファイルの存在チェック
            if (!File.Exists(filepath))
            {
                Console.WriteLine("ファイルが存在しません。");
                return;
            }
            try
            {
                // ファイルの読み込み
                List<string> list = new List<string>();
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() != -1)
                    {
                        list.Add(sr.ReadLine());
                    }
                }

                // ファイル書き込み
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    char last;
                    if (string.IsNullOrEmpty(config.Get("lastChar")))
                    {
                        // 未設定の場合は空白を設定
                        last = ' ';
                    }
                    else
                    {
                        // 設定された文字を
                        last = char.Parse(config.Get("lastChar"));
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i].TrimEnd(last).Split(config.Get("splitChar"));
                        if(item.Length > 2)
                        {
                            Console.WriteLine("分割結果が2より多いです。");
                            continue;
                        }
                        sw.WriteLine(item[1] + config.Get("splitChar") + item[0] + config.Get("lastChar"));
                    }
                }
                Console.WriteLine("正常終了しました。");
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラーが出ました。" + ex.Message);
            }
        }
    }
}
