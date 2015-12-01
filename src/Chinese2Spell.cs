using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//添加Microsoft.VisualBasic.dll引用
using Microsoft.VisualBasic;

namespace FanFunction
{
    /// <summary>
    /// 中文转拼音
    /// </summary>
    public class Chinese2Spell
    {
        #region 属性数据定义
        /// <summary>
        /// 汉字的机内码数组
        /// </summary>
        private static int[] pyValue = new int[]
        {
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };
        /// <summary>
        /// 机内码对应的拼音数组
        /// </summary>
        private static string[] pyName = new string[]
        {
            "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
            "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
            "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
            "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
            "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
            "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
            "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
            "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
            "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
            "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
            "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
            "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
            "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
            "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
            "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
            "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
            "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
            "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
            "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
            "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
            "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
            "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
            "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
            "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
            "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
            "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
            "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
            "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
            "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
            "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
            "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
            "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
            "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };
        #endregion
        #region 把汉字转换成拼音(全拼)无间隔符号
        /// <summary>
        /// 把汉字转换成拼音(全拼) 例如：张三转换成了ZhangSan
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string Convert(string hzString)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString += noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString += "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString += pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString += noWChar[j].ToString();
                }
            }
            return pyString;
        }
        #endregion
        #region 把汉字转换成拼音(全拼) 用空格间隔
        /// <summary>
        /// 把汉字转换成拼音(全拼) 例如：张三转换成了Zhang San
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string ConvertWithBlank(string hzString)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString = pyString + " " + noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString = pyString + " " + "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString = pyString + " " + pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString = pyString + " " + noWChar[j].ToString();
                }
            }
            return pyString.Trim();
        }
        #endregion
        #region 把汉字转换成拼音(全拼) 用特定的字符间隔
        /// <summary>
        /// 把汉字转换成拼音(全拼) 例如第二个参数是下划线_则：张三转换成了Zhang_San
        /// </summary>
        /// <param name="hzString">汉字字符串</param>
        /// <param name="splitChar">用于分隔的字符串</param>
        /// <returns>转换后的拼音(全拼)字符串</returns>
        public static string ConvertWithSplitChar(string hzString, string splitChar)
        {
            // 匹配中文字符
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            byte[] array = new byte[2];
            string pyString = "";
            int chrAsc = 0;
            int i1 = 0;
            int i2 = 0;
            char[] noWChar = hzString.ToCharArray();
            for (int j = 0; j < noWChar.Length; j++)
            {
                // 中文字符
                if (regex.IsMatch(noWChar[j].ToString()))
                {
                    array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);
                    chrAsc = i1 * 256 + i2 - 65536;
                    if (chrAsc > 0 && chrAsc < 160)
                    {
                        pyString = pyString + splitChar + noWChar[j];
                    }
                    else
                    {
                        // 修正部分文字
                        if (chrAsc == -9254)  // 修正“圳”字
                            pyString = pyString + splitChar + "Zhen";
                        else
                        {
                            for (int i = (pyValue.Length - 1); i >= 0; i--)
                            {
                                if (pyValue[i] <= chrAsc)
                                {
                                    pyString = pyString + splitChar + pyName[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                // 非中文字符
                else
                {
                    pyString = pyString + splitChar + noWChar[j].ToString();
                }
            }
            char[] trimAChar = splitChar.ToCharArray();
            return pyString.TrimStart(trimAChar);
        }
        #endregion
        #region 汉字转拼音缩写 (字符串) (小写)
        /// <summary>
        /// 汉字转拼音缩写 例如：张三转换成了zs
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringLower(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr += GetSpellCharLower(c.ToString());
                }
            }
            return tempStr;
        }
        #endregion
        #region 汉字转拼音缩写 (字符串) (小写) (空格间隔)
        /// <summary>
        /// 汉字转拼音缩写 (字符串) (小写) (空格间隔) 例如：张三转换成了z s
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringLowerSplitWithBlank(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr = tempStr + " " + c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr = tempStr + " " + GetSpellCharLower(c.ToString());
                }
            }
            return tempStr.Trim();
        }
        #endregion
        #region 汉字转拼音缩写 (字符串)(大写)
        /// <summary>
        /// 汉字转拼音缩写 (大写) 例如：张三转换成了ZS
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringUpper(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr += GetSpellCharUpper(c.ToString());
                }
            }
            return tempStr;
        }
        #endregion
        #region 汉字转拼音缩写 (字符串)(大写)(空格间隔)
        /// <summary>
        /// 汉字转拼音缩写  (字符串)(大写)(空格间隔) 例如：张三转换成了Z S
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetSpellStringUpperSplitWithBlank(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留
                    tempStr = tempStr + " " + c.ToString();
                }
                else
                {
                    //累加拼音声母
                    tempStr = tempStr + " " + GetSpellCharUpper(c.ToString());
                }
            }
            return tempStr.Trim();
        }
        #endregion
        #region 取单个字符的拼音声母(字符)(大写)
        /// <summary>
        /// 取单个字符的拼音声母 例如：张三转换成了Z
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetSpellCharUpper(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return c;
            if (i < 0xB0C5) return "A";
            if (i < 0xB2C1) return "B";
            if (i < 0xB4EE) return "C";
            if (i < 0xB6EA) return "D";
            if (i < 0xB7A2) return "E";
            if (i < 0xB8C1) return "F";
            if (i < 0xB9FE) return "G";
            if (i < 0xBBF7) return "H";
            if (i < 0xBFA6) return "J";
            if (i < 0xC0AC) return "K";
            if (i < 0xC2E8) return "L";
            if (i < 0xC4C3) return "M";
            if (i < 0xC5B6) return "N";
            if (i < 0xC5BE) return "O";
            if (i < 0xC6DA) return "P";
            if (i < 0xC8BB) return "Q";
            if (i < 0xC8F6) return "R";
            if (i < 0xCBFA) return "S";
            if (i < 0xCDDA) return "T";
            if (i < 0xCEF4) return "W";
            if (i < 0xD1B9) return "X";
            if (i < 0xD4D1) return "Y";
            if (i < 0xD7FA) return "Z";
            return c;
        }
        #endregion
        #region 取单个字符的拼音声母(字符)(小写)
        /// <summary>
        /// 取单个字符的拼音声母 例如：张三转换成了z
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetSpellCharLower(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return c;
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "j";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return c;
        }
        #endregion
        #region 随机生成汉字
        /// <summary>
        /// 随机生成汉字
        /// </summary>
        /// <param name="length">要生成的汉字个数</param>
        /// <returns></returns>
        public static string GetChineseRandom(int length)
        {
            if (length == 0) return "";
            string strChinese = string.Empty;
            //获取GB2312编码页（表） 
            Encoding gb = Encoding.GetEncoding("gb2312");
            //调用函数产生4个随机中文汉字编码 
            object[] bytes = CreateRegionCode(length);
            for (int i = 0; i < length; i++)
            {
                strChinese += gb.GetString((byte[])System.Convert.ChangeType(bytes[i], typeof(byte[])));
            }
            return strChinese;
        }
        /// <summary>
        /// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将四个字节数组存储在object数组中。 
        /// </summary>
        /// <param name="strlength">需要产生的汉字个数</param>
        /// <returns></returns>
        public static object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random((int)DateTime.Now.Ticks);

            //定义一个object数组用来 
            object[] bytes = new object[strlength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
            每个汉字有四个区位码组成 
            区位码第1位和区位码第2位作为字节数组第一个元素 
            区位码第3位和区位码第4位作为字节数组第二个元素 
            */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的 种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = System.Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = System.Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中 
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(str_r, i);
            }
            return bytes;
        }
        #endregion
        #region 随机生成姓名
        //static string[] _firstName = new string[]{  
        //            "白","毕","卞","蔡","曹","岑","常","车","陈","成" ,"程","池","邓","丁","范","方","樊","费","冯","符"
        //           ,"傅","甘","高","葛","龚","古","关","郭","韩","何" ,"贺","洪","侯","胡","华","黄","霍","姬","简","江"
        //           ,"姜","蒋","金","康","柯","孔","赖","郎","乐","雷" ,"黎","李","连","廉","梁","廖","林","凌","刘","柳"
        //           ,"龙","卢","鲁","陆","路","吕","罗","骆","马","梅" ,"孟","莫","母","穆","倪","宁","欧","区","潘","彭"
        //           ,"蒲","皮","齐","戚","钱","强","秦","丘","邱","饶" ,"任","沈","盛","施","石","时","史","司徒","苏","孙"
        //           ,"谭","汤","唐","陶","田","童","涂","王","危","韦" ,"卫","魏","温","文","翁","巫","邬","吴","伍","武"
        //           ,"席","夏","萧","谢","辛","邢","徐","许","薛","严" ,"颜","杨","叶","易","殷","尤","于","余","俞","虞"
        //           ,"元","袁","岳","云","曾","詹","张","章","赵","郑" ,"钟","周","邹","朱","褚","庄","卓","东方","上官"
        //           ,"令狐","申屠","欧阳" };

        //static string _lastNameMale = "伟刚勇毅俊峰强军平保东文辉力明永健世广志义兴良海山仁波宁贵福生龙元全国胜学祥才发武新利清飞彬富顺信子杰涛昌成康星光天达安岩中茂进林有坚和彪博诚先敬震振壮会思群豪心邦承乐绍功松善厚庆磊民友裕河哲江超浩亮政谦亨奇固之轮翰朗伯宏言若鸣朋斌梁栋维启克伦翔旭鹏泽晨辰士以建家致树炎德行时泰盛雄琛钧冠策腾楠榕风航弘";

        //static string _lastNameFeMa = "秀娟英华慧巧美娜静淑惠珠翠雅芝玉萍红娥玲芬芳燕彩春菊兰凤洁梅琳素云莲真环雪荣爱妹霞香月莺媛艳瑞凡佳嘉琼勤珍贞莉桂娣叶璧璐娅琦晶妍茜秋珊莎锦黛青倩婷姣婉娴瑾颖露瑶怡婵雁蓓纨仪荷丹蓉眉君琴蕊薇菁梦岚苑婕馨瑗琰韵融园艺咏卿聪澜纯毓悦昭冰爽琬茗羽希宁欣飘育滢馥筠柔竹霭凝鱼晓欢霄枫芸菲寒伊亚宜可姬舒影荔枝思丽墨";

        //public static List<string> GetName(string sex, int count)
        //{
        //    string firstName = string.Empty;
        //    string lastName = string.Empty;
        //    for (int i = 0; i < count; i++)
        //    {
        //        Random rnd = new Random((int)DateTime.Now.Ticks);
        //        firstName = _firstName[rnd.Next(0, _firstName.Length - 1)];
        //        if (sex == "男")
        //        {
        //           //lastName=  _lastNameMale.
        //        }
        //        else if (sex == "女")
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }

        //}

        //public static List<string> GetNames(string sex, int count)
        //{
        //    int fLength = _firstName.Length;
        //    int lLength = 0;

        //    string nameSource = string.Empty;
        //    if (sex=="男")
        //    {
        //        lLength = _lastNameMale.Length;
        //        nameSource = _lastNameMale;
        //    }
        //    else if (sex == "女")
        //    {
        //        lLength = _lastNameFeMa.Length;
        //        nameSource = _lastNameFeMa;
        //    }
        //    else
        //    {
        //        nameSource = string.Concat(_lastNameMale, _lastNameFeMa);
        //        lLength = nameSource.Length;
        //    }
        //    List<string> names = new List<string>();

        //    System.Threading.Thread.Sleep(20);
        //    int[] Indexfn = GenerateNonRepeatArray(count, count);
        //    System.Threading.Thread.Sleep(20);
        //    int[] Indexln1 = GenerateNonRepeatArray(count, count);
        //    System.Threading.Thread.Sleep(20);
        //    int[] Indexln2 = GenerateNonRepeatArray(count, count);

        //    int nameLength = nameSource.Length;

        //    for (int i = 0; i < count; i++)
        //    {
        //        int v1 = Indexfn[i];
        //        int v2 = Indexln1[i];
        //        int v3 = Indexln2[i];

        //        if (v1 >= fLength)
        //        {
        //            int j = Indexfn[i] / fLength;
        //            v1 = Indexfn[i] - fLength * j;
        //        }
        //        if (v2 >= nameLength)
        //        {
        //            int j = Indexln1[i] / nameLength;
        //            v2 = Indexln1[i] - nameLength * j;
        //        }
        //        if (v3 >= nameLength)
        //        {
        //            int j = Indexln2[i] / nameLength;
        //            v3 = Indexln2[i] - nameLength * j;
        //        }

        //        if (v1 < 0 || v2 < 0 || v3 < 0)
        //        {
        //            Console.WriteLine();
        //        }

        //        if (v1 >= _firstName.Length || v2 >= nameSource.Length || v3 >= nameSource.Length)
        //        {
        //            Console.WriteLine();
        //        }

        //        string n1 = string.Empty;
        //        string n2 = string.Empty;
        //        string n3 = string.Empty;

        //        names.Add(string.Format("{0}{1}{2}", _firstName[v1], nameSource[v2], nameSource[v3]));

        //    }

        //    return names;
        //}
        ///// <summary>
        ///// 返回指定范围内不重复的随机数组，如范围小于数组长度，则返回最多能生成的随机数组
        ///// </summary>
        ///// <param name="max"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //private static int[] GenerateNonRepeatArray(int max, int count)
        //{
        //    if (max <= 0) { throw new Exception("范围不能等于或小于0"); }
        //    int size = count > max ? count - max : 0;
        //    List<int> temp = new List<int>();
        //    Random ran = new Random();
        //    while (temp.Count < (count - size))
        //    {
        //        int item = ran.Next(max);
        //        if (!temp.Contains(item))
        //        {
        //            temp.Add(item);
        //        }
        //    }
        //    return temp.ToArray();
        //}
        #endregion
        #region 繁体转简体
        /// <summary>
        /// 繁体转简体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSimplifiedChinese(string input)
        {
            return Strings.StrConv(input, VbStrConv.SimplifiedChinese, 0);
        }
        #endregion
        #region 简体转繁体
        public static string ToTraditionalChinese(string input)
        {
            return Strings.StrConv(input, VbStrConv.TraditionalChinese, 0);
        }
        #endregion
    }
}
