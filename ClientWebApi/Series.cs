namespace ClientWebApi
{
    public class Series
    {
        public static IEnumerable<int> Fibonacci(int n)
        {
            var fibonacci = new int[n];
            fibonacci[0] = 1;
            fibonacci[1] = 1;
            for (int i = 2; i < fibonacci.Length; i++)
                fibonacci[i] = fibonacci[i - 1] + fibonacci[i - 2];

            return fibonacci;
        }

        public static IEnumerable<int> Factorial(long n)
        {
            if (n <= 0)
                yield break;

            var factorial = 1;
            for (int i = 1; i < n; i++)
                yield return factorial *= i;
        }

        public static IEnumerable<int> Random(int n)
        {
            if (n <= 0)
                yield break;

            var random = new Random();
            for (int i = 0; i < n; i++)
                yield return random.Next(1, n);
        }
    }
}
