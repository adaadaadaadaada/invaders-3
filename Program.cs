using System;
using System.Numerics;
using System.Security.Cryptography;
using Raylib_CsLo;
using static System.Formats.Asn1.AsnWriter;

namespace invaders
{
    internal class Program
    {

        enum GameState
        {
            Start,
            Play,
            ScoreScreen,
            SettingsMenu,
            PauseMenu,
            Menu,
            DevMenu
        }

        Stack<GameState> stateStack = new Stack<GameState>();

        int window_width = 680;
        int window_height = 420;

        Player player;
        List<Bullet> bullets;
        List<Enemy> enemies;
        Menu menu;
        SettingsMenu settingsMenu;
        PauseMenu pauseMenu;
        DevMenu devMenu;

        double enemyShootInterval = 1.0f;
        double lastEnemyShootTime = 5.0f;
        float enemyBulletSpeed;
        float enemyBulletSize;
        float enemyMaxYLine;
        float enemySpeedDown;

        public Texture playerImage;
        public Texture enemyImage;
        public Texture background;
        public Sound shootSound;
        public Music music;
        public Font font1;
        public Font font2;
        public int fontSize1 = 45;
        public int fontSize2 = 60;

        int scoreCounter = 0;

        public static void Main()
        {
            Program Invaders = new Program();
            Invaders.Run();
        }

        public void Run()
        {
            Init();
            GameLoop();
        }

        void Init()
        {
            Raylib.InitWindow(window_width, window_height, "Space Invaders");
            Raylib.SetExitKey(KeyboardKey.KEY_TAB);
            Raylib.SetTargetFPS(30);

            Raylib.InitAudioDevice();
            RayGui.GuiLoadStyle("style.rgs");

            playerImage = Raylib.LoadTexture("images/ufo.png");
            enemyImage = Raylib.LoadTexture("images/ufo2.png");
            background = Raylib.LoadTexture("images/space.png");
            shootSound = Raylib.LoadSound("sounds/sound.wav");
            music = Raylib.LoadMusicStream("sounds/music.mp3");
            font1 = Raylib.LoadFontEx("fonts/dpcomic.ttf", fontSize: 45, 250);
            font2 = Raylib.LoadFontEx("fonts/joystix monospace.otf", fontSize: 20, 250);

            Raylib.PlayMusicStream(music);
            Raylib.SetMusicVolume(music, 2f);
            stateStack.Push(GameState.Start);
            //statestack = GameState.Start; <-- vanha

            ResetGame();
        }

        void OnStartButtonPressed(Object sender, EventArgs e)
        {
            ResetGame();
            stateStack.Push(GameState.Play);
        }

        void OnSettingsBackPressed(Object sender, EventArgs e)
        {
            stateStack.Pop();
        }

        void OnSettingsButtonPressed(Object sender, EventArgs e)
        {
            stateStack.Push( GameState.SettingsMenu);
        }
        void OnQuitButtonPressed(Object sender, EventArgs e)
        {
            Raylib.CloseWindow();
        }
        void OnPauseSettingsButtonPressed(Object sender, EventArgs e)
        {
            stateStack.Push( GameState.SettingsMenu);
        }
        void OnPauseButtonPressed(Object sender, EventArgs e)
        {
            stateStack.Push( GameState.PauseMenu);
        }
        void OnPauseBackPressed (object sender, EventArgs e)
        {
            stateStack.Pop();
        }
        void OnRestartButtonPressed(object sender, EventArgs e)
        {
            ResetGame();
        }
        void OnInvincibilityButtonPressed(object sender, EventArgs e)
        {
            player.InvincibilityMode();

            stateStack.Pop();
        }
        void OnDestroyButtonPressed(object sender, EventArgs e)
        {
            enemies.Clear();
        }
        void OnDevBackButtonPressed(object sender, EventArgs e)
        {
            stateStack.Pop();
        }
        void OnDevModePressed(object sender, EventArgs e)
        {
            // go to debug mode
        }

