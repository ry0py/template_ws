import cv2 
import mediapipe as mp
import json
import socket


def send_message_to_server(message):
    # クライアントの設定
    host = '127.0.0.1'  # サーバーのIPアドレスこれはlocalhostと同じでlanを経由しない
    port = 12345         # ポート番号

    # messageをjsonに変換
    message_string = json.dumps(message)
    # ソケットオブジェクトの作成
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # サーバーへの接続
    client_socket.connect((host, port))

    # メッセージの送信
    client_socket.sendall(message_string.encode("utf-8"))

    # サーバーからの応答の受信
    response = client_socket.recv(1024)
    print(f"サーバーからの応答: {response.decode()}")

    # ソケットのクローズ
    client_socket.close()
# mediapipeのNormalizedLandmarkListはjosnに変換できないので、変換できるようにする
def convert_to_dict(normalized_landmark_list):
    # NormalizedLandmarkList オブジェクトを辞書に変換する
    landmarks = [] # 空のdictを作成 {}はリスト、[]は辞書
    for results in normalized_landmark_list:
        landmarks.append({
            'x': results.x,
            'y': results.y,
            'z': results.z,
            'visibility': results.visibility
        })
    # unityのJsonUtility.FromJsonを使うためには、keyが必要なので、keyを追加する
    landmarks = {
        'items': landmarks
    }
    return landmarks

if __name__ == '__main__':
    mp_hands = mp.solutions.hands
    hands = mp_hands.Hands(
        max_num_hands=2,                # 最大検出数
        min_detection_confidence=0.7,   # 検出信頼度
        min_tracking_confidence=0.7     # 追跡信頼度
    )
    cap = cv2.VideoCapture(0)   # カメラのID指定
    if cap.isOpened():
        while True:
            # カメラから画像取得
            success, img = cap.read()
            if not success:
                continue
            img = cv2.flip(img, 1)          # 画像を左右反転
            img_h, img_w, _ = img.shape     # サイズ取得
            results = hands.process(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
            # for hand_landmarks in results.multi_hand_landmarks: # 検出したての数
            #     hand_lamdmarks_dict = convert_to_dict(hand_landmarks) #一つと仮定する
            if results.multi_hand_landmarks:
                hand_landmarks_dict = convert_to_dict(results.multi_hand_landmarks[0].landmark) #一つと仮定する
                send_message_to_server(hand_landmarks_dict)
                # for i in range(len(hand_landmarks_dict)):
                #     hand_landmarks_dict[i]["x"] = hand_landmarks_dict[i]["x"] * img_w
                #     hand_landmarks_dict[i]["y"] = hand_landmarks_dict[i]["y"] * img_h
                #     send_message_to_server(hand_landmarks_dict[i])
