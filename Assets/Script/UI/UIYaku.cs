using UnityEngine;
using TMPro;
using System.Collections.Generic;

using HanafudaPoker.Yakus;
using HanafudaPoker.Network;

namespace HanafudaPoker.UIs
{
    public class UIYaku : MonoBehaviour
    {
        [SerializeField] private TMP_Text yakuText;

        private readonly Dictionary<Yaku, string> yakuNames = new()
        {
            { Yaku.Tsui         ,   "‘Î" },
            { Yaku.Nitsui       ,   "“ñ‘Î" },
            { Yaku.Santsui      ,   "O‘Î" },
            { Yaku.Yontsui      ,   "l‘Î" },
            { Yaku.Mangetsu     ,   "–Œ" }, // ŒD‚ª4–‡‚ ‚é
            { Yaku.Akatan       ,   "Ôƒ^ƒ“" },
            { Yaku.Aotan        ,   "Âƒ^ƒ“" },
            { Yaku.Tan          ,   "ƒ^ƒ“" },
            { Yaku.Gokou        ,   "ŒÜŒõ" },
            { Yaku.Yonkou       ,   "lŒõ" },
            { Yaku.Ameshikou    ,   "‰J“ü‚èlŒõ" },
            { Yaku.Sankou       ,   "OŒõ" },
            { Yaku.Inoshikacho  ,   "’–­’±" },
            { Yaku.Sakeutage    ,   "ğ‰ƒ" }, // ğDE–ŒE÷‚É–‹
            { Yaku.Mizu         ,   "…" },
            { Yaku.Murasaki     ,   "‡" },
            { Yaku.Hanaikada    ,   "‰Ô”³" },
            { Yaku.Adabana      ,   "“k‰Ô" },
            { Yaku.Chidori      ,   "ç’¹" },
            { Yaku.MidareChidori,   "—‚êç’¹" },
            { Yaku.Houou        ,   "–P™€" },
            { Yaku.Hououraigi   ,   "–P™€—ˆ‹V" }
        };

        public string GetYakuName(Yaku yaku)
        {
            return yakuNames.TryGetValue(yaku, out string name) ? name : yaku.ToString();
        }

        public void InitShowYaku()
        {
            yakuText.text = "–ğ";
        }

        public void ShowYaku(List<Yaku> yakus)
        {
            yakuText.text = "";

            if (yakus == null)
                return;

            foreach (Yaku yaku in yakus)
            {
                string yakuName = GetYakuName(yaku);
                yakuText.text += yakuName + "\n";
            }
        }

    }
}