using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using HTTP.lib;

namespace HTTP
{
    public partial class Form1 : Skin_Mac
    {
        public Form1()
        {
            InitializeComponent();
        }
        public configobj obj = new configobj();
        public string yunversion;
        public string nowversion;
        public bool opengame = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (configmethod.hasConfig() != true)
            {
                configmethod.initConfig();
                          
            }
            nowversion = configmethod.LoadConfig(obj);
            LoadMainform();
         
            skinTextBox1.Text += "\r\n";
            skinTextBox1.Text += "双击图片进入游戏";
            if (obj.NowVersion != yunversion)
            {
                skinTextBox1.Text = "游戏需要更新";
                skinProgressBar1.Visible = true;
                label2.Visible = true;
                Thread th = new Thread(UpdateThread);
                    th.IsBackground = true;
                th.Start();
                    //点确定的代码
                  
                    obj.NowVersion = yunversion;
                    configmethod.SaveConfig(obj);
                   
               
            }
            else
            {


            }
             
        }

        /// <summary>
        /// 新线程更新游戏
        /// </summary>
        private void UpdateThread()
        {
            UpdateGame();
        }

        private void UpdateGame()
        {

            //skinTextBox1.Text +=   DownloadFile("http://gengxin-1253115816.costj.myqcloud.com/heroes.db3", "heroes.db3", "sql");

            //skinTextBox1.Text += DownloadFile("http://gengxin-1253115816.costj.myqcloud.com/286FE9924483F382029EF68BA6C260B3C2563BF9.hfs", "286FE9924483F382029EF68BA6C260B3C2563BF9.hfs", "hfs");
            //skinTextBox1.Text = "更新成功";

            DownloadFile("http://gengxin-1253115816.costj.myqcloud.com/heroes.db3", Application.StartupPath + @"\zh-CN\sql","heroes.db3", skinProgressBar1,label2 );
            DownloadFile("http://gengxin-1253115816.costj.myqcloud.com/286FE9924483F382029EF68BA6C260B3C2563BF9.hfs", Application.StartupPath + @"\zh-CN\hfs","286FE9924483F382029EF68BA6C260B3C2563BF9.hfs", skinProgressBar1, label2);
            opengame = false;
            Invoke(new MethodInvoker(() =>
            {
                // your code
                label2.Text = "更新完成";
                skinTextBox1.Text = "双击图片进入游戏";
            }));

        }

        /// <summary>
        /// 读取界面
        /// </summary>
        /// <returns></returns>
        private void LoadMainform()
        {
            skinPictureBox1.Image = LoadJPG("http://gengxin-1253115816.costj.myqcloud.com/gengxintu.jpg");
            skinChatRichTextBox1.Text = LoadTxt("http://gengxin-1253115816.costj.myqcloud.com/gengxinrizhi.txt");
            skinLabel1.Text = LoadTxt("http://gengxin-1253115816.costj.myqcloud.com/zuixinbanben.txt");
            yunversion = skinLabel1.Text;
            skinPictureBox2.Image = LoadJPG("http://gengxin-1253115816.costj.myqcloud.com/zhengchang.png");
        }



        private string DownloadFile(string url, string FileName,string Dir)
        {
            Stream resstream = WebRequest.Create(url).GetResponse().GetResponseStream();

            HasDirectory(Dir);
            string FileAllPath = Application.StartupPath+"/"+"zh-CN/"+ Dir +"/"+ FileName;
            Stream fsstream = new FileStream(FileAllPath, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = resstream.Read(bArr, 0, (int)bArr.Length);
            while(size>0)
            {
                fsstream.Write(bArr, 0, size);
                size = resstream.Read(bArr, 0, (int)bArr.Length);
            }
            fsstream.Close();
            resstream.Close();
            return "更新"+FileAllPath+"成功";
            
        }

        public void HasDirectory(string PathName)
        {
            if(Directory.Exists(PathName))
            {
                return;
            }
            else
            {
                Directory.CreateDirectory(PathName);
                return;
            }
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string LoadTxt(string url)
        {
            Stream stream = WebRequest.Create(url).GetResponse().GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
            {
                string str = reader.ReadToEnd();
                return str;
            }
        }


        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Image LoadJPG(string url)
        {
            Image O_Image = Image.FromStream(WebRequest.Create(url).GetResponse().GetResponseStream());
            return O_Image;
        }

     

        /// <summary>
        /// 带参数启动
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "-stage -dev -lang zh-CN -noupdate";

                Process[] proc = Process.GetProcessesByName("Vindictus.exe");
                    if (proc.Length == 0)
                {
                    Process myprocess = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
                    myprocess.StartInfo = startInfo;
                    //通过以下参数可以控制exe的启动方式，具体参照 myprocess.StartInfo.下面的参数，如以无界面方式启动exe等
                    myprocess.StartInfo.UseShellExecute = false;

                    myprocess.Start();
                    return true;
                    //不存在...
                }
                    else
                {
                    foreach (Process process in proc) process.Kill();
                } 
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
            }
            return false;
        }

        private void skinPictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (opengame != true)
            {

                string[] arg = new string[] { "-state", "-dev", "-lang", "-zh-CN", "-noupdate" };
                StartProcess(Application.StartupPath + @"\zh-CN\Vindictus.exe", arg);
                skinPictureBox2.Image = LoadJPG("http://gengxin-1253115816.costj.myqcloud.com/dianran.png");

                skinTextBox1.Text = "游戏启动中，请稍后";
                opengame = true;
            }
            else
            {
                MessageBox.Show("游戏已启动，请勿重复点击");
            }
            
        }



        ///下载补丁并显示在进度条中
        public void DownloadFile(string URL, string filepath,string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)
        {
            HasDirectory(filepath);
            filename = filepath + "/" + filename;
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                   
                    Invoke(new MethodInvoker(() =>
                    {
                        prog.Maximum = (int)totalBytes;
                    }));
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            prog.Value = (int)totalDownloadedByte;
                        }));
                      
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    Invoke(new MethodInvoker(() =>
                    {
                        // your code
                        label1.Text = "当前补丁下载进度" + percent.ToString() + "%";
                    }));
                   
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                }
                so.Close();
                st.Close();
                Invoke(new MethodInvoker(() =>
                {
                    // your code
                    label1.Text = "补丁" + filename + "下载完成";
                }));

               
            }
            catch (System.Exception ex)
            {
               
            }
        }

    }
}