        /// <summary>
        /// Tämä funktio laittaa pelin takaisin aloitustilaan
        /// </summary>
        void ResetGame()
        {
            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(window_width / 2, window_height - playerSize * 2);
            player = new Player(playerStart, playerSpeed, playerSize, playerImage);

            menu = new Menu(font2, fontSize2);
            menu.StartButtonPressed += OnStartButtonPressed;
            menu.SettingsButtonPressed += OnSettingsButtonPressed;

            settingsMenu = new SettingsMenu(font2, fontSize2);
            settingsMenu.BackButtonPressed += OnSettingsBackPressed;

            pauseMenu = new PauseMenu(font2, fontSize2);
            pauseMenu.BackButtonPressed += OnPauseBackPressed;
            pauseMenu.QuitButtonPressed += OnQuitButtonPressed;
            pauseMenu.SettingsButtonPressed += OnPauseSettingsButtonPressed;
            pauseMenu.RestartButtonPressed += OnRestartButtonPressed;
            pauseMenu.InvincibilityButtonPressed += OnInvincibilityButtonPressed;
            pauseMenu.DestroyButtonPressed += OnDestroyButtonPressed;

            devMenu = new DevMenu(font2, fontSize2);
            devMenu.DevBackButtonPressed += OnDevBackButtonPressed;
            devMenu.DevModeButtonPressed += OnDevModePressed;

            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            int rows = 2;
            int columns = 4;
            int startX = 0;
            int startY = 0;
            int currentX = startX;
            int currentY = startY;
            int enemyBetween = playerSize;
            enemyBulletSize = 10;
            enemyBulletSpeed = 60;
            enemyMaxYLine = window_height - 4 * playerSize;
            enemySpeedDown = 10;

            int maxScore = 40;
            int minScore = 10;
            int currentScore = maxScore;

            for (int row = 0; row < rows; row++)
            {
                currentX = startX;

                currentScore = maxScore - row * 10;
                if (currentScore < minScore)
                {
                    currentScore = minScore;
                }

                for (int col = 0; col < columns; col++)
                {
                    Vector2 enemyStart = new Vector2(currentX, currentY);
                    int enemyScore = currentScore;

                    Enemy enemy = new Enemy(enemyStart, new Vector2(1, 0), playerSpeed, playerSize, enemyScore, enemyImage);

                    enemies.Add(enemy);

                    currentX += playerSize + enemyBetween; // Horizontal space between enemies
                }
                currentY += playerSize + enemyBetween; // Vertical space between enemies
            }
        }

        void GameLoop() // peli
        {
            while (Raylib.WindowShouldClose() == false)
            {
                Raylib.UpdateMusicStream(music);
                switch (stateStack.Peek())

                // Eri peliruudut ja niiden sisällä toimivat metodit
                {
                    case GameState.Start:
                        UpdateStart();
                        Raylib.BeginDrawing();
                        Raylib.EndDrawing();
                        break;

                    case GameState.Play:
                        Update();

                        Raylib.BeginDrawing();
                        Raylib.DrawTextureEx(background, new Vector2(0,0), 0, 1.2f, Raylib.WHITE);
                        Draw();
                        Raylib.EndDrawing();
                        break;

                    case GameState.ScoreScreen:
                        ScoreUpdate();
                        Raylib.BeginDrawing();
                        ScoreDraw();
                        Raylib.EndDrawing();
                        break;

                    case GameState.Menu:
                        menu.DrawStartScreen();
                        Raylib.EndDrawing();
                        break;

                    case GameState.SettingsMenu:
                        settingsMenu.DrawSettingsMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.PauseMenu:
                        pauseMenu.DrawPauseMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.DevMenu:
                        devMenu.DrawDevMenu();
                        Raylib.EndDrawing();
                        break;
                }
            }
        }

        void UpdateStart()
        { 
            menu.DrawStartScreen();

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                stateStack.Push( GameState.Play);
            }
        }

        void UpdatePlayer()
        {
            bool playerShoots = player.Update();
            KeepInsideArea(player.transform, player.collision, 0, 0, window_width, window_height);
            if (playerShoots)
            {
                Raylib.PlaySound(shootSound);
                CreateBullet(player.transform.position, new Vector2(0, -1), 300, 20);

                Console.WriteLine($"Bullet count: {bullets.Count()}");
            }
        }

