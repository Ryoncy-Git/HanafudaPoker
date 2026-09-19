using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Cards;

namespace HanafudaPoker.Animation
{
    // カードprefabを生成
    public class CardFactory : MonoBehaviour
    {
        [Header("自分が見るカード")]
        [SerializeField] private GameObject cardPrefab;
        [Header("他の人の手札")]
        [SerializeField] private GameObject cardFakePrefab;

        // 自分と場の札生成
        public CardView CreateCard(CardData card)
        {
            GameObject obj = Instantiate(cardPrefab);

            CardView view = obj.GetComponent<CardView>();

            view.SetCard(card);

            return view;
        }

        // 見かけ用
        public CardView CreateFakeCard()
        {
            Debug.Log("CreateFakeCard");

            GameObject obj = Instantiate(cardFakePrefab);

            Debug.Log(obj);

            return obj.GetComponent<CardView>();
        }
    }
}