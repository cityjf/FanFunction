using System;
using System.Collections.Generic;
using System.Text;

namespace FanFunction
{
    public class clsSuanFa
    {
        /// <summary>
        /// 冒泡排序，升序
        /// </summary>
        /// <param name="num">数组</param>
        public static void 冒泡(int[] num)
        {
            int temp;
            for (int i = 0; i < num.Length - 1; i++)
            {
                for (int j = 0; j < num.Length - 1 - i; j++)
                {
                    if (num[j] > num[j + 1])//大于号[>]是升序,小于号[<]是降序
                    {
                        temp = num[j];
                        num[j] = num[j + 1];
                        num[j + 1] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 要求：有一列数1，1，2，3，5，8，。。。。。。求第30个数（递归算法）结果是：1346269
        /// </summary>
        public static int 规律(int num)
        {
            if (num == 0 || num == 1)
            {
                return 1;
            }
            return 规律(num - 1) + 规律(num - 2);
        }
        /// <summary>
        /// 要求：将一整数逆序后放入一数组（要求用递归实现）
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="output">输出的数组</param>
        /// <returns></returns>
        public static List<int> 逆序(int input, List<int> output)
        {
            if (input >= 10)
            {
                output.Add(input % 10);
                return 逆序(input / 10, output);
            }
            if (input < 10)
                output.Add(input % 10);
            return output;
        }
        /// <summary>
        /// 编码完成下面的处理函数。函数将字符串中的字符'*'移到串的前部分
        /// 前面的非'*'字符后移但不能改变非'*'字符的先后顺序函数返回串中字符'*'的数量
        /// 如原始串为ab**cd**e*12处理后为*****abcde12函数并返回值为5
        /// 
        /// string str = "ab**cd**e*12";
        /// char[] arrChar = new char[str.Length];
        /// int i1111 = clsSuanFa.移动字符(str, out arrChar);
        /// </summary>
        /// <param name="str">如：ab**cd**e*12</param>
        /// <param name="output">用于返回</param>
        /// <returns>*的个数</returns>
        public static int 移动字符(string str,out char[] output)
        {
            int i, j = str.Length - 1; 
            char[] temp = str.ToCharArray();
            for (i = j; j >= 0; j--)
            {
                if (temp[i] != '*')
                    i--;
                else if (temp[j] != '*')
                {
                    temp[i] = temp[j];
                    temp[j] = '*';
                    i--;
                }
            }
            output = temp;
            return i + 1;
        }
        /// <summary>
        /// 随机分配座位，共50个学生，使学号相邻的同学座位不能相邻
        /// </summary>
        /// <returns></returns>
        public static int[] 随机分配座位()
        {
            int temp = 0;
            int[] iSeats = new int[50];
            bool[] Students = new bool[50];
            Random RStu = new Random();
            Students[iSeats[0] = RStu.Next(0, 50)] = true;
            for (int i = 1; i < 50; )
            {
                temp = (int)RStu.Next(0, 50);
                if ((!Students[temp]) && (iSeats[i - 1] - temp != 1) && (iSeats[i - 1] - temp) != -1)
                {
                    iSeats[i++] = temp;
                    Students[temp] = true;
                }
            }
            return iSeats;
        }
    }
}
