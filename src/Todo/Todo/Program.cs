namespace FlowStudy
{

    static class Program
    {
        static void Main(string[] args)
        {
            Console.Write("가격을 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int price))
            {
                Console.WriteLine($"2개 가격: {price * 2}");
            }
            else
            {
                Console.WriteLine("정수로 입력해주세요.");

            }
            Console.WriteLine(5 / 2);
            Console.WriteLine(5 / 2.0);
        }
    }
}