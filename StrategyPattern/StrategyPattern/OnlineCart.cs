using System.Collections.Generic;

namespace StrategyPattern
{
	public class OnlineCart
	{
		private readonly IDictionary<PaymentType, IPaymentStrategy> paymentStrategies;

		public OnlineCart()
		{
			paymentStrategies = new Dictionary<PaymentType, IPaymentStrategy>();
			paymentStrategies.Add( PaymentType.CreditCard, new PaypalPaymentStrategy() );
			paymentStrategies.Add( PaymentType.GoogleCheckout, new GoogleCheckoutPaymentStrategy() );
			paymentStrategies.Add( PaymentType.AmazonPayments, new AmazonPaymentsPaymentStrategy() );
			paymentStrategies.Add( PaymentType.Paypal, new PaypalPaymentStrategy() );			
		}

		public void CheckOut( PaymentType paymentType )
		{
			paymentStrategies[ paymentType ].ProcessPayment();
		}

		public enum PaymentType
		{
			CreditCard,
			GoogleCheckout,
			AmazonPayments,
			Paypal
		}
	}
}
