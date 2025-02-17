﻿using System.Windows.Media;

namespace PZ2.Models
{
    public class PowerEntity
    {
        private long id;
        private string name;
        private double x;
        private double y;
        private Brush color;
        private string toolTip;

        public PowerEntity()
        {

        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Brush Color { get => color; set => color = value; }
        public string ToolTip { get => toolTip; set => toolTip = value; }
    }
}
