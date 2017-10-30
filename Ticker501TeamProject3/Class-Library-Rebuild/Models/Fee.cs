using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Fee : Transaction
    {
        public enum FeeSelect { DepositOrWithdraw, BuyOrSell };

        public const double DEPOSIT = -4.99;
        public const double BUY_OR_SELL = -9.99;

        readonly FeeSelect SelectedFee;
        public Fee(FeeSelect feeSelectIn)
        {
            SelectedFee = feeSelectIn;
        }

        public double GainLossInfluence
        {
            get
            {
                if (SelectedFee == FeeSelect.DepositOrWithdraw)
                {
                    return DEPOSIT;
                }
                else
                {
                    return BUY_OR_SELL;
                }
            }
        }
    }
}
