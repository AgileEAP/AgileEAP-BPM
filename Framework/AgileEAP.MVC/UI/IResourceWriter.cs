using System;
using System.IO;
namespace AgileEAP.MVC.UI
{
    interface IResourceWriter
    {
        void Metas(System.IO.TextWriter Output);
        void HeadCss(System.IO.TextWriter Output);
        void HeadScripts(System.IO.TextWriter Output);
        void FootScripts(System.IO.TextWriter Output);
        void WriteResources(TextWriter Output, string resourceType, string resourceName);
    }
}
