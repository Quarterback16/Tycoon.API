namespace RosterLib
{
   public class PassesCaught : BaseStat
    {

        #region IStat Members

        public override string Name
        {
            get
            {
                return "Passes Caught";
            }
        }

        public override bool IsReasonable()
        {
			//  Michael Thomas caught 16 in week 1 of 2018
            if ((Quantity > 16) || (Quantity < 0))
                return false;
            else
                return true;
        }

        #endregion
    }
}
