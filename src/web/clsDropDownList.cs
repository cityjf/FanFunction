using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace FanFunction
{
    /// <summary>
    /// web项目用于绑定下拉框的类
    /// </summary>
    public class clsDropDownList
    {
        /// <summary>
        /// 通用绑定下拉框(默认会在第一列加上请选择...)
        /// </summary>
        /// <param name="objDDL">要绑定的下拉框</param>
        /// <param name="strValueField">取值的字段</param>
        /// <param name="strTextField">显示的字段</param>
        /// <param name="strTableName">表名</param>
        public static void Bind_DropDownList(DropDownList objDDL, string strValueField, string strTextField, string strTableName)
        {
            string strSQL = "select " + strValueField + ", " + strTextField + " from " + strTableName;
            objDDL.DataValueField = strValueField;
            objDDL.DataTextField = strTextField;
            objDDL.DataSource = FanFunction.db.DBHelper.GetDataTable(strSQL);
            objDDL.DataBind();
            //默认在第一列插入请选择...
            objDDL.Items.Insert(0, new ListItem("请选择...", "0"));
            objDDL.SelectedIndex = 0;
        }
        /// <summary>
        /// 通用绑定下拉框(可选条件，可选是否在第一列插入请选择)
        /// </summary>
        /// <param name="objDDL">要绑定的下拉框</param>
        /// <param name="strValueField">取值的字段</param>
        /// <param name="strTextField">显示的字段</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strWhere">条件串</param>
        /// <param name="li">如果要在第一行插入“请选择...”等，请传递。否则插入null</param>
        public static void Bind_DropDownList(DropDownList objDDL, string strValueField, string strTextField, string strTableName, string strWhere, ListItem li)
        {
            //获取某学院所有专业信息
            string strSQL = "select " + strValueField + ", " + strTextField + " from " + strTableName + " where " + strWhere;
            objDDL.DataValueField = strValueField;
            objDDL.DataTextField = strTextField;
            objDDL.DataSource = FanFunction.db.DBHelper.GetDataTable(strSQL);
            objDDL.DataBind();
            if (li != null)
            {
                objDDL.Items.Insert(0, li);
                objDDL.SelectedIndex = 0;
            }
        }
    }
}
