// 1. 탄막 슈팅 게임이 무엇인가 ? (기획)
//  콘솔 화면에서 위에서 몬스터가 내려온다 가끔씩 커다란 몬스터도 나온다 
// 내 비행기는 방향키로 조종할수있다 
// 내 비행기에서는 탄막이 나가고 그 탄막이 몬스터를 잡을수있다 
// 몬스터를 잡으면 가끔 업그레이드 탄막이 나온다 
//
//
// 2. 어떤 구조로 만들어야 하는가 ? (시스템 설계)
// (1) 게임 루프를 반드시 지켜서 만들어라 
// (2) - 게임 루프를 입력, 갱신 ,렌더링을 프레임(한 바퀴)로 표현한것      
// (3)   입력 (input) 이벤트로 만들어줘 
// (4)   갱신 (update) delta time 으로 만들어줘 
// (5)   렌더 (Render) 화면에 그려줘 
// (6) C# 으로 만들어라 .
//
//
//부록. 최상문을 활용해서 Main코드를 간략하게 실행할수있는 구조로 만드세요 
// 예를 들어 Game.Run() 같은 형태여야 한다 . 
// Game 클래스 시스템 설계를 충실히 구성하기 위해 여러개의 클래스로 구성해라.



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

// 최상위 문 : 게임 실행
Game game = new Game();
game.Run();


// ========================================
// 1. 게임 전체 관리 클래스
// ========================================

class Game
{
    public const int Width = 40;
    public const int Height = 25;

    private bool isRunning = true;

    private Player player = new Player();
    private InputManager input = new InputManager();
    private Renderer renderer = new Renderer();

    private List<Bullet> bullets = new List<Bullet>();
    private List<Enemy> enemies = new List<Enemy>();
    private List<UpgradeItem> items = new List<UpgradeItem>();

    private Random random = new Random();

    private float spawnTimer = 0;
    private float shootTimer = 0;

    private int score = 0;

    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;

        // 입력 이벤트 연결
        input.OnKeyPressed += HandleInput;

        Stopwatch stopwatch = Stopwatch.StartNew();
        double previousTime = stopwatch.Elapsed.TotalSeconds;

        // 게임 루프
        while (isRunning)
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds;

            float deltaTime =
                (float)(currentTime - previousTime);

            previousTime = currentTime;

            // 1. 입력
            input.ReadInput();

            // 2. 갱신
            Update(deltaTime);

            // 3. 렌더링
            Render();

            Thread.Sleep(16);
        }

        Console.Clear();
        Console.CursorVisible = true;

        Console.WriteLine("===== GAME OVER =====");
        Console.WriteLine($"최종 점수 : {score}");
    }

    // 입력 이벤트 처리
    private void HandleInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                player.Move(-1, 0);
                break;

            case ConsoleKey.RightArrow:
                player.Move(1, 0);
                break;

            case ConsoleKey.UpArrow:
                player.Move(0, -1);
                break;

            case ConsoleKey.DownArrow:
                player.Move(0, 1);
                break;

            case ConsoleKey.Escape:
                isRunning = false;
                break;
        }
    }

    // 게임 상태 갱신
    private void Update(float deltaTime)
    {
        // 몬스터 생성 시간
        spawnTimer += deltaTime;

        if (spawnTimer >= 1.0f)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }

        // 자동 공격 시간
        shootTimer += deltaTime;

        if (shootTimer >= 0.3f)
        {
            player.Shoot(bullets);
            shootTimer = 0;
        }

        // 총알 이동
        foreach (Bullet bullet in bullets)
        {
            bullet.Update(deltaTime);
        }

        // 몬스터 이동
        foreach (Enemy enemy in enemies)
        {
            enemy.Update(deltaTime);
        }

        // 아이템 이동
        foreach (UpgradeItem item in items)
        {
            item.Update(deltaTime);
        }

        CheckCollision();

        // 화면 밖으로 나간 객체 제거
        bullets.RemoveAll(b => b.Y < 0);
        enemies.RemoveAll(e => e.Y >= Height);
        items.RemoveAll(i => i.Y >= Height);

        if (player.HP <= 0)
        {
            isRunning = false;
        }
    }

    // 몬스터 생성
    private void SpawnEnemy()
    {
        bool isBig = random.Next(100) < 20;

        int x = random.Next(1, Width - 1);

        Enemy enemy = new Enemy(x, isBig);

        enemies.Add(enemy);
    }

    // 충돌 검사
    private void CheckCollision()
    {
        // 총알과 몬스터의 충돌
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            Bullet bullet = bullets[i];

            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                Enemy enemy = enemies[j];

                if (enemy.IsHit(bullet.X, bullet.Y))
                {
                    enemy.HP--;
                    bullets.RemoveAt(i);

                    if (enemy.HP <= 0)
                    {
                        score += enemy.IsBig ? 50 : 10;

                        // 20% 확률로 업그레이드 생성
                        if (random.Next(100) < 20)
                        {
                            items.Add(
                                new UpgradeItem(enemy.X, enemy.Y)
                            );
                        }

                        enemies.RemoveAt(j);
                    }

                    break;
                }
            }
        }

        // 플레이어와 몬스터의 충돌
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];

            if (enemy.IsHit(player.X, player.Y))
            {
                player.HP--;
                enemies.RemoveAt(i);
            }
        }

        // 플레이어와 아이템의 충돌
        for (int i = items.Count - 1; i >= 0; i--)
        {
            UpgradeItem item = items[i];

            if (player.X == item.X &&
                player.Y == (int)item.Y)
            {
                player.Upgrade();
                items.RemoveAt(i);
            }
        }
    }

    // 화면 출력
    private void Render()
    {
        renderer.Draw(
            player,
            bullets,
            enemies,
            items,
            score
        );
    }
}


