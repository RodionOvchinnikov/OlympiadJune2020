using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class BFSBehavior : IBehavior
    {
        private List<Point> wave = new List<Point>();
        private int wall = 99;

        private bool FindPath(Point headPosition, Point fruitPosition, int[,] map, int Width, int Height)
        {
            int x = headPosition.X;
            int y = headPosition.Y;
            int nx = fruitPosition.X;
            int ny = fruitPosition.Y;

            if (map[x, y] == wall || map[nx, ny] == wall)
            {
                return false;
            }

            //волновой алгоритм поиска пути (заполнение значений достижимости) начиная от конца пути
            int[,] cloneMap = (int[,])map.Clone();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            int nstep = 0;
            map[nx, ny] = nstep;

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

            while (oldWave.Count > 0)
            {
                ++nstep;
                wave.Clear();
                foreach (Point i in oldWave)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        nx = i.X + dx[d];
                        ny = i.Y + dy[d];

                        if (ny >= 0 && ny < Height && nx >= 0 && nx < Width)
                        {
                            if (map[nx, ny] == -1)
                            {
                                wave.Add(new Point(nx, ny));
                                map[nx, ny] = nstep;
                            }
                        }
                    }
                }
                oldWave = new List<Point>(wave);

            }

            //волновйо алгоритм поиска пути начиная от начала
            bool flag = true;
            wave.Clear();
            wave.Add(new Point(x, y));
            while (map[x, y] != 0)
            {
                flag = true;
                for (int d = 0; d < 4; d++)
                {
                    nx = x + dx[d];
                    ny = y + dy[d];

                    if (ny >= 0 && ny < Height && nx >= 0 && nx < Width)
                    {
                        if (map[x, y] - 1 == map[nx, ny])
                        {
                            x = nx;
                            y = ny;
                            wave.Add(new Point(x, y));
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    break;
                }
            }

            map = cloneMap;

            wave.ForEach(delegate (Point i)
            {
                map[i.X, i.Y] = 0;
            });

            return true;
        }

        public MoveDirection Move(MoveState state)
        {
            Snake snake = state.Snakes.Single(x => x.Id == state.You);

            int[,] map = new int[state.Width, state.Height];

            for (int i = 0; i < state.Width; ++i)
            {
                for(int j = 0; j < state.Height; ++j)
                {
                    map[i, j] = -1;
                }
            }

            foreach (var fruit in state.Food)
            {
                
                
                FindPath(snake.HeadPosition, fruit, map, state.Width, state.Height);
            }

            // Берем вторую ячейку пути, т.к. первая - это ячейка из которой мы начинаем движение
            var nextCell = wave[1];

            int translationX = snake.HeadPosition.X - nextCell.X;
            int translationY = snake.HeadPosition.Y - nextCell.Y;

            if (translationX == 0)
            {
                if(translationY > 0)
                {
                    return new MoveDirection { Move = "up", Taunt = "Moving up" };
                } 
                else
                {
                    return new MoveDirection { Move = "down", Taunt = "Moving down" };
                }
            }

            if(translationX > 0)
            {
                return new MoveDirection { Move = "left", Taunt = "Moving left" };
            }
            else
            {
                return new MoveDirection { Move = "right", Taunt = "Moving right" };
            }
        }
    }
}
