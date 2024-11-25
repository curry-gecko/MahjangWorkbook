using UnityEngine;

namespace CGC.App
{
    /// <summary>
    /// テキストを対象の中央に反映させる。
    /// </summary>
    public class TextOnGameObject : MonoBehaviour
    {
        public string textToDisplay = "Hello, World!";
        public Color textColor = Color.black;
        public float textSize = 0.2f;

        private void Start()
        {
            // TextMeshを追加
            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(transform); // このGameObjectに紐付ける
            textObject.transform.localPosition = Vector3.zero; // ローカル位置を設定

            // TextMeshコンポーネントを追加
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = textToDisplay;
            textMesh.characterSize = textSize;
            textMesh.color = textColor;

            // フォントやアライメントを設定（任意）
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.fontSize = 48; // 解像度による文字サイズ
        }
    }
}