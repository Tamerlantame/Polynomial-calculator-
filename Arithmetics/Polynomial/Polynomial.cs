﻿// verified by ususucsus. gadost' detected and destructed

using System;
using System.Collections;
using System.Collections.Generic;
using Arithmetics.Parsers;

namespace Arithmetics.Polynomial1
{
    public class Polynomial : IComparable<Polynomial>, IEnumerable<int>
    {
        //Поля
        private readonly SortedList<int, double> coeff;

        public int Deg { get; }
        //Индексатор
        public double this[int index]
        {
            get
            {
                if (coeff.ContainsKey(index))
                {
                    return coeff[index];
                }
                else return 0;
            }
        }

        /// <summary>
        /// If <paramref name="poly"/> is a correct representaion of a polynomial (i.e. a sum of monomials of the form either a, ax or ax^b for some integers a and b.), 
        /// constructs the corresponding polynomial. Otherwise the polynomial is initialized as 0. 
        /// </summary>
        /// <param name="poly">string representation of a polynomial</param>
        [Obsolete("Use Polynomial(SortedList<int, double> coeff) instead")]
        public Polynomial(string poly)
        {
            try
            {
                coeff = PolynomialParser.Parse(poly);
            }
            catch (InvalidPolynomialStringException)
            {
                coeff.Add(0, 0);
            }
            Deg = coeff.Keys[coeff.Keys.Count - 1];
        }
        //Конструкторы
        public Polynomial()
        {
            Deg = 0;
            coeff = new SortedList<int, double>();
        }

        public Polynomial(int Deg)
        {
            this.Deg = Deg;
            coeff = new SortedList<int, double>();
            
        }

        public Polynomial(SortedList<int, double> coeff)
        {

            Deg = coeff.Keys[coeff.Count - 1];
            this.coeff = new SortedList<int, double>();
            foreach (int deg in coeff.Keys)
            {
                this.coeff.Add(deg, coeff[deg]);
            }


        }

        public Polynomial(Polynomial polynimial)
        {
            Deg = polynimial.Deg;
            coeff = new SortedList<int, double>();
            foreach (int deg in polynimial.coeff.Keys)
            {
                this.coeff.Add(deg, polynimial.coeff[deg]);
            }

        }

        /// <summary>
        /// Пока что совсем не аккуратно.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";

            for (int deg = this.Deg;deg>=0;deg--)
            {
                if (this.coeff.ContainsKey(deg))
                {
                    if (coeff[deg] >= 0)
                    {
                        switch (deg)
                        {
                            case 0:
                                if (coeff[deg] != 0)
                                {
                                    if (coeff.Count != 1)
                                        s += "+" + coeff[deg];
                                    else
                                        s += coeff[deg];
                                }
                                break;
                            case 1:
                                    if (Deg >= 2)
                                    {
                                        if (coeff[deg] == 1)
                                            s += "+x" ;
                                        else
                                            s += "+" + coeff[deg] + "x";
                                    }
                                    else
                                    {
                                        if (coeff[deg] == 1)
                                            s += "x";
                                        else
                                            s += coeff[deg] + "x";
                                    }
                                break;
                            default:
                                if (coeff[deg] != 0)
                                {
                                    if (deg != this.coeff.Keys[this.coeff.Keys.Count - 1])
                                    {
                                        if (coeff[deg] == 1)
                                            s += "+x^" + deg;
                                        else
                                            s += "+" + coeff[deg] + "x^" + deg;
                                    }
                                    else
                                    {
                                        if (coeff[deg] == 1)
                                            s += "x^" + deg;
                                        else
                                            s += coeff[deg] + "x^" + deg;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (deg)
                        {
                            case 0:
                                s += coeff[deg];
                                break;
                            case 1:
                                if (coeff[deg] == -1)
                                    s += "-x";
                                else
                                    s += coeff[deg] + "x";

                                break;
                            default:
                                if (coeff[deg] == -1)
                                    s += "-x^" + deg;
                                else
                                    s += +coeff[deg] + "x^" + deg;

                                break;
                        }
                    }
                }
            }
            if (s == "")
                return "0";
            else
                return s;
        }

        //Операторы
        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            SortedList<int, double> coeff = new SortedList<int, double>(p1.coeff);
            foreach (int deg in p2)
            {
                if (coeff.ContainsKey(deg))
                    coeff[deg] += p2[deg];
                else
                    coeff.Add(deg, p2[deg]);
            }
            return new Polynomial(coeff);
        }

        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            SortedList<int, double> coeff = new SortedList<int, double>(p1.coeff);
            foreach (int deg in p2)
            {
                if (coeff.ContainsKey(deg))
                    coeff[deg] -= p2[deg];
                else
                    coeff.Add(deg, p2[deg]);
            }
            return new Polynomial(coeff);
        }

        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();
            foreach (int deg1 in p1)
            {
                foreach (int deg2 in p2)
                {
                    if (!coeff.ContainsKey(deg1 + deg2))
                    {
                        coeff.Add(deg1 + deg2, p1[deg1] * p2[deg2]);
                    }
                    else
                    {
                        coeff[deg1 + deg2] += p1[deg1] * p2[deg2];
                    }
                }
            }
            return new Polynomial(coeff);
        }

