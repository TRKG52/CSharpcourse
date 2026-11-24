Player hero = new Player();
hero.Name = "용사";
Player slime = new Player();
slime.Name = "슬라임";
slime.Hp = 30;

hero.TakeDamage(25);
hero.PrintStatus();
slime.PrintStatus();

class Player
{
    public string Name = "";
    public int Hp = 100;

    public void TakeDamage(int amount)
    {
        Hp -= amount;
    }

    public void PrintStatus()
    {
        Console.WriteLine($"{Name}: HP {Hp}");
    }
}