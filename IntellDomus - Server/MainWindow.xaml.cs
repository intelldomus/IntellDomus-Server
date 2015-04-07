using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace IntellDomus___Server
{

    public static class variableGlobal //Classe delle variabili globali all'interno del namespace
    {
        public static TextBox logBox2;
    }


    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            variableGlobal.logBox2 = logBox;


            //ConnecModule cm = new ConnecModule(); //Server Start

            //Face recognizon
            FaceRecognition fr = new FaceRecognition(smabellImage);
        }
       

        public void writeLog(String log)
        {
            logBox.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                    new Action(() => variableGlobal.logBox2.AppendText(log + "\n")));
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //Chiusura Applicazione
        }

    }
}
