//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Web.MackdownLib.ContentProviders;
//using Web.MackdownLib;

//namespace MarkdownServiceExtention
//{
//    public class MarkdownServiceImplement
//    {
//        private static readonly Lazy<IMarkdownService> Singleton;

//        public static IMarkdownService Instance
//        {
//            get { return Singleton.Value; }
//        }

//        static MarkdownServiceImplement()
//        {
//            Singleton = new Lazy<IMarkdownService>(
//                () => new MarkdownService(new FileContentProvider(HttpContext.Current.Server.MapPath("~/App_Data/MarkdownFiles"))), true);
//        }
//    }
//}