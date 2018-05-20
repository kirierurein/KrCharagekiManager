# KrCharagekiManagerはADV機能の基本的な機能(キャラ表示、メッセージ表示等)をサポートします

## 使い方
KrCharagekiManagerはADVの処理を記述するためのTextScriptとキャラクターのセリフを管理するためのcsvの2つのデータで再生することができます
画面でKrCharagekiManagerのCreate関数を呼ぶことでADV機能のマネージャーを生成することができます
e.g.
KrCharagekiManager.Create(["parent"], ["script paths"], ["character parent"], ["character  view  mode"], ["interval in auto mode"], ["asset base path"], ["server base path"]);

ボタンが押されたときにKrCharagekiManagerのTapScreen関数を呼ぶことで次の処理に送ることができます

KrCharagekiManagerからKrCharagekiUIControllerを取得してボタンが押された場合などのコールバックを設定します
- RegisterFade
- RegisterTitle
- RegisterBackground
- RegisterTextArea
- ToggleAutoMode
- ResetAutoMode

## 注意点
- 音声データは外部からの読み込みも想定しているため、wav形式のみとなってます
- シナリオデータはcsv形式のみとなってます
- シナリオのデータはcsv形式のため、カンマ(,)を入力しないでください

## TextScriptのリファレンス
TextScriptの記述はconst, resources, initialize, mainの4つのグループに分かれています

### const
constは定数を定義するグループです
e.g.
const:
    hoge="hoge"	 // 変数hogeに文字列"hoge"を設定
    $hoge        // 定数を使用するときは頭に$をつけます
end

### resources
resourcesはADVを再生する時に必要な外部のリソースを宣言するグループです
※ 外部のリソースのダウンロード処理は自分で実装する必要があります
e.g.
resources:
    "test/hoge.csv"  // BaseパスはADVを再生する際に設定するためBaseパス以下のパスを設定します
end

### initialize
initializeはADVを再生するための初期化処理を行うためのグループです
e.g.
initialize:
    load_scenario key="csv" path="test/hoge.csv" // csvデータをロードします
end

#### initialize command
- シナリオの読み込み
load_scenario key=["(string)key to register this scenario"] path=["(string)csv path"]
- 背景の読み込み
load_bg id=["(uint)background id"]
- キャラクターの読み込み
load_chara id=["(uint)character id"]

### main
mainはADVを実際に再生する際の処理を行うためのグループです
1つ1つの処理がsectionという単位に分かれています
e.g.
main:
    section:
        set_scenario key="csv" // csvデータを設定します
    end
end

#### main command (within section)
- テキストの設定
set_text id=["(uint)text id of scenario"]
- テキストの表示
show_text
- シナリオの設定
set_scenario key=["(string)key of the registered scenario"]
- 背景の設定
set_bg id=["(uint)background id"]
- 背景を表示
show_bg
- 背景を非表示
hide_bg
- 背景に紐付いた名前の設定
set_title
- タイトルを表示
show_title
- タイトルを非表示
hide_title
- フェードアウト
fade_out
- フェードイン
fade_in
- 入力待ち設定の切り替え
wait_input wait=["(boolean)wait for input"]
- 待機時間の設定
wait_time time=["(float)wait time"]
- キャラクターを表示
show_chara id=["(uint)character id"]
- キャラクターを非表示
hide_chara id=["(uint)character id"]
- キャラクターのモーション、ポーズを設定
chara_action chara=["(uint)character id"] action=["(uint)action id"]
- キャラクターの座標の設定
chara_position chara=["(uint)character id"] position=["(string)position name"]
- SEの再生
play_se path=["(string)se path"]
- BGMの再生
play_bgm path=["(string)bgm path"]

