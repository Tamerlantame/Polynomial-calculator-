﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetics
{
    static class SternBrocotFunctions
    {
        // Метод возвращает последовательнсоть Фаррея
        public static List<Fraction> FindFarrey(int n)
        {

            List<Fraction> fractions = new List<Fraction>();
            BinaryTree<BrocotFraction> a = GetSubTree(n).left;
            fractions.Add(new Fraction(0, 1));
            fractions.AddRange(GetListElements(a, n));
            fractions.Add(new Fraction(1, 0));
            for (int i = 0; i < fractions.Count(); i++)
            {
                if (fractions[i].p >= n || fractions[i].q >= n)
                {
                    fractions.Remove(fractions[i]);
                }
            }
            return fractions;
        }
        // Рекурсивное построение поддерева заданной глубины(или высоты)
        public static BinaryTree<BrocotFraction> GetSubTree(int depth)
        {

            if (depth <= 0)
                return null;
            else
            {
                Fraction t = new Fraction(1, 1);
                Fraction left = new Fraction(0, 1);
                Fraction right = new Fraction(1, 0);

                BrocotFraction FirstStage = new BrocotFraction(t, left, right);
                BinaryTree<BrocotFraction> b = new BinaryTree<BrocotFraction>(FirstStage);
                if (depth == 1)
                    return b;
                b.value = FirstStage;
                b.right = GetSubTree(new BrocotFraction(Fraction.Mediant(b.value.current, b.value.right), b.value.current, b.value.right), depth - 1);
                b.left = GetSubTree(new BrocotFraction(Fraction.Mediant(b.value.current, b.value.left), b.value.left, b.value.current), depth - 1);

                return b;
            }
        }
        // метод возвращает последовательность Дробей
        public static List<Fraction> GetListElements(BinaryTree<BrocotFraction> binarytree, int q)
        {
            List<Fraction> fractions = new List<Fraction>();

            if (binarytree.left != null || binarytree.right != null)
            {

                Fraction c = binarytree.value.current;
                fractions.AddRange(GetListElements(binarytree.left, q));
                if (c.q <= q)
                {
                    fractions.Add(c);
                }
                fractions.AddRange(GetListElements(binarytree.right, q));
            }
            return fractions;
        }
        // Рекурсивное построение поддерева заданной глубины(или высоты), вызвается в методе GetSubThree(int depth)
        public static BinaryTree<BrocotFraction> GetSubTree(BrocotFraction f, int depth)
        {
            if (depth > 0)
            {
                Fraction x = Fraction.Mediant(f.left, f.current);
                Fraction y = Fraction.Mediant(f.right, f.current);

                BrocotFraction leftSub = new BrocotFraction(x, f.left, f.current);
                BrocotFraction rightSub = new BrocotFraction(y, f.current, f.right);

                BinaryTree<BrocotFraction> leftSubTree = GetSubTree(leftSub, depth - 1);
                BinaryTree<BrocotFraction> rightSubTree = GetSubTree(rightSub, depth - 1);
                BinaryTree<BrocotFraction> tree = new BinaryTree<BrocotFraction>(f, leftSubTree, rightSubTree);

                return tree;
            }
            else

                return null;
        }
    }
}