        public static Polynomial operator *(Polynomial p1, double number)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();
            foreach(int deg in p1.coeff.Keys)
            {
                coeff.Add(deg, p1.coeff[deg] * number);
            }
            return new Polynomial(coeff);
        }

        public static Polynomial operator ^(Polynomial p1, int deg)
        {
            if (deg == 0)
                return 1;
            Polynomial result = new Polynomial(p1);
            for(int i = 1; i <deg; i++)
            {
               result =  new Polynomial( result * p1);
            }
            return result;
        }

        public static Polynomial operator *(double number, Polynomial p1)
        {
            return p1 * number;
        }
        public static Polynomial operator /(Polynomial p1, double number)
        {
            if (number == 0)
                throw new DivideByZeroException();
            SortedList<int, double> coeff = new SortedList<int, double>();
            foreach (int deg in p1.coeff.Keys)
            {
                coeff.Add(deg, p1.coeff[deg] / number);
            }
            return new Polynomial(coeff);
        }
        public static bool operator ==(Polynomial p1, Polynomial p2)
        {
            foreach (int deg in p1.coeff.Keys)
            {
                if (p1[deg] != p2[deg])
                    return false;
            }
            foreach (int deg in p2.coeff.Keys)
            {
                if (p2[deg] != p1[deg])
                    return false;
            }
            return true;
        }

        public static bool operator !=(Polynomial p1, Polynomial p2)
        {
            return !(p1==p2);
        }
        public override bool Equals(object obj)
        {
            return obj is Polynomial polynomial &&
                   EqualityComparer<SortedList<int, double>>.Default.Equals(coeff, polynomial.coeff) &&
                   Deg == polynomial.Deg;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //Методы
        /// <summary>
        /// Возвращает d/dx
        /// </summary>
        /// <param name="polynomial"></param>
        /// <returns></returns>
        public static Polynomial Diff(Polynomial polynomial)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();

            for(int i = polynomial.Deg; i>0; i--)
            {
                coeff.Add(i-1, polynomial[i]*i);
            }

            return new Polynomial(coeff);
        }
        /// <summary>
        ///  подставляет polynomial вместо x и получает полином
        /// </summary>
        /// <param name="polynomial"></param>
        /// <returns></returns>
        public Polynomial Eval(Polynomial polynomial)
        {
            string poly = ""; 
            foreach(int deg in this.coeff.Keys)
            {
                poly = poly + "+" + (this[deg]*(polynomial^deg)).ToString(); 
            }


            return new Polynomial(PolynomialParser.Parse(poly));
        }
        //TODO подумать как реализовать CompareTo более детально
        public int CompareTo(Polynomial a)
        {
            if (this.Deg != a.Deg)
                return this.Deg.CompareTo(a.Deg);
            else
                for (int i = Deg; i > 0; i++)
                {
                    try
                    {
                        if (this.coeff[i] != a.coeff[i])
                            return this[i].CompareTo(a[i]);
                    }
                    catch (KeyNotFoundException)
                    {
                        if (this.coeff.ContainsKey(i))
                            return 1;
                        else
                            return -1;
                    }
                }
            return 0;
        }

        public static implicit operator Polynomial(double number)
        {
            SortedList<int, double> coeff = new SortedList<int, double>();
            coeff.Add(0, number);
            return new Polynomial(coeff);
        }

        public IEnumerator<int> GetEnumerator()
        {

            return coeff.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return coeff.Keys.GetEnumerator();
        }
    }
}