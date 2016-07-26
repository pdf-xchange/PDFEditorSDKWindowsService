using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService1
{

	public partial class Service1 : ServiceBase
	{

		public static PDFXEdit.IPXV_Inst m_Inst = null;

		protected System.Threading.Thread m_thread;
		protected System.Threading.ManualResetEvent m_shutdownEvent;
		protected TimeSpan m_delay;

		public Service1()
		{
			// create a new timespan object
			// with a default of 5 seconds delay.
			m_delay = new TimeSpan(0, 0, 0, 5, 0);
			InitializeComponent();
		}

		protected void ServiceMain()
		{
			bool bSignaled = false;
			int nReturnCode = 0;

			while (true)
			{
				// wait for the event to be signaled
				// or for the configured delay
				bSignaled = m_shutdownEvent.WaitOne(m_delay, true);

				// if we were signaled to shutdown, exit the loop
				if (bSignaled == true)
				{
					System.IO.FileStream file = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "ServiceStop.txt");
					try
					{
						m_Inst.Shutdown(0);
						String str = "Success";
						byte[] info = new UnicodeEncoding().GetBytes(str);
						file.Write(info, 0, info.Length);
					}
					catch (Exception e)
					{
						String str = e.ToString();
						byte[] info = new UnicodeEncoding().GetBytes(str);
						file.Write(info, 0, info.Length);
					}
					break;
				}
					

				// let's do some work
				if (m_Inst == null)
					nReturnCode = Execute();
			}
		}

		protected virtual int Execute()
		{
			System.IO.FileStream file = System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "ServiceStart.txt");
			try
			{
				m_Inst = new PDFXEdit.PXV_Inst();
				m_Inst.Init(null, "");
				PDFXEdit.IPXC_Inst pxcInst = (PDFXEdit.IPXC_Inst)m_Inst.GetExtension("PXC");

				string sFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\")) + "Numerated.pdf";
				PDFXEdit.IPXC_Document doc = pxcInst.OpenDocumentFromFile(sFilePath, null);

				String str = "Number of pages: " + doc.Pages.Count.ToString();
				byte[] info = new UnicodeEncoding().GetBytes(str);
				file.Write(info, 0, info.Length);
			}
			catch (Exception e)
			{
				String str = e.ToString();
				byte[] info = new UnicodeEncoding().GetBytes(str);
				file.Write(info, 0, info.Length);
			}
			return -1;
		}


		protected override void OnStart(string[] args)
		{
			// create our thread start object to wrap our delegate method
			ThreadStart ts = new ThreadStart(this.ServiceMain);

			// create the manual reset event and
			// set it to an initial state of unsignaled
			m_shutdownEvent = new ManualResetEvent(false);

			// create the worker thread
			m_thread = new Thread(ts);

			// go ahead and start the worker thread
			m_thread.Start();
		}

		protected override void OnStop()
		{
			// signal the event to shutdown
			m_shutdownEvent.Set();

			// wait for the thread to stop giving it 10 seconds
			m_thread.Join(10000);

			
		}
	}
}
