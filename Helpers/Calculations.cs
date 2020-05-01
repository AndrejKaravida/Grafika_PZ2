using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = System.Windows.Point;

namespace PZ2.Helpers
{
    public class Calculations
    {
        public static void CalculatePoints(LineEntity l, out Point firstEnd, out Point secondEnd, List<PowerEntity> powerEntities, Dictionary<Point, PowerEntity> keyValuePairs)
        {
            PowerEntity first = powerEntities.Find(el => el.Id == l.FirstEnd);
            PowerEntity second = powerEntities.Find(el => el.Id == l.SecondEnd);

            firstEnd = new Point();
            secondEnd = new Point();

            if (first != null && second != null)
            {
                firstEnd = keyValuePairs.Where(x => x.Value == first).First().Key;
                secondEnd = keyValuePairs.Where(x => x.Value == second).First().Key;
            }
        }

        public static Point FindNearestFree(PowerEntity element, double realX, double realY, List<Point> gridPoints, Dictionary<Point, PowerEntity> keyValuePairs)
        {
            Point tacka = new Point();
            int brojac = 1;
            bool flag = false;
            while (true)
            {
                for (double iksevi = realX - brojac * 3; iksevi <= realX + brojac * 3; iksevi += 3)
                {
                    if (iksevi < 0)
                        continue;
                    for (double ipsiloni = realY - brojac * 2; ipsiloni <= realY + brojac * 2; ipsiloni += 2)
                    {
                        if (ipsiloni < 0)
                            continue;
                        tacka = gridPoints.Find(t => t.X == iksevi && t.Y == ipsiloni);
                        if (!keyValuePairs.ContainsKey(tacka))
                        {
                            keyValuePairs.Add(tacka, element);
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        break;
                }
                if (flag)
                    break;

                brojac++;
            }
            return tacka;
        }

        public static void CalculateXY(double newX, double newY, out double realX, out double realY, double lengthX, double lengthY, double minX, double minY)
        {
            double odstojanjeX = 300 / lengthX;
            double odstojanjeY = 300 / lengthY;

            realX = Math.Round((newX - minX) * odstojanjeX) * 3;
            realY = Math.Round((newY - minY) * odstojanjeY) * 2;
        }

    }
}
