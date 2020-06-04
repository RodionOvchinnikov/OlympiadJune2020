using BehavioralAlgorithms.Interfaces;
using BehavioralAlgorithms.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BehavioralAlgorithms.Behaviors
{
    public class BFSBehavior : IBehavior
    {
        private readonly int wall = int.MaxValue;

        private List<Point> FindPath(Point headPosition, Point fruitPosition, int[,] map, int Width, int Height)
        {
            List<Point> wave = new List<Point>();
            int x = headPosition.X;
            int y = headPosition.Y;
            int nx = fruitPosition.X;
            int ny = fruitPosition.Y;

            //заполнение значений достижимости(длинны пути) для конечной точки
            int[,] cloneMap = (int[,])map.Clone();
            List<Point> oldWave = new List<Point>();
            oldWave.Add(new Point(nx, ny));
            int nstep = 0;
            cloneMap[nx, ny] = nstep;

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
                            if (cloneMap[nx, ny] == -1)
                            {
                                wave.Add(new Point(nx, ny));
                                cloneMap[nx, ny] = nstep;
                            }
                        }
                    }
                }
                oldWave = new List<Point>(wave);

            }

            wave.Clear();
            //wave.Add(new Point(x, y));
            while (cloneMap[x, y] != 0)
            {
                //поиск пути от точки отправления
                bool flag = true;
                for (int d = 0; d < 4; d++)
                {
                    nx = x + dx[d];
                    ny = y + dy[d];

                    if (ny >= 0 && ny < Height && nx >= 0 && nx < Width)
                    {
                        if (cloneMap[x, y] - 1 == cloneMap[nx, ny])
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

            return wave;
        }

        public MoveDirection Move(MoveState state)
        {
            Snake mainSnake = state.Snakes.Single(x => x.Id == state.You);

            int[,] map = new int[state.Width, state.Height];

            for (int i = 0; i < state.Width; ++i)
            {
                for(int j = 0; j < state.Height; ++j)
                {
                    map[i, j] = -1;
                }
            }

            foreach (var snake in state.Snakes)
            {
                foreach (var cell in snake.Coords)
                {
                    map[cell.X, cell.Y] = wall;
                }
            }

            map[mainSnake.HeadPosition.X, mainSnake.HeadPosition.Y] = -1;

            List<List<Point>> paths = new List<List<Point>>();
            foreach (var fruit in state.Food)
            {
                var path = FindPath(mainSnake.HeadPosition, fruit, map, state.Width, state.Height);
                paths.Add(path);
            }

            var pathToNearestFruit = paths.OrderBy(x => x.Count).FirstOrDefault(x => x.Count != 0);

            // Если нашли путь до фрукта
            if(pathToNearestFruit != null)
            {
                // Берем первую ячейку пути, в которую нам и надо шагнуть
                var nextCell = pathToNearestFruit[0];

                int translationX = mainSnake.HeadPosition.X - nextCell.X;
                int translationY = mainSnake.HeadPosition.Y - nextCell.Y;

                if (translationX == 0)
                {
                    if (translationY > 0)
                    {
                        return new MoveDirection { Move = "up", Taunt = "Moving up" };
                    }
                    else
                    {
                        return new MoveDirection { Move = "down", Taunt = "Moving down" };
                    }
                }

                if (translationX > 0)
                {
                    return new MoveDirection { Move = "left", Taunt = "Moving left" };
                }
                else
                {
                    return new MoveDirection { Move = "right", Taunt = "Moving right" };
                }
            }
            // Если не смогли найти путь до фрукта. TODO: реализовать движение к самой дальней точке от головы змеи с помощью гамильтоновых циклов
            else
            {
                return new MoveDirection { Move = "right", Taunt = "Moving right" };
            }
        }
    }
}
