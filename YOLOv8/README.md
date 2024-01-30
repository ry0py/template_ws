# YOLOv8を用いて矩形で物体検出する方法

## 私がじゃんけんやった時のメモ
[じゃんけんできたぞ](https://shelled-emmental-f27.notion.site/4aada340f2a945cf8a3062164dbc1cad?pvs=4)

## 誰でもできる簡単な手順
1. ultralyticsをpipでインストール
2. 自分でアノテーションしたものを使用したい場合は./data/sample.yamlを参考にするtest,train,valにoriginデータとアノテーションしたものを入れる。
3. アノテーションは個人的にfastlabelを使ってる。アカウントを作成しないといけないがチームでアノテーションのデータを管理できるのと応用が利くのとwebで動くからOS依存にならない。あとはyoloデータとしてエクスポートできる
4. dataにファイルをインポートしたらtrain.pyのmodelのインスタンスの中身を```yolov8n.pt```として作成する。もし別のyoloでの学習した.ptがあるならそれに差し替える
5. model.trainの引数をdataフォルダの中のsample.yamlを指定する。epoch数とかはじぶんで調べて
6. これを実行すると学習が始まる。特殊な設定をしてないのでCPUのみで学習が始まる。GPUでやるやり方は知らない。
7. 学習が完了するとrunsフォルダが作成されるのでdetect.pyと同じ感じでmodelのインスタンスを作成するときにYOLOの引数に```./runs/detect/train/weights/best.pt```を与える
8. ちなみにresultsがmodelのメソッドを使わずに直接インスタンスの中に()で与えられるのはpythonのデコレーターの@classmethodがdetectメソッドについてたからな気がする
9. 引数は自分で調べて、信頼性とかも設定できるしほかにも　いろいろある