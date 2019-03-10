namespace RosterLib
{
   public class PassAttempts : BaseStat
    {
        #region IStat Members

        public override string Name
        {
            get
            {
                return "Pass Attempts";
            }
        }

        public override bool IsReasonable()
        {
			//  Andrew Luck had 62 attempts in Week 4 of 2018
            if ( (Quantity > 62) || ( Quantity < 0 ) )
                return false;
            else
                return true;
        }

        #endregion
    }
}
