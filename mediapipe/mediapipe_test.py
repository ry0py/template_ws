import cv2
import mediapipe as mp

def convert_to_dict(normalized_landmark_list):
    # NormalizedLandmarkList オブジェクトを辞書に変換する
    landmarks = [] # 空のdictを作成 {}はリスト、[]は辞書
    for landmark in normalized_landmark_list:
        landmarks.append({
            'x': landmark.x,
            'y': landmark.y,
            'z': landmark.z,
            'visibility': landmark.visibility
        })
    return landmarks

mp_hands = mp.solutions.hands
hands = mp_hands.Hands(
    max_num_hands=2,                # 最大検出数
    min_detection_confidence=0.7,   # 検出信頼度
    min_tracking_confidence=0.7     # 追跡信頼度
)
cap = cv2.VideoCapture(0)   # カメラのID指定
success, img = cap.read()
img = cv2.flip(img, 1)          # 画像を左右反転
img_h, img_w, _ = img.shape     # サイズ取得
results = hands.process(cv2.cvtColor(img, cv2.COLOR_BGR2RGB))
# for hand_landmarks in results.multi_hand_landmarks:
#     print(hand_landmarks.landmark)
#     print("-----------------")
# print(results.multi_hand_landmarks[0].landmark)
print(convert_to_dict(results.multi_hand_landmarks[0].landmark))
# print(results.multi_hand_landmarks.landmark[0])