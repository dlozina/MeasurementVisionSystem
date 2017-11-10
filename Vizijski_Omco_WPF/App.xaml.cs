using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using HalconDotNet;


namespace VizijskiSustavWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PIzvjestaji pIzvjestaji;
        public static PPostavke pPostavke;
        public static PDimenzije pDimenzije; 
        public static PSrh pSrh;
        public static PValovitost pValovitost;
        public static PSablja pSablja;
        public static PKut pKut;
        public static PRucno pRucno;
        public static PLCInterface PLC;
        public static string  ReportPath = "reports/ControlSheet.xlsx";
        public static MainWindow mwHandle;
        public static ReportInterface MainReportInterface;
        public static Algoritmi AutoSearch = new Algoritmi();
        public static HDevelopExport HDevExp;
        private bool edgeDetection=false;



        public App()
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            InitializeComponent();
            MainReportInterface = ((ReportInterface)Application.Current.FindResource("MainReport"));
           
            PLC = ((PLCInterface)Application.Current.FindResource("PLCinterf"));
            pIzvjestaji = new PIzvjestaji();
            pPostavke = new PPostavke();
            pDimenzije = new PDimenzije();
            pSrh = new PSrh();
            pValovitost = new PValovitost();
            pSablja = new PSablja();
            pKut = new PKut();
            pRucno = new PRucno();
            pRucno = new PRucno();

            App.PLC.StartCyclic();
            App.PLC.Update_Online_Flag += new PLCInterface.OnlineMarker(PLCInterface_PLCOnlineChanged);
            App.PLC.Update_100_ms += new PLCInterface.UpdateHandler(PLC_Update_100_ms);
            App.HDevExp.UpdateResult += new HDevelopExport.UpdateHandler(HalconUpdate);
            

        }

        private void PLC_Update_100_ms(PLCInterface sender, PLCInterfaceEventArgs e)
        {
            String msg = "";

            // Start analize slike PRVOG RUBA S1
            if (((bool)e.StatusData.Kamere.CAM4ZahtjevZaAnalizomS1.Value) && (!edgeDetection)) //Edge detection
            {

                HDevExp.InitHalcon();
                Thread exportThread = new Thread(new ThreadStart(this.RunDia1));
                exportThread.Start();
            }
            edgeDetection = (bool)e.StatusData.Kamere.CAM4ZahtjevZaAnalizomS1.Value == true; //Edge detection help marker

            // Start analize slike PRVOG RUBA S2
            if (((bool)e.StatusData.Kamere.CAM4ZahtjevZaAnalizomS2.Value) && (!edgeDetection))
            {

                HDevExp.InitHalcon();
                Thread exportThread = new Thread(new ThreadStart(this.RunDia2));
                exportThread.Start();
            }
            edgeDetection = (bool)e.StatusData.Kamere.CAM4ZahtjevZaAnalizomS2.Value == true;

            // Prioritet poruka (najviši prioritet je na dnu)
            //if ((bool)e.StatusData.RotacijskaOs.AutomaticActive.Value) msg = "MJERENJE SRHA U TIJEKU";
            //if ((bool)e.StatusData.VertikalnaOs.AutomaticActive.Value) msg = "MJERENJE VALOVITOSTI U TIJEKU";
            //if ((bool)e.StatusData.Ticalo.AutomaticActive.Value) msg = "MJERENJE DIMENZIJA U TIJEKU";
            //if ((bool)e.StatusData.HorizontalnaOs.ReferencedX.Value == false) msg = "OS X NIJE REFERENCIRANA";
            //if ((bool)e.StatusData.HorizontalnaOs.ReferencedY.Value == false) msg = "OS Y NIJE REFERENCIRANA";
            //if ((bool)e.StatusData.HorizontalnaOs.FaultX.Value == true) msg = "GREŠKA OSI X";
            //if ((bool)e.StatusData.HorizontalnaOs.FaultY.Value == true) msg = "GREŠKA OSI Y";
            if (mwHandle != null)
            { 
            mwHandle.tb_statusMessage.Dispatcher.BeginInvoke((Action)(() => { mwHandle.tb_statusMessage.Text = msg; }));
            }

            
        }

        private void RunDia1()
        {

            HDevExp.RunHalconS1();

        }

        private void RunDia2()
        {

            HDevExp.RunHalconS2();

        }

        private void HalconUpdate(HDevelopExport sender, HalconEventArgs e)
        {
            App.PLC.WriteTag(PLC.STATUS.Kamere.CAM4Rezultat, e.PXvalue);
            App.PLC.WriteTag(PLC.STATUS.Kamere.CAM4AnalizaOk, true);
            App.PLC.WriteTag(PLC.STATUS.Kamere.CAM4AnalizaOk, false);
        }


        // Event handler koji se poziva kad PLC postane online ili offline (Ethernet kabel se spoji ili odspoji).
        private void PLCInterface_PLCOnlineChanged(object sender, OnlineMarkerEventArgs e)
        {
            if (e.OnlineMark)
            {
                mwHandle.onlineFlag.Dispatcher.BeginInvoke((Action)(() => {mwHandle.onlineFlag.Fill = new LinearGradientBrush(Colors.Green, Colors.White, 0.0); }));
                mwHandle.t_connectionStatus.Dispatcher.BeginInvoke((Action)(() => { mwHandle.t_connectionStatus.Text = "PLC Status: Online"; }));
            }
            else
            {
                mwHandle.onlineFlag.Dispatcher.BeginInvoke((Action)(() => { mwHandle.onlineFlag.Fill = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FF979797"), Colors.White, 0.0); }));
                mwHandle.t_connectionStatus.Dispatcher.BeginInvoke((Action)(() => { mwHandle.t_connectionStatus.Text = "PLC Status: Offline"; }));
            }
        }

    }
}
