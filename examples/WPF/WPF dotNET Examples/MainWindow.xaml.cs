using Microsoft.Win32;
using NationalInstruments.VBAI;
using NationalInstruments.VBAI.Structures;
using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
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

namespace WPF_dotNET_Examples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageViewer imageViewer;
        VBAIEngine engine;

        public MainWindow()
        {
            InitializeComponent();

            imageViewer = new ImageViewer();
            imageViewer.ZoomToFit = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            wfhost.Child = imageViewer;
            btnOpenInspection.IsEnabled = false;
            btnInspect.IsEnabled = false;
        }

        private void Launch_engine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // passing an emtpy string for the version will launch the latest Vision Builder AI engine available on the system.
                engine = new VBAIEngine("APITest", "", true);
                btnLaunchEngine.IsEnabled = false;
                btnOpenInspection.IsEnabled = true;
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Open_inspection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if(openFileDialog.ShowDialog() == true)
                {
                    engine.OpenInspection(openFileDialog.FileName);
                    InspectionInfo info = engine.GetInspectionInfo();
                    txtInspectionName.Text = info.productName;
                    btnInspect.IsEnabled = true;
                }
            }
            catch (VBAIException ex)
            {
                txtInspectionName.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void Inspect_Click(object sender, RoutedEventArgs e)
        {
            bool newImageAvailable, inspectionStatus;
            VBAIDateTime timeStamp;

            try
            {
                engine.RunInspectionOnce(-1);
                VisionImage image = engine.GetInspectionImage("", 1, 1, out newImageAvailable);
                imageViewer.Attach(image);
                engine.GetInspectionResults(out timeStamp, out inspectionStatus);
                rdbInspectionPass.IsChecked = inspectionStatus;
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (engine != null)
                    engine.CloseConnection(true);
            }
            catch (VBAIException ex)
            {
                MessageBox.Show(ex.Message);
            }

            System.Windows.Application.Current.Shutdown();
        }
    }
}
