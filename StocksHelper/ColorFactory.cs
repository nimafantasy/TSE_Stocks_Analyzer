using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksHelper
{
    public static class ColorFactory
    {
        public static Color GetStrengthCodedColor(int strength, int maxstrength, Color color)
        {
            double alpha = (255 * strength) / maxstrength;
            return Color.FromArgb(Convert.ToInt32(alpha), color.R, color.G, color.B);
        }

        public static Color GetTransparentColor(int percentage, Color color)
        {
            double alpha = (255 * percentage) / 100;
            return Color.FromArgb(Convert.ToInt32(alpha), color.R, color.G, color.B);
        }


        public static Color GetComparingColor(double buy_req, double sell_req, Color previous_color)
        {
            double diff = Math.Abs(buy_req - sell_req);
            if(diff < buy_req && diff < sell_req)
            {
                // not much difference
                return previous_color;
            }
            else
            {
                double pct = 0;
                if (buy_req >= diff)
                {
                    // grreen max => 0
                    pct = (diff / buy_req);
                    pct = 1 - pct;
                    if (pct == 0)
                        pct = 0.01;
                }
                else
                {
                    //red max => 1
                    pct = (diff / sell_req);
                    if (pct == 1)
                        pct = 0.99;
                }

                //ColorRGB c = HSL2RGB(pct < 1 ? pct : 0.999999, 0.5, 0.5);
                if (pct < 0.1)
                    return Color.FromArgb(255, 0, 255, 0);
                else if (pct < 0.2)
                    return Color.FromArgb(255, 25, 255, 25);
                else if (pct < 0.3)
                    return Color.FromArgb(255, 50, 255, 50);
                else if (pct < 0.4)
                    return Color.FromArgb(255, 255, 255, 255);
                else if (pct < 0.5)
                    return Color.FromArgb(255, 255, 255, 255);
                else if (pct < 0.6)
                    return Color.FromArgb(255, 255, 255, 255);
                else if (pct < 0.7)
                    return Color.FromArgb(255, 255, 255, 255);
                else if (pct < 0.8)
                    return Color.FromArgb(255, 255, 50, 50);
                else if (pct < 0.9)
                    return Color.FromArgb(255, 255, 25, 25);
                else if (pct < 1)
                    return Color.FromArgb(255, 255, 0, 0);
                else
                    return Color.White;


            }
        }

        // Given a Color (RGB Struct) in range of 0-255
        // Return H,S,L in range of 0-1
        private static void RGB2HSL(ColorRGB rgb, out double h, out double s, out double l)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;
            double v;
            double m;
            double vm;
            double r2, g2, b2;

            h = 0; // default to black
            s = 0;
            l = 0;
            v = Math.Max(r, g);
            v = Math.Max(v, b);
            m = Math.Min(r, g);
            m = Math.Min(m, b);
            l = (m + v) / 2.0;
            if (l <= 0.0)
            {
                return;
            }
            vm = v - m;
            s = vm;
            if (s > 0.0)
            {
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            }
            else
            {
                return;
            }
            r2 = (v - r) / vm;
            g2 = (v - g) / vm;
            b2 = (v - b) / vm;
            if (r == v)
            {
                h = (g == m ? 5.0 + b2 : 1.0 - g2);
            }
            else if (g == v)
            {
                h = (b == m ? 1.0 + r2 : 3.0 - b2);
            }
            else
            {
                h = (r == m ? 3.0 + g2 : 5.0 - r2);
            }
            h /= 6.0;
        }


        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }
    }

    public struct ColorRGB
    {
        public byte R;
        public byte G;
        public byte B;
        public ColorRGB(Color value)
        {
            this.R = value.R;
            this.G = value.G;
            this.B = value.B;
        }
        public static implicit operator Color(ColorRGB rgb)
        {
            Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
            return c;
        }
        public static explicit operator ColorRGB(Color c)
        {
            return new ColorRGB(c);
        }
    }
}
