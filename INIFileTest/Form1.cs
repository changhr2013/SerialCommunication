using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INIFileTest
{
    public partial class Form1 : Form
    {

  #region "声明变量"

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,string key,string val,string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key,string def,StringBuilder retval,int size,string filePath);

        private string strFilePath = Application.StartupPath + "\\FileConfig.ini";//获取INI文件路径
        private string strSec =""; //INI文件名
         
        #endregion


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 写入按钮事件
        /// </summary>
        private void writeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //根据INI文件名设置要写入INI文件的节点名称
                //此处的节点名称可以根据实际需要进行配置
                strSec = Path.GetFileNameWithoutExtension(strFilePath);
                WritePrivateProfileString(strSec, "Name", nameTxt.Text.Trim(), strFilePath);
                WritePrivateProfileString(strSec, "Sex", sexTxt.Text.Trim(), strFilePath);
                WritePrivateProfileString(strSec, "Age", ageTxt.Text.Trim(), strFilePath);
                WritePrivateProfileString(strSec, "Address", addressTxt.Text.Trim(), strFilePath);
                MessageBox.Show("写入成功","提示信息",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }
        }

        /// <summary>
        /// 读取按钮事件
        /// </summary>        
        private void readBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(strFilePath))
            {
                strSec = Path.GetFileNameWithoutExtension(strFilePath);
                nameTxt.Text = ContentValue(strSec,"Name");
                sexTxt.Text = ContentValue(strSec, "Sex");
                ageTxt.Text = ContentValue(strSec, "Age");
                addressTxt.Text = ContentValue(strSec, "Address");
            }
            else
            {
                MessageBox.Show("INI文件不存在");
            }
        }

        /// <summary>
        /// 读取INI文件中内容方法
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section,key,"",temp,1024,strFilePath);
            return temp.ToString();
        }
    }
}
