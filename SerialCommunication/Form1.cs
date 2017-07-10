using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using INIFILE;
using System.Text.RegularExpressions;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        SerialPort sp1 = new SerialPort();

        public Form1()
        {
            InitializeComponent();
        }

        #region Form_load
        //窗口加载
        private void Form1_Load(object sender, EventArgs e)
        {
            //加载所有
            INIFILE.Profile.LoadProfile();

            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "300":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "1200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "2400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "4800":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 5;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 6;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 7;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 8;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }

            //预置波特率
            switch (Profile.G_DATABITS)
            {
                case "5":
                    cbDataBits.SelectedIndex = 0;
                    break;
                case "6":
                    cbDataBits.SelectedIndex = 1;
                    break;
                case "7":
                    cbDataBits.SelectedIndex = 2;
                    break;
                case "8":
                    cbDataBits.SelectedIndex = 3;
                    break;
                default:
                    {
                        MessageBox.Show("数据位预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

            }
            //预置停止位
            switch (Profile.G_STOP)
            {
                case "1":
                    cbStop.SelectedIndex = 0;
                    break;
                case "1.5":
                    cbStop.SelectedIndex = 1;
                    break;
                case "2":
                    cbStop.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("停止位预置参数错误。", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
            }

            //预置校验位
            switch (Profile.G_PARITY)
            {
                case "NONE":
                    cbParity.SelectedIndex = 0;
                    break;
                case "ODD":
                    cbParity.SelectedIndex = 1;
                    break;
                case "EVEN":
                    cbParity.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("校验位预置参数错误。","异常提示信息",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
            }

            //检查是否含有串口  
               string[] str = SerialPort.GetPortNames();  
               if (str == null)  
               {  
                   MessageBox.Show("本机没有串口！", "信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);  
                   return;  
               }  
      
               //添加串口项目  
               foreach (string s in System.IO.Ports.SerialPort.GetPortNames())  
               {
                   //获取有多少个COM口  
                   cbSerial.Items.Add(s);  
               }  
      
               //串口设置默认选择项  
               //设置cbSerial的默认选项  
               cbSerial.SelectedIndex = 0;
               sp1.BaudRate = 9600;

            //这个类中不检查跨线程的调用是否合法
            /*.NET2.0以后加强了安全机制，不允许在WinForm中直接跨线程访问控件的属性*/
               Control.CheckForIllegalCrossThreadCalls = false;

               sp1.DataReceived += new SerialDataReceivedEventHandler(sp1_DataReceived);

            //发送和接收格式默认是字符串
               rbSendStr.Checked = true;
               rbRcvStr.Checked = true;

            //设置数据读取超时为1秒
               sp1.ReadTimeout = 1000;

            //设置编码
               sp1.Encoding = System.Text.Encoding.GetEncoding("UTF-8");

            //关闭串口
               sp1.Close();
        }
        #endregion

        #region sp1_DataReceived
        /// <summary>
        /// 对接收的数据进行处理的方法
        /// </summary>
        void sp1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp1.IsOpen)
            {
                //输出当前时间
                DateTime dateTime = DateTime.Now;

                txtRcv.Text += dateTime.GetDateTimeFormats('f')[0].ToString() + "\r\n";

                //改变前景色为蓝色
                txtRcv.SelectAll();
                txtRcv.ForeColor = Color.Blue;

                //BytestoRead:sp1接收的字符个数
                byte[] receiveBytes = new byte[sp1.BytesToRead];
                sp1.Read(receiveBytes, 0, receiveBytes.Length);
                sp1.DiscardInBuffer();

                //rdRcvStr:"接收字符串"单选按钮
                if (rbRcvStr.Checked)
                {
                    string receiveStr = ByteToString(receiveBytes);
                    txtRcv.AppendText(receiveStr + "\r\n");
                }
                else
                {
                    try
                    {
                        string strRcv = ByteToHex(receiveBytes);
                        txtRcv.AppendText(strRcv + "\r\n");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"异常提示信息",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        txtSend.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("请打开某个串口", "信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        #endregion

        #region btnClear_Click
        /// <summary>
        /// "清空"按钮事件
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRcv.Text = "";
        }
        #endregion

        #region btnExit_Click
        /// <summary>
        /// "退出"按钮事件
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region btnSend_Click
        /// <summary>
        /// 点击“发送按钮”事件
        /// </summary>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //发送前判断串口是否打开
            if (!sp1.IsOpen)
            {
                MessageBox.Show("请先打开串口！","提示信息",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            //判断cbTimeSend是否被勾选，来决定是否启动Timer控件进行定时发送
            if (cbTimeSend.Checked)
            {
                timeSend.Enabled = true;
            }
            else
            {
                timeSend.Enabled = false;
            }

            string strSend = txtSend.Text;

            if (rbSend16.Checked==true)
            {
                //如果以16进制形式发送，将16进制数转换为byte数组进行发送
                byte[] byteBuffer = HexToByte(strSend);
                sp1.Write(byteBuffer, 0, byteBuffer.Length);
            }
            else if(rbSendStr.Checked==true)
            {
                //如果以字符串形式发送，将字符串转换为byte数组进行发送
                    byte[] byteBuffer = StringToByte(txtSend.Text);
                    sp1.Write(byteBuffer, 0, byteBuffer.Length);
            }
            else
            {
                MessageBox.Show("请选择数据发送的格式！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region timeSend_Tick
        /// <summary>
        /// 定时发送的定时器Timer
        /// </summary>
        private void timeSend_Tick(object sender, EventArgs e)
        {
            //转换时间间隔
            string strSecond = txtSecond.Text;

            try
            {
                //Interval的单位是ms
                int isecond = int.Parse(strSecond) * 1000;
                timeSend.Interval = isecond;

                if (timeSend.Enabled==true)
                {
                    //如果timeSend空间是可用状态，用按钮的PerformClick()方法生成按钮的点击事件。
                    btnSend.PerformClick();
                }
            }
            catch (Exception ex)
            {
                //若出现异常，就将Timer控件状态设为不可用，然后抛出异常提示
                timeSend.Enabled = false;
                MessageBox.Show("设置的时间间隔格式错误!\n"+ex.Message, "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region btnSwitch_Click
        /// <summary>
        /// 打开/关闭串口的按钮事件
        /// </summary>
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (!sp1.IsOpen)
            {
                try
                {
                    //设置串口号
                    string serialName = cbSerial.SelectedItem.ToString();
                    sp1.PortName = serialName;

                    //对串口参数进行设置
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;

                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    //设置波特率
                    sp1.BaudRate = iBaudRate;
                    //设置数据位
                    sp1.DataBits = iDateBits;
                    //设置停止位
                    switch (cbStop.Text)
                    {
                        case "1": sp1.StopBits = StopBits.One; break;
                        case "1.5": sp1.StopBits = StopBits.OnePointFive; break;
                        case "2": sp1.StopBits = StopBits.Two; break;                        
                        default:
                            MessageBox.Show("停止位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                    //设置校验位
                    switch (cbParity.Text)
                    {
                        case "无": sp1.Parity = Parity.None; break;
                        case "奇校验": sp1.Parity = Parity.Odd; break;
                        case "偶校验": sp1.Parity = Parity.Even; break;
                        default:
                            MessageBox.Show("校验位参数不正确！", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }

                    //如果串口为打开状态，就先关闭一下来初始化串口
                    if (sp1.IsOpen==true)
                    {
                        sp1.Close();
                    }

                    //状态栏设置
                    tsSpNum.Text = "串口号：" + sp1.PortName + "|";
                    tsBaudRate.Text = "波特率：" + sp1.BaudRate + "|";
                    tsDataBits.Text = "数据位：" + sp1.DataBits + "|";
                    tsStopBits.Text = "停止位：" + sp1.StopBits + "|";
                    tsParity.Text = "校验位：" + sp1.Parity;

                    //设置必要的控件为不可用状态
                    SettingControls(0);

                    //打开串口
                    sp1.Open();
                    //将按钮内容设置为"关闭串口"
                    btnSwitch.Text = "关闭串口";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误：" + ex.Message, "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timeSend.Enabled = false;

                    //设置必要的控件为可用状态
                    SettingControls(1);
                    return;                    
                }
            }
            else
            {
                //状态栏设置
                tsSpNum.Text = "串口号：未指定|";
                tsBaudRate.Text = "波特率：未指定|";
                tsDataBits.Text = "数据位：未指定|";
                tsStopBits.Text = "停止位：未指定|";
                tsParity.Text = "校验位：未指定|";

                //恢复控件功能，设置必要控件为可用状态
                SettingControls(1);

                //关闭串口
                sp1.Close();

                //设置按钮内容为"打开串口"
                btnSwitch.Text = "打开串口";

                //关闭计时器Timer
                timeSend.Enabled = false;
            }
        }
        #endregion

        #region Form1_FormClosing
        /// <summary>
        /// 程序窗口关闭时的事件
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存配置到INI文件，关闭打开的串口
            INIFILE.Profile.SaveProfile();
            sp1.Close();
        }
        #endregion

        #region btnSave_Click
        /// <summary>
        /// "保存设置"按钮点击事件
        /// </summary>        
        private void btnSave_Click(object sender, EventArgs e)
        {
            //设置各"串口设置"
            string strBaudRate = cbBaudRate.Text;
            string strDataBits = cbDataBits.Text;
            string strStopBits = cbStop.Text;
            Int32 iBaudRate = Convert.ToInt32(strBaudRate);
            Int32 iDataBits = Convert.ToInt32(strDataBits);

            //保存波特率设置
            Profile.G_BAUDRATE = iBaudRate + "";
            //保存数据位设置
            Profile.G_DATABITS = iDataBits + "";
            //保存停止位设置
            switch (cbStop.Text)
            {
                case "1": Profile.G_STOP = "1"; break;
                case "1.5": Profile.G_STOP = "1.5"; break;
                case "2": Profile.G_STOP = "2"; break;
                default:
                    MessageBox.Show("停止位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            //保存校验位设置
            switch (cbParity.Text)
            {
                case "无": Profile.G_PARITY = "NONE"; break;
                case "奇校验": Profile.G_PARITY = "ODD"; break;
                case "偶校验": Profile.G_PARITY = "EVEN"; break;
                default:
                    MessageBox.Show("校验位参数不正确", "异常提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            //保存设置
            Profile.SaveProfile();

            //保存成功提示
            MessageBox.Show("设置保存成功！","信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        #endregion

        #region txtSend_KeyPress
        /// <summary>
        /// 对发送框的输入进行校验
        /// </summary>
        private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rbSend16.Checked == true)
            {
                //正则匹配,\b表示退格键（对16进制的输入进行格式校验）
                string pattern = "[0-9a-fA-F]|\b|0x|0X| ";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(e.KeyChar.ToString());

                if (match.Success)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = false;
            }
        }
        #endregion

        #region txtSecond_KeyPress
        /// <summary>
        /// 对定时输入的时间进行校验
        /// </summary>
        private void txtSecond_KeyPress(object sender, KeyPressEventArgs e)
        {
            string pattern = "[0-9]|\b";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(e.KeyChar.ToString());

            if (match.Success)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Method ByteToHex and HexToByte
        //byte字节数组转16进制
        private string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            foreach (byte data in comByte)
            {
                builder.Append(Convert.ToString(data,16).PadLeft(2,'0').PadRight(3,' '));
            }
            return builder.ToString().ToUpper();
        }

        //16进制转字节数组
        private byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");
            msg = msg.Replace("0x", "");
            msg = msg.Replace("0X", "");
            byte[] comBuffer = new byte[msg.Length / 2];

            for (int i = 0; i < msg.Length; i+=2)
            {
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
            }
            return comBuffer;
        }

        //字符串转字节型数组
        private byte[] StringToByte(string msgtxt)
        {
            return System.Text.Encoding.UTF8.GetBytes(msgtxt);
        }

        //字节型数组转字符串
        private string ByteToString(byte[] buffer)
        {
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
        #endregion

        #region rbSend16_CheckedChanged
        //当发送方式发生变化时更改cbSendHex的可用状态并清空当前发送框的内容
        private void rbSend16_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSend16.Checked==true)
            {
                cbSendHex.Enabled = false;
                txtStrTo16.Text = "";
            }
            else
            {
                cbSendHex.Enabled = true;
            }
            txtSend.Text = "";
        }
        #endregion

        #region Method SettingControls
        //用于设置控件的可用状态，canUse 参数-> 1：可用  0：不可用
        private void SettingControls(int canUse)
        {
            if (canUse==1)
            {
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;
            }
            else if(canUse==0)
            {
                cbSerial.Enabled = false;
                cbBaudRate.Enabled = false;
                cbDataBits.Enabled = false;
                cbStop.Enabled = false;
                cbParity.Enabled = false;
            }
        }
        #endregion

        //用于监听cbSendHex按钮的状态
        private void cbSendHex_CheckedChanged(object sender, EventArgs e)
        {
            //如果cbSendHex按钮选中就翻译发送框的内容
            if (cbSendHex.Checked==true)
            {
                txtStrTo16.Text = ByteToHex(StringToByte(txtSend.Text));
            }
            else
            {
                txtStrTo16.Text = "";
            }
        }
        //用于监听txtSend的文本输入，如果txtSend内容发生变化，txtStrTo16实时将字符串翻译为16进制
        private void txtSend_TextChanged(object sender, EventArgs e)
        {
            if (cbSendHex.Checked == true&&rbSend16.Checked==false)
            {
                txtStrTo16.Text = ByteToHex(StringToByte(txtSend.Text));
            }
        }
    }
}
