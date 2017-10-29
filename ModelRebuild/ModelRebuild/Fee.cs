using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Fee : Transaction
    {
        enum FeeSelect { Deposit, BuyOrSell };

        const double DEPOSIT = -4.99;
        const double BUY_OR_SELL = -9.99;

        readonly FeeSelect SelectedFee;
        Fee(FeeSelect feeSelectIn)
        {
            SelectedFee = feeSelectIn;
        }

        public double GainLossInfluence
        {
            get
            {
                if (SelectedFee == FeeSelect.Deposit)
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