        void UpdateEnemies()
        {
            bool changeFormationDirection = false;
            bool canGoDown = true;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    enemy.Update();
                    bool enemyIn = IsInsideArea(enemy.transform, enemy.collision, 0, 0, window_width, window_height);
                    if (enemyIn == false)
                    {
                        changeFormationDirection = true;
                    }
                    if (enemy.transform.position.Y > enemyMaxYLine)
                    {
                        canGoDown = false;
                    }
                }
            }
            if (changeFormationDirection)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    if (canGoDown)
                    {
                        enemy.transform.position.Y += enemySpeedDown;
                    }
                }
            }

            double timeNow = Raylib.GetTime();
            if (timeNow - lastEnemyShootTime >= enemyShootInterval)
            {
                Enemy shooter = FindBestEnemyShooter();
                if (shooter != null)
                {
                    CreateBullet(shooter.transform.position + new Vector2(0, shooter.collision.size.Y), new Vector2(0, 1), enemyBulletSpeed, (int)enemyBulletSize);
                    lastEnemyShootTime = timeNow;
                }
            }
        }

        Enemy FindBestEnemyShooter()
        {
            Enemy best = null;

            float bestY = 0.0f;
            float bestXDifference = window_width;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy test = enemies[i];
                if (test.active)
                {
                    if (test.transform.position.Y >= bestY)
                    {
                        bestY = test.transform.position.Y;

                        float xDifference = Math.Abs(player.transform.position.X - test.transform.position.X);
                        if (xDifference < bestXDifference && xDifference < 10)
                        {
                            bestXDifference = xDifference;
                            best = test;
                        }
                    }
                }
            }
            return best;
        }

        void UpdateBullets()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Update();

                bool isOutside = KeepInsideArea(bullet.transform, bullet.collision, 0, 0, window_width, window_height);

                if (isOutside)
                {
                    bullet.active = false;
                    continue;
                }
            }
        }

        void Update()
        {
            UpdatePlayer();
            UpdateBullets();
            UpdateEnemies();
            CheckCollisions();
            
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {
                OnPauseButtonPressed(this, EventArgs.Empty);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE))
            {
# if DEBUG
                stateStack.Push(GameState.DevMenu);
#endif
            }
        }

        void CheckCollisions()
        {
            Rectangle playerRect = getRectangle(player.transform, player.collision);

            foreach (Enemy enemy in enemies)
            {
                if (enemy.active == false)
                {
                    continue;
                }
                Rectangle enemyRec = getRectangle(enemy.transform, enemy.collision);

                foreach (Bullet bullet in bullets)
                {
                    if (bullet.active)
                    {
                        Rectangle bulletRec = getRectangle(bullet.transform, bullet.collision);
                        if (bullet.transform.direction.Y < 0)
                        {
                            if (Raylib.CheckCollisionRecs(bulletRec, enemyRec))
                            {
                                Console.WriteLine("Enemy hit!" + enemy.scoreValue);
                                scoreCounter += enemy.scoreValue;
                                enemy.active = false;
                                bullet.active = false;

                                int enemiesLeft = CountAliveEnemies();
                                if (enemiesLeft == 0)
                                {
                                    stateStack.Push( GameState.ScoreScreen);
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (Raylib.CheckCollisionRecs(bulletRec, playerRect))
                            {
                                if (player.isInvincible == false)
                                {
                                    stateStack.Push(GameState.ScoreScreen);
                                }
                            }
                        }
                    }
                }
            }
        }

        void Draw()
        {
            player.Draw();
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw();
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    enemy.Draw();
                }

            }
            Raylib.DrawText(scoreCounter.ToString(), 10, 10, 16, Raylib.BLACK);
        }

        /// <summary>
        /// Metodi luo ammuksia pelaajalle ja vihollisille
        /// </summary>
        /// <param name="pos">kohta johon ammus luodaan</param>
        /// <param name="dir">suunta mihin ammus lähtee</param>
        /// <param name="speed">ammuksen nopeus</param>
        /// <param name="size">ammuksen koko</param>
        void CreateBullet(Vector2 pos, Vector2 dir, float speed, int size)
        {
            bool found = false;
            foreach (Bullet bullet in bullets)
            {
                if (bullet.active == false)
                {
                    bullet.Reset(pos, dir, speed, size);
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                bullets.Add(new Bullet(pos, dir, speed, size));
            }
        }

        Rectangle getRectangle(TransformComponent t, CollisionComponent c)
        {
            Rectangle r = new Rectangle(t.position.X, t.position.Y, c.size.X, c.size.Y);
            return r;
        }

        bool KeepInsideArea(TransformComponent transform, CollisionComponent collision, int left, int top, int right, int bottom)
        {
            float newX = Math.Clamp(transform.position.X, left, right - collision.size.X);
            float newY = Math.Clamp(transform.position.Y, top, bottom - collision.size.Y);

            bool xChange = newX != transform.position.X;
            bool yChange = newY != transform.position.Y;

            transform.position.X = newX;
            transform.position.Y = newY;

            return xChange || yChange;
        }

        bool IsInsideArea(TransformComponent transform, CollisionComponent collision, int left, int top, int right, int bottom)
        {
            float x = transform.position.X;
            float r = x + collision.size.X;

            float y = transform.position.Y;
            float b = y + collision.size.Y;

            if (x < left || y < top || r > right || b > bottom)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        int CountAliveEnemies()
        {
            int alive = 0;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    alive++;
                }
            }
            return alive;
        }

        void ScoreDraw()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                ResetGame();
                stateStack.Push( GameState.Play);
            }
        }
        void ScoreUpdate()
        {
            string scoreText = $"Final score {scoreCounter}";
            string instructionText = "Game over. Press SPACE to play again";
            if (CountAliveEnemies() == 0)
            {
                instructionText = "You won! Press SPACE to play again";
            }

            int fontSize = 20;
            Vector2 sw = Raylib.MeasureTextEx(font1, scoreText, fontSize1, 2);
            int iw = Raylib.MeasureText(instructionText, fontSize);

            Raylib.DrawTextEx(font1, scoreText, new Vector2(window_width / 2 - sw.X / 2, window_height / 2 - 60), fontSize1, 2, Raylib.WHITE);

            Raylib.DrawText(instructionText, window_width / 2 - iw / 2, window_height / 2, fontSize, Raylib.WHITE);
        }

        void PlaySound(Sound music)
        {

        }
    }
}
