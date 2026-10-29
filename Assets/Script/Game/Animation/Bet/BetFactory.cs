using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HanafudaPoker.Bet
{
    // ネットワークから誰がどのくらいかけたか、を取得して
    // 個人でその金額に合った量のオブジェクトを生成

    /* 貨幣価値
     * 文久永宝 - 5
     * 寛永通宝 - 10
     * 和同開珎 - 50
     */

    public class BetFactory : MonoBehaviour
    {
        [Header("掛金のオブジェクト")]
        public GameObject[] betObj;
        private List<GameObject> createdBetObj;

        [Header("掛金の生成場所")]
        [SerializeField]
        private Transform createTransform;

        private void Start()
        {
            createdBetObj = new List<GameObject>();
        }

        // ボタン制御で受け取る
        public void CreateBetCoin(BetCoin coin)
        {
            GameObject obj =
                Instantiate(betObj[(int)coin],
                    createTransform.position,
                    createTransform.rotation);

            createdBetObj.Add(obj);
        }

        public void ResetBetObjects()
        {
            foreach (var obj in createdBetObj)
                Destroy(obj);

            createdBetObj.Clear();
        }

    }

    public enum BetCoin
    {
        BunkyuEiho,
        KaneiTsuho,
        Wadokaichin
    }
}