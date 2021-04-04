using System;

namespace EnterpriseAppDev
{
	public record Shape(
		string Name);
	public record Product(
		int Quantity, 
		float UnitPrice);

	class Program
	{
		static void Main(string[] args)
		{
			Shape s1 = new Shape("Shape");
			Shape s2 = new Shape("Shape");
			Console.WriteLine(s1.ToString());
			Console.WriteLine($"Hascode of s1 is {s1.GetHashCode()}");
			Console.WriteLine($"Hascode of s2 is {s2.GetHashCode()}");
			Console.WriteLine($"Is s1 equal to s2 : {s1 == s2}");
		}

		static float GetProductDiscount(Product product)
		{
			float discount = product switch
			{
				Product p when p.Quantity is >= 10 and < 20 => 0.05F,
				Product p when p.Quantity is >= 20 and < 50 => 0.10F,
				Product p when p.Quantity is >= 50 => 0.10F,
				_ => throw new ArgumentException(nameof(product))
			};
			return discount * product.UnitPrice * product.Quantity;
		}

		static string GenerateOrderReport(
			Func<string> getFormatString)
		{
			var order = new
			{
				Orderid = 1,
				OrderDate = DateTime.Now
			};

			return string.Format(getFormatString(), order.Orderid);
		}
		void GenerateSummary(string[] args)
		{
			GenerateOrderReport(
			static () =>
			{
				return $"Order Id:{0}, Order Date:{1}";
			});
		}
	}
}
