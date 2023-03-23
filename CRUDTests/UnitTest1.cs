namespace CRUDTests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			MyMath myMath = new MyMath();
			int num1 = 10, num2 = 20;
			int expected = 30;

			int actualNum = myMath.Add(num1, num2);

			Assert.Equal(expected, actualNum);
		}
	}
}