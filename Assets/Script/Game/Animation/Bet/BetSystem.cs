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
        // task:�ebet�^�[���ŁA�q�����ő���z�𐧌�����
        public static bool TryBet(int betAmount)
        {
            // ���݂̎�����
            int money = NetworkManager.GetMoney();    // ����͏��������擾����֐����~����
            int allBetValue = NetworkManager.GetBet(); 

            // Bet�z�������𖞂����ĂȂ�(�s���E�}�C�i�X�ɂȂ�)
            if (betAmount + allBetValue > 100 /*����100��GameConst�Ō��߂��Ă�*/|| betAmount > money)
            {
                return false;
            }

            // Bet�z��ݒ�
            NetworkManager.SetBet(betAmount);       // �|�������z
            NetworkManager.AddBet(betAmount);       // ����܂Ŋ|�������z
            NetworkManager.AddMoney(-betAmount);    // ������

            return true;
        }

    }
}