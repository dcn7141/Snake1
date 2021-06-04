using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowHeight = 32;
            WindowWidth = 64;

            var rand = new Random();

            var score = 5;

            var screen = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            var screen2 = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);

            var body = new List<Pixel>();

            var movement = Direction.Right;

            var gameover = false;

            while (true)
            {
                Clear();

                gameover |= (screen.XPos == WindowWidth - 1 || screen.XPos == 0 || screen.YPos == WindowHeight - 1 || screen.YPos == 0);

                DrawBorder();

                if (screen2.XPos == screen.XPos && screen2.YPos == screen.YPos)
                {
                    score++;
                    screen2 = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
                }

                for (int i = 0; i < body.Count; i++)
                {
                    DrawPixel(body[i]);
                    gameover |= (body[i].XPos == screen.XPos && body[i].YPos == screen.YPos);
                }

                if (gameover)
                {
                    break;
                }

                DrawPixel(screen);
                DrawPixel(screen2);

                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds <= 250)
                {
                    movement = ReadMovement(movement);
                }

                body.Add(new Pixel(screen.XPos, screen.YPos, ConsoleColor.Green));

                switch (movement)
                {
                    case Direction.Up:
                        screen.YPos--;
                        break;
                    case Direction.Down:
                        screen.YPos++;
                        break;
                    case Direction.Left:
                        screen.XPos--;
                        break;
                    case Direction.Right:
                        screen.XPos++;
                        break;
                }

                if (body.Count > score)
                {
                    body.RemoveAt(0);
                }
            }
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            Console.WriteLine($"Game over, Score: {score - 5}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            Console.ReadKey();
        }

        static Direction ReadMovement(Direction movement)
        {
            if (KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && movement != Direction.Down)
                {
                    movement = Direction.Up;
                }
                else if (key == ConsoleKey.DownArrow && movement != Direction.Up)
                {
                    movement = Direction.Down;
                }
                else if (key == ConsoleKey.LeftArrow && movement != Direction.Right)
                {
                    movement = Direction.Left;
                }
                else if (key == ConsoleKey.RightArrow && movement != Direction.Left)
                {
                    movement = Direction.Right;
                }
            }

            return movement;
        }

        static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Console.Write("@");
            SetCursorPosition(0, 0);
        }

        static void DrawBorder()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Console.Write("--");

                SetCursorPosition(i, WindowHeight - 1);
                Console.Write("--");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Console.Write("|");

                SetCursorPosition(WindowWidth - 1, i);
                Console.Write("|");
            }
        }

        struct Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }

        enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}