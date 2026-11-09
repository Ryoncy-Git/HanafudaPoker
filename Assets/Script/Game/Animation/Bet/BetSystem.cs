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


            // 現在のターンから最大ベット数を確認
            TurnState currentTurn = (TurnState)NetworkManager.GetTurnState();
            int betMaximun = 0;
            switch (currentTurn)
            {
                case TurnState.WaitForFirstBet:
                    betMaximun = BetConst.FIRST_BET_MAXIMUM;
                    break;
                
                case TurnState.WaitForSecondBet:
                    betMaximun = BetConst.SECOND_BET_MAXIMUM;
                    break;
                
                case TurnState.WaitForThirdBetAndInsurance:
                    betMaximun = BetConst.THIRD_BET_MAXIMUM;
                    break;

                case TurnState.WaitForKoikoiBetAndInsurance:
                    betMaximun = BetConst.KOIKOI_BET_MAMIMUM; 
                    break;

                default :
                    Debug.Log("ベットターンではありません");
                    break;
            }


            // 金額不足か最大ベット数を超えていたら終了
            if (betAmount + allBetValue > betMaximun || betAmount > money)
            {
                return false;
            }

            // Bet�z��ݒ� 
            NetworkManager.AddBet(betAmount);       // ����܂Ŋ|�������z
            NetworkManager.AddMoney(-betAmount);    // ������

            return true;
        }

        public static void ResetBetMount()
        {
            NetworkManager.SetBet(0);
        }

        public static void DistributeInitFunds()
        {
            NetworkManager.SetMoney(BetConst.INIT_FUNDS);
        }

        public static void SetInsurance()
        {
            // 小数切り捨て
            int insuranceAmount = 
                (int)((float)NetworkManager.GetBet() * BetConst.INSURANCE_COST_MULTIPLIER);

            
            if(insuranceAmount < 0)
            {
                Debug.Log("Failed To Set Insurance");
                return;
            }

            NetworkManager.SetInsurace(insuranceAmount);
        }

        public static void ResetInsurance()
        {
            NetworkManager.SetInsurace(0);
        }

        public static void OnInsuranceButton()
        {
            if(NetworkManager.GetInsurace() > 0)
            {
                ResetInsurance();
            }
            else
            {
                SetInsurance();
            }
        }
    }
}