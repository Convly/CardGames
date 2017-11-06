//using System.Net.Http;
//using Microsoft.Owin.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OwinSelfhostSample;
//using System.Net;

//namespace Server
//{
//    class WebServer
//    {
//        private WebServer()
//        {
//            StartOptions options = new StartOptions("http://*:8989")
//            {
//                ServerFactory = "Microsoft.Owin.Host.HttpListener"
//            };

//            WebApp.Start<Startup>("http://*:8989");
//        }

//        private static WebServer instance = null;
//        public static WebServer Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new WebServer();
//                }
//                return instance;
//            }
//        }

//        public void Dispose()
//        {

//        }
//    }
//}