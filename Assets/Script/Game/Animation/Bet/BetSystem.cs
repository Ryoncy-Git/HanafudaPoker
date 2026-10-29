using System.Collections.Generic;
using UnityEngine;

using HanafudaPoker.Network;
using HanafudaPoker.UIs;
using HanafudaPoker.Games;

// ho6:betSystem�́A�l�b�g���[�N����擾�������݂̎������ɉ��������̋��z�𐶐�
namespace HanafudaPoker.Bet
{
    public static class BetSystem
    {
        private static void Start()
        {
            // NetworkManager.SetBet(100);
        }

        public static bool TryBet(int betAmount)
        {
            // ���݂̎�����
            int money = NetworkManager.GetBet();    // ����͏��������擾����֐����~����

            // Bet�z�������𖞂����ĂȂ�(�s���E�}�C�i�X�ɂȂ�)
            if (betAmount <= 0 || betAmount > money)
            {
                return false;
            }


            // Bet�z��ݒ�
            NetworkManager.SetBet(betAmount);

            money = money - betAmount;

            return true;
        }

    }
}