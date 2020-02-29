# DBX2_MsgCreator

Dragon Ball Xenoverse 2 MSG Tool By Delutto を使ってメッセージデータを作成するための補助ツール

実行には[.Net Core Runtime 3.1](https://www.ipentec.com/document/windows-install-dotnet-core-31-runtime)が必要

メッセージデータは各自自力で吸い出す

日本語の表示は別途フォントデータが必要(CS版のものをリネームするだけでOK)

# 必要なもの

* [Dragon Ball Xenoverse 2 MSG Tool By Delutto](https://zenhax.com/viewtopic.php?t=4052#p35491)
* PC版の英語メッセージデータ(*_en.msg)
* CS版の日本語メッセージデータ(*_ja.msg)

# 作業手順

1. Dragon Ball Xenoverse 2 MSG Tool By Delutto をダウンロードして実行ファイルを同ディレクトリに置く
2. PC版の英語メッセージデータ(*_en.msg)を「PcEnMsgフォルダ」に追加する
3. CS版の日本語メッセージデータ(*_ja.msg)を「CsJaMsgフォルダ」に追加する
4. DBX2_MsgCreator.exe を実行
5. 変換後のメッセージデータが「outputフォルダ」に出力される

変換に失敗したメッセージデータの一覧は「error.log」で確認できる

メッセージデータの反映は XV2Patcher を使うと簡単