// ========================================
// 2. 입력 관리 클래스
// ========================================

class InputManager
{
    public event Action<ConsoleKey>? OnKeyPressed;

    public void ReadInput()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey key =
                Console.ReadKey(true).Key;

            OnKeyPressed?.Invoke(key);
        }
    }
}


// ========================================
// 3. 플레이어 클래스
// ========================================

class Player
{
    public int X = Game.Width / 2;
    public int Y = Game.Height - 2;

    public int HP = 3;
    public int Level = 1;

    public void Move(int x, int y)
    {
        X += x;
        Y += y;

        // 화면 밖으로 이동하지 못하도록 제한
        X = Math.Clamp(X, 0, Game.Width - 1);
        Y = Math.Clamp(Y, 0, Game.Height - 1);
    }

    public void Shoot(List<Bullet> bullets)
    {
        // 기본 중앙 공격
        bullets.Add(new Bullet(X, Y - 1));

        // 2단계 : 왼쪽 탄막 추가
        if (Level >= 2 && X > 0)
        {
            bullets.Add(new Bullet(X - 1, Y - 1));
        }

        // 3단계 : 오른쪽 탄막 추가
        if (Level >= 3 && X < Game.Width - 1)
        {
            bullets.Add(new Bullet(X + 1, Y - 1));
        }
    }

    public void Upgrade()
    {
        if (Level < 3)
        {
            Level++;
        }
    }
}


// ========================================
// 4. 총알 클래스
// ========================================

class Bullet
{
    public int X;
    public float Y;

    public float Speed = 18.0f;

    public Bullet(int x, float y)
    {
        X = x;
        Y = y;
    }

    public void Update(float deltaTime)
    {
        Y -= Speed * deltaTime;
    }
}


// ========================================
// 5. 몬스터 클래스
// ========================================

class Enemy
{
    public int X;
    public float Y = 0;

    public int HP;
    public float Speed;
    public bool IsBig;

    public Enemy(int x, bool isBig)
    {
        X = x;
        IsBig = isBig;

        if (IsBig)
        {
            HP = 5;
            Speed = 2.0f;
        }
        else
        {
            HP = 1;
            Speed = 4.0f;
        }
    }

    public void Update(float deltaTime)
    {
        Y += Speed * deltaTime;
    }

    // 해당 위치가 몬스터와 충돌하는지 검사
    public bool IsHit(int x, float y)
    {
        if ((int)Y != (int)y)
        {
            return false;
        }

        if (IsBig)
        {
            return Math.Abs(X - x) <= 1;
        }

        return X == x;
    }
}


// ========================================
// 6. 업그레이드 아이템 클래스
// ========================================

class UpgradeItem
{
    public int X;
    public float Y;

    public float Speed = 3.0f;

    public UpgradeItem(int x, float y)
    {
        X = x;
        Y = y;
    }

    public void Update(float deltaTime)
    {
        Y += Speed * deltaTime;
    }
}


// ========================================
// 7. 렌더링 클래스
// ========================================

class Renderer
{
    public void Draw(
        Player player,
        List<Bullet> bullets,
        List<Enemy> enemies,
        List<UpgradeItem> items,
        int score)
    {
        char[,] screen =
            new char[Game.Height, Game.Width];

        // 화면 초기화
        for (int y = 0; y < Game.Height; y++)
        {
            for (int x = 0; x < Game.Width; x++)
            {
                screen[y, x] = ' ';
            }
        }

        // 총알 그리기
        foreach (Bullet bullet in bullets)
        {
            SetPixel(
                screen, bullet.X, (int)bullet.Y, '|'
            );
        }

        // 몬스터 그리기
        foreach (Enemy enemy in enemies)
        {
            int y = (int)enemy.Y;

            if (enemy.IsBig)
            {
                SetPixel(screen, enemy.X - 1, y, '[');
                SetPixel(screen, enemy.X, y, 'M');
                SetPixel(screen, enemy.X + 1, y, ']');
            }
            else
            {
                SetPixel(screen, enemy.X, y, 'V');
            }
        }

        // 아이템 그리기
        foreach (UpgradeItem item in items)
        {
            SetPixel(
                screen, item.X, (int)item.Y, 'U'
            );
        }

        // 플레이어 그리기
        SetPixel(screen, player.X, player.Y, 'A');

        // 출력 위치 초기화
        Console.SetCursorPosition(0, 0);

        Console.WriteLine(
            $"HP:{player.HP} LEVEL:{player.Level} SCORE:{score}"
            .PadRight(Game.Width + 2)
        );

        Console.WriteLine(
            "+" + new string('-', Game.Width) + "+"
        );

        for (int y = 0; y < Game.Height; y++)
        {
            Console.Write("|");

            for (int x = 0; x < Game.Width; x++)
            {
                Console.Write(screen[y, x]);
            }

            Console.WriteLine("|");
        }

        Console.WriteLine(
            "+" + new string('-', Game.Width) + "+"
        );
    }

    private void SetPixel(
        char[,] screen, int x, int y, char symbol)
    {
        if (x >= 0 && x < Game.Width &&
            y >= 0 && y < Game.Height)
        {
            screen[y, x] = symbol;
        }
    }
}
