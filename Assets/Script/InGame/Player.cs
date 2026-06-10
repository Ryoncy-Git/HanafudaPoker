using System.Collections.Generic;

using HanafudaPoker.Cards;

namespace HanafudaPoker.Players
{
    public class PlayerData
    {
        public int SeatID;
        public int ActorNumber;
        public int Point;
        public List<CardData> HandCards;
        public bool IsReady;
        public bool[] WillChangeCards;

        public PlayerData(int seatId, int Id)
        {
            SeatID = seatId;
            ActorNumber = Id;
            Point = 0;
            HandCards = new();
            IsReady = false;
            WillChangeCards = new[] {false, false, false};// 手札が3枚なので
        }
    }
}