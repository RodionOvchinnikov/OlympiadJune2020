using BehavioralAlgorithms.Models.Hamiltonian;
using System;

namespace BehavioralAlgorithms.Behaviors.Hamiltonian
{
    public class Util
    {
        public static string MapHamiltonToDirection(int[] start, ArrayNode node)
        {
            int yStart = start[0];
            int xStart = start[1];

            if (node.Y == yStart)
            {
                if (node.X > xStart)
                {
                    return "right";
                }
                else
                {
                    return "left";
                }
            }
            else
            {
                if (node.Y > yStart)
                {
                    return "down";
                }
                else
                {
                    return "up";
                }
            }

            throw new Exception("Ошибка при определении направления движения");
        }
    }
}
