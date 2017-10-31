using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRebuild
{
    public class Fee_M : Transaction
    {
        public enum FeeSelect { DepositOrWithdraw, BuyOrSell }; //enum for selecting the type of fee to be applied

        public const double DEPOSIT = -4.99;
        public const double BUY_OR_SELL = -9.99;

        readonly FeeSelect SelectedFee;

        /// <summary>
        /// Constructor for Fee object
        /// </summary>
        /// <param name="feeSelectIn">Type of fee to be applied</param>
        public Fee_M(FeeSelect feeSelectIn)
        {
            SelectedFee = feeSelectIn;
        }

        /// <summary>
        /// Getter for fee amount based on enum value
        /// </summary>
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
