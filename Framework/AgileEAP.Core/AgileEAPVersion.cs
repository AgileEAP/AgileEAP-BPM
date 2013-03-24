
namespace AgileEAP.Core
{
    public class AgileEAPVersion
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ICP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string eCloudName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string eClientName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the store version
        /// </summary>
        public static string CurrentVersion
        {
            get
            {
                return "1.0";
            }
        }
    }
}
