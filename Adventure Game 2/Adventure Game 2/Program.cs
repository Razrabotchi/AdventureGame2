using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;
using System.ComponentModel.Design;
using System.Security.Cryptography;

namespace ConsoleApp8
{
    internal class GameMainObject    //This is the main game object responsible for rendering the scene
    {
        public char[,] map;
        char[] bag;
        Player player;
        Zombie zombie;
        SoundPlayer soundtrack, win, lose, keyboard;
        SoundPlayer powerDown, hello, helloError, helloError2, helpMe, dontCaught, happenBad;
        string message;
        long count = 0;
        int userX, userY, zombieX, zombieY;
        int userLastX, userLastY, zombieLastX, zombieLastY;
        int exclCount = 0, stoneCount = 0;
        public bool stoneWas = false, exclWas = false, shield = false,
        shieldWas = false, shieldNow = false, question = false;
        bool isPlaying = false, isWin = false, isSecret = false;
        bool blade = false, lever = false, rope = false;
        bool music;
        public GameMainObject
            (
            char[,] map, Player player, Zombie zombie, SoundPlayer soundtrack, SoundPlayer win, SoundPlayer lose, SoundPlayer keyboard,
            SoundPlayer powerDown, SoundPlayer hello, SoundPlayer helloError, SoundPlayer helloError2, SoundPlayer helpMe, 
            SoundPlayer dontCaught, SoundPlayer happenBad, bool music
            )
        {
            this.map = map;
            this.player = player;
            this.bag = player.bag;
            this.userX = player.x;
            this.userY = player.y;
            this.zombie = zombie;
            this.zombieX = zombie.x;
            this.zombieY = zombie.y;
            this.soundtrack = soundtrack;
            this.win = win;
            this.lose = lose;
            this.keyboard = keyboard;
            this.powerDown = powerDown;
            this.hello = hello;
            this.helloError = helloError;
            this.helloError2 = helloError2;
            this.helpMe = helpMe;
            this.dontCaught = dontCaught;
            this.happenBad = happenBad;
            this.music = music;
        }
        public void Informing()
        {
            Console.SetWindowSize(120, 30);
            message = "You have entered the lair of a very dangerous and powerful zombie boss. After you collected everything that was in the house, a zombie boss broke into it. Apparently, there are still some objects in the house and among them there are parts of an old axe (handle | and blade &) The zombie boss is certainly strong, but he has no chance against the axe. All you have to do is find the axe pieces and combine them with a rope (6), which you also need to find to defeat the zombie boss and get out of here. Good luck!";
            Console.SetCursorPosition(94, 29);
            Console.WriteLine("Press any key to skip...");
            Console.SetCursorPosition(0, 0);
            if (music)
            {
                keyboard.Play();
            }
            foreach (char j in message)
            {
                Console.Write(j);
                Thread.Sleep(70);
                if (Console.KeyAvailable)
                {
                    goto endInforming;
                }
            }
            Console.SetCursorPosition(90, 29);
            Console.Write("Press any key to continue...");
            keyboard.Stop();
            Console.ReadKey();
        endInforming:
            if (music)
            {
                keyboard.Stop();
            }
            Console.Clear();
            isPlaying = true;
        }
        public void MainLoop()    //The main loop in the game
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(50, 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("28");
            if (music)
            {
                soundtrack.Play();
            }

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        if (map[i, j] != ' ' && map[i, j] != '#') { Console.ForegroundColor = ConsoleColor.Yellow; }
                        Console.Write(map[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();

                userLastX = player.x;
                userLastY = player.y;

                if (exclCount == 0)
                {
                    player.Move();
                }
                else if (exclCount > 0)
                {
                    if (count % 2 == 0)
                    {
                        player.Move();
                    }
                }

                userX = player.x;
                userY = player.y;
                bag = player.bag;


                foreach (char m in bag)
                {
                    if (m == '*' && stoneWas == false)
                    {
                        stoneCount++; ;
                    }
                    if (stoneCount == 30)
                    {
                        stoneWas = true;
                        stoneCount = 0;
                    }

                    if (m == '!' && exclWas == false)
                    {
                        exclCount++;
                    }

                    if (m == '%' && shieldWas == false)
                    {
                        shield = true;
                    }

                    if (exclCount == 30)
                    {
                        exclCount = 0;
                        exclWas = true;
                    }

                    if (m == '?')
                    {
                        question = true;
                    }
                }

                if (userX == 29 && userY == 1)
                {
                    if (question)
                    {
                        isPlaying = false;
                        isSecret = true;
                    }
                }

                zombieLastX = zombie.x;
                zombieLastY = zombie.y;

                if (stoneCount == 0)
                {
                    if (shieldNow)
                    {
                        zombie.x -= 8;
                        shieldNow = false;
                        shield = false;
                        shieldWas = true;
                    }
                    else
                    {
                        zombie.Move();
                    }
                }

                zombieX = zombie.x;
                zombieY = zombie.y;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(userY, userX);
                Console.Write('@');
                Console.ResetColor();

                Console.SetCursorPosition(50, 0);
                Console.Write("Bag: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (char h in bag)
                {
                    Console.Write(h + " ");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(zombieY, zombieX);
                Console.Write('Z');
                Console.ResetColor();

                if (userX == zombieX && userY == zombieY && shield == false)
                {
                    isPlaying = false;
                    foreach (char t in bag)
                    {
                        if (t == '&')
                        {
                            blade = true;
                        }
                        if (t == '|')
                        {
                            lever = true;
                        }
                        if (t == '6')
                        {
                            rope = true;
                        }
                    }
                    if (blade && lever && rope)
                    {
                        isWin = true;
                    }
                }
                else if (userX == zombieX && userY == zombieY && shield == true)
                {
                    shieldNow = true;
                }
                count++;
            }

            if (music)
            {
                soundtrack.Stop();
            }
            Console.Clear();
            Console.CursorVisible = true;
            if (isWin)
            {
                if (music)
                {
                    win.Play();
                }
                Thread.Sleep(1400);
                if (music)
                {
                    keyboard.Play();
                }
                string winmsg1 = "You were able to find all the ax parts and defeat the zombie boss!";
                foreach (char u in winmsg1)
                {
                    Console.Write(u);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string winmsg2 = "Congratulations!";
                foreach (char k in winmsg2)
                {
                    Console.Write(k);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string winmsg3 = "Press any key to exit the game...";
                foreach (char i in winmsg3)
                {
                    Console.Write(i);
                    Thread.Sleep(75);
                }
                if (music)
                {
                    keyboard.Stop();
                }
                Console.ReadKey();
            }
            else if (isWin == false && isSecret == false)
            {
                if (music)
                { 
                    lose.Play();
                }
                Thread.Sleep(3100);
                if (music)
                {
                    keyboard.Play();
                }
                string losemsg1 = "The zombie boss caught you and ate you for lunch :(";
                foreach (char u in losemsg1)
                {
                    Console.Write(u);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string losemsg2 = "My condolences!";
                foreach (char k in losemsg2)
                {
                    Console.Write(k);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string losemsg3 = "Press any key to exit the game...";
                foreach (char i in losemsg3)
                {
                    Console.Write(i);
                    Thread.Sleep(75);
                }
                if (music) 
                { 
                    keyboard.Stop();
                }
                Console.ReadKey();
            }
            else if (isSecret == true && isWin == false)
            {
                if (music)
                {
                    powerDown.Play();
                }
                Thread.Sleep(3000);
                if (music)
                { 
                    hello.Play();
                }
                Console.SetCursorPosition(40, 10);
                foreach (char b in "WELCOME TO THE UNIX OPERATING SYSTEM!")
                {
                    Console.Write(b);
                    Thread.Sleep(135);
                }
                Console.SetCursorPosition(40, 11);
                foreach (char b in "Enter user name: ")
                {
                    Console.Write(b);
                    Thread.Sleep(100);
                }
                Console.ReadLine();
                if (music)
                {
                    helloError.Play();
                }
                Console.SetCursorPosition(40, 10);
                Console.Write("HyLErOG ET L5T PIFQ mGEFSeARo EYiKJH");
                Thread.Sleep(300);
                Console.SetCursorPosition(40, 11);
                Console.Write("eNYiq UStr nmyD; fkgGFhijoOOfgj");
                Thread.Sleep(300);
                Console.SetCursorPosition(40, 10);
                Console.Write("uyTErOG zG Krl yqGs m4tyFSARo rtiKyH");
                Thread.Sleep(300);
                Console.SetCursorPosition(40, 11);
                Console.Write("WtYrq iOptq rty~$ VijoFOPFOdhv");
                Thread.Sleep(50);
                Console.Clear();
                Thread.Sleep(1000);
                if (music)
                {
                    helloError2.Play();
                }
                Thread.Sleep(2500);
                if (music)
                {
                    helpMe.Play();
                }
                Thread.Sleep(9000);
                if (music)
                {
                    dontCaught.Play();
                }
                Thread.Sleep(19000);
                if (music)
                {
                    happenBad.Play();
                }
                Thread.Sleep(20000);

            }
        }
    }
    internal class Player    //Player class
    {
        public int x, y;
        public char[] bag;
        char[,] map;
        public Player(int x, int y, char[] bag, char[,] map)
        {
            this.x = x;
            this.y = y;
            this.bag = bag;
            this.map = map;
        }

        public void Move()  //The only player function responsible for movement
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userKey = Console.ReadKey();
                switch (userKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (map[x - 1, y] != '#')
                        {
                            x--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (map[x + 1, y] != '#')
                        {
                            x++;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (map[x, y - 1] != '#')
                        {
                            y--;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (map[x, y + 1] != '#')
                        {
                            y++;
                        }
                        break;

                    case ConsoleKey.W:
                        if (map[x - 1, y] != '#')
                        {
                            x--;
                        }
                        break;

                    case ConsoleKey.S:
                        if (map[x + 1, y] != '#')
                        {
                            x++;
                        }
                        break;

                    case ConsoleKey.A:
                        if (map[x, y - 1] != '#')
                        {
                            y--;
                        }
                        break;

                    case ConsoleKey.D:
                        if (map[x, y + 1] != '#')
                        {
                            y++;
                        }
                        break;
                }
            }

            if (map[x, y] != ' ')
            {
                char[] tempBag = new char[bag.Length + 1];
                for (int i = 0; i < bag.Length; i++)
                {
                    tempBag[i] = bag[i];
                }
                tempBag[tempBag.Length - 1] = map[x, y];
                bag = tempBag;
                map[x, y] = ' ';
            }
        }
    }
    internal class Zombie   //Zombie class
    {
        public int x, y;
        int userX, userY;
        Random rand = new Random();
        int a, b;
        Player player;
        public Zombie(int x, int y, Player player)
        {
            this.x = x;
            this.y = y;
            this.player = player;
        }

        public void Move()    //The only zombie function responsible for the algorithm for its movement 
        {
            userX = player.x;
            userY = player.y;

            a = y - userY;
            b = x - userX;

            if (Math.Abs(a) > Math.Abs(b))
            {
                if (a > 0)
                {
                    y -= 1;
                }

                else if (a < 0)
                {
                    y += 1;
                }
            }

            else if (Math.Abs(b) > Math.Abs(a))
            {
                if (b > 0)
                {
                    x -= 1;
                }

                else if (b < 0)
                {
                    x += 1;
                }
            }

            else if (Math.Abs(a) == Math.Abs(b))
            {
                if (a > 0)
                {
                    if (rand.Next(0, 1) == 0)
                    {
                        y -= 1;
                    }

                    else
                    {
                        x += 1;
                    }
                }

                else if (a < 0)
                {
                    if (rand.Next(0, 1) == 0)
                    {
                        y += 1;
                    }

                    else
                    {
                        x -= 1;
                    }
                }
            }
        }
    }

    internal class Program
    {
        static void Main()    //Game objects are created in the Main function
        {
            char[] bag = { 'X', 'X', 'X', 'X', 'X' };
            bool music = false;

            SoundPlayer soundtrack = null;
            SoundPlayer win = null;
            SoundPlayer lose = null;
            SoundPlayer keyboard = null;
            SoundPlayer error = null;
            SoundPlayer helpMorze = null;
            SoundPlayer caughtMorze = null;
            SoundPlayer happenMorze = null;
            SoundPlayer hello = null;
            SoundPlayer helloError = null;
            SoundPlayer helloError2 = null;
            SoundPlayer powerDown = null;

            if (music)
            {
                soundtrack = new SoundPlayer(@"<enter file address here>\soundtrack.wav");
                win = new SoundPlayer(@"<enter file address here>\win.wav");
                lose = new SoundPlayer(@"<enter file address here>\lose.wav");
                keyboard = new SoundPlayer(@"<enter file address here>\keyboard.wav");
                error = new SoundPlayer(@"<enter file address here>\error.wav");
                helpMorze = new SoundPlayer(@"<enter file address here>\help_me.wav");
                caughtMorze = new SoundPlayer(@"<enter file address here>\dont_get_caught.wav");
                happenMorze = new SoundPlayer(@"<enter file address here>\happen.wav");
                hello = new SoundPlayer(@"<enter file address here>\hello.wav");
                helloError = new SoundPlayer(@"<enter file address here>\error_hello.wav");
                helloError2 = new SoundPlayer(@"<enter file address here>\error_hello2.wav");
                powerDown = new SoundPlayer(@"<enter file address here>\power_down.wav");
            }

            char[,] map =
            {
                    {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','6',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','*',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','&',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','%',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','!',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ','?',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                    {'#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#','#'}
            };
            try
            {
                Player player = new Player(6, 6, bag, map);
                Zombie zombie = new Zombie(15, 15, player);

                GameMainObject game = new GameMainObject
                   (
                   map, player, zombie, soundtrack, win, lose, keyboard,
                   powerDown, hello, helloError, helloError2, helpMorze,
                   caughtMorze, happenMorze, music
                   );

                game.Informing();
                game.MainLoop();
            }
            catch (IndexOutOfRangeException)
            {
                Console.Clear();
                if (music)
                {
                    error.Play();
                } 
                Thread.Sleep(1200);
                if (music)
                {
                    keyboard.Play();
                }
                string errormsg1 = "Error! Cause: The index was outside the bounds of the array.";
                foreach (char u in errormsg1)
                {
                    Console.Write(u);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string errormsg2 = "Further execution of the program is impossible!";
                foreach (char k in errormsg2)
                {
                    Console.Write(k);
                    Thread.Sleep(75);
                }
                Console.WriteLine();
                string errormsg3 = "Press any key to exit the game...";
                foreach (char i in errormsg3)
                {
                    Console.Write(i);
                    Thread.Sleep(75);
                }
                if (music)
                {
                    keyboard.Stop();
                }
                Console.ReadKey();
            }

        }
    }
}
