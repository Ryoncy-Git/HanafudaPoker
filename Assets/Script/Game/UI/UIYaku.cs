using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

using HanafudaPoker.Yakus;
using HanafudaPoker.Network;

// ho6:
namespace HanafudaPoker.UIs
{
    public class UIYaku : MonoBehaviour
    {
        [Header("役を表示するText")]
        [SerializeField] private TMP_Text yakuText; // 役の名前
        [Header("役の点数を表示するText")]
        [SerializeField] private TMP_Text scoreText; // 役の点数
        [Header("その人の合計点を表示するText")]
        [SerializeField] private TMP_Text totalScoreText; // その人の合計点

        [SerializeField] private GameObject yakuPanel;

        private readonly Dictionary<Yaku, string> yakuNames = new()
        {
            { Yaku.Tsui         ,   "対" },
            { Yaku.Nitsui       ,   "二対" },
            { Yaku.Santsui      ,   "三対" },
            { Yaku.Yontsui      ,   "四対" },
            { Yaku.Mangetsu     ,   "満月" }, // 月札が4枚ある
            { Yaku.Akatan       ,   "赤タン" },
            { Yaku.Aotan        ,   "青タン" },
            { Yaku.Tan          ,   "タン" },
            { Yaku.Gokou        ,   "五光" },
            { Yaku.Yonkou       ,   "四光" },
            { Yaku.Ameshikou    ,   "雨入り四光" },
            { Yaku.Sankou       ,   "三光" },
            { Yaku.Inoshikacho  ,   "猪鹿蝶" },
            { Yaku.Sakeutage    ,   "酒宴" }, // 酒札・満月・桜に幕
            { Yaku.Mizu         ,   "水" },
            { Yaku.Murasaki     ,   "紫" },
            { Yaku.Hanaikada    ,   "花筏" },
            { Yaku.Adabana      ,   "徒花" },
            { Yaku.Chidori      ,   "千鳥" },
            { Yaku.MidareChidori,   "乱れ千鳥" },
            { Yaku.Houou        ,   "鳳凰" },
            { Yaku.Hououraigi   ,   "鳳凰来儀" }
        };

        public string GetYakuName(Yaku yaku)
        {
            return yakuNames.TryGetValue(yaku, out string name) ? name : yaku.ToString();
        }

        public void Init()
        {
            yakuText.text = "";
            scoreText.text = "";
            totalScoreText.text = "0";
            yakuPanel.SetActive(false);
        }

        public void Show(List<Yaku> yakus)
        {
            yakuPanel.SetActive(true);
            yakuText.text = "";
            scoreText.text = "";

            int totalScore = 0;

            if (yakus == null)
                return;

            foreach (Yaku yaku in yakus)
            {
                string yakuName = GetYakuName(yaku);
                yakuText.text += yakuName + "\n";

                int score = YakuData.GetYakuScore(yaku);
                scoreText.text += $"+{score}文\n";

                totalScore += score;
            }

            totalScoreText.text = $"合計:{totalScore}文";
        }

        public void Hide()
        {
            yakuPanel.SetActive(false);
        }
    }
}