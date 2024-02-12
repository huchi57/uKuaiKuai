using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UrbanFox.uKuaiKuai.Editor
{
    public class KuaiKuaiWindow : EditorWindow
    {
        [Serializable]
        public struct EditorData
        {
            public Language Language;
            public string WhoToKuaiKuai;
            public string WhatToKuaiKuai;
        }

        [Serializable]
        public enum Language
        {
            English,
            [InspectorName("華語")] Mandarin,
            [InspectorName("台語")] Taigi,
            [InspectorName("日本語")] Japanese
        }

        [SerializeField]
        private EditorData m_editorData;

        private Vector2 m_scroll;
        private Texture m_kuaiKuaiGreen;
        private Vector2 m_imageSize;

        private float m_imageAspectRatio;

        private GUIStyle m_titleStyle;
        private GUIStyle m_textStyle;

        private static string EditorDataKey => $"{Application.companyName}/{Application.productName}/{typeof(KuaiKuaiWindow).Name}/{nameof(m_editorData)}";

        private GUIStyle TitleStyle
        {
            get
            {
                if (m_titleStyle == null)
                {
                    m_titleStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 20,
                        alignment = TextAnchor.MiddleCenter,
                    };
                }
                return m_titleStyle;
            }
        }

        private GUIStyle TextStyle
        {
            get
            {
                if (m_textStyle == null)
                {
                    m_textStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 15,
                        alignment = TextAnchor.MiddleCenter,
                    };
                    m_textStyle.normal.textColor = Color.red;
                    m_textStyle.hover.textColor = Color.red;
                }
                return m_textStyle;
            }
        }

        [MenuItem("OwO/Windows/uKuaiKuai...")]
        private static void OpenWindow()
        {
            var window = GetWindow<KuaiKuaiWindow>();
            window.titleContent = new GUIContent("uKuaiKuai");
            window.minSize = new Vector2(250, window.minSize.y);
            window.Show();
        }

        private void OnEnable()
        {
            m_kuaiKuaiGreen = AssetDatabase.LoadAssetAtPath<Texture>("Packages/com.urbanfox.ukuaikuai/Images/T_KuaiKuaiGreen.png");
            m_imageAspectRatio = m_kuaiKuaiGreen.width / m_kuaiKuaiGreen.height;
            m_imageSize = new Vector2(m_kuaiKuaiGreen.width, m_kuaiKuaiGreen.height);

            m_editorData = EditorPrefs.HasKey(EditorDataKey) ? JsonUtility.FromJson<EditorData>(EditorPrefs.GetString(EditorDataKey)) : new EditorData()
            {
                WhoToKuaiKuai = "Unity 大神",
                WhatToKuaiKuai = "開發不出包"
            };
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(EditorDataKey, JsonUtility.ToJson(m_editorData));
        }

        private void OnGUI()
        {
            m_scroll = GUILayout.BeginScrollView(m_scroll);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            m_editorData.Language = (Language)EditorGUILayout.EnumPopup(m_editorData.Language, GUILayout.Width(120));
            GUILayout.EndHorizontal();
            m_editorData.WhoToKuaiKuai = EditorGUILayout.TextField(GetLocalization("Who to Kuai Kuai"), m_editorData.WhoToKuaiKuai);
            m_editorData.WhatToKuaiKuai = EditorGUILayout.TextField(GetLocalization("What to Kuai Kuai"), m_editorData.WhatToKuaiKuai);

            EditorGUILayout.Space(10);

            GUILayout.Label(string.IsNullOrEmpty(m_editorData.WhoToKuaiKuai) ? "" : $"{m_editorData.WhoToKuaiKuai}:", TitleStyle);

            float imageWidth = Mathf.Min(EditorGUIUtility.currentViewWidth - 20, m_imageSize.x);
            float imageHeight = imageWidth / m_imageAspectRatio;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(m_kuaiKuaiGreen, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
            var imageRect = GUILayoutUtility.GetLastRect();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            imageRect.y += Mathf.Min(400, EditorGUIUtility.currentViewWidth) * 0.19f;
            GUI.Label(imageRect, m_editorData.WhatToKuaiKuai, TextStyle);

            EditorGUILayout.HelpBox("本工具僅作為娛樂使用。圖片「乖乖玉米脆條-奶油椰子（綠色包裝）」源自乖乖股份有限公司官方網站，版權屬該公司所有。", MessageType.Info);

            GUILayout.EndScrollView();
        }

        private string GetLocalization(string key)
        {
            if (m_dictionary.ContainsKey(key))
            {
                return m_dictionary[key][(int)m_editorData.Language];
            }
            return key;
        }

        private readonly Dictionary<string, string[]> m_dictionary = new Dictionary<string, string[]>()
        {
            {
                "Who to Kuai Kuai", new string[]
                {
                    "Who to Kuai Kuai",
                    "誰要乖乖",
                    "誰欲乖乖",
                    "誰が乖乖"
                }
            },
            {
                "What to Kuai Kuai", new string[]
                {
                    "What to Kuai Kuai",
                    "怎麼乖乖",
                    "安怎乖乖",
                    "どうやって乖乖"
                }
            }
        };
    }
}
