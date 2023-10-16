namespace SomeBlog.Model
{
    public class BlogKeyword : Core.ModelBase
    {
        public BlogKeyword()
        {
            LastPositionMInSC = 0;
            PrevPositionMInSC = 0;
            LastPositionDInSC = 0;
            PrevPositionDInSC = 0;
        }

        public int BlogId { get; set; }
        public int KeywordId { get; set; }
        public int ContentId { get; set; }

        /// <summary>
        /// Google Search Console'dan alinan son position
        /// </summary>
        public double LastPositionMInSC { get; set; }

        /// <summary>
        /// Google Search Console'dan alinan son position
        /// </summary>
        public double PrevPositionMInSC { get; set; }

        /// <summary>
        /// Google Search Console'dan alinan son position
        /// </summary>
        public double LastPositionDInSC { get; set; }

        /// <summary>
        /// Google Search Console'dan alinan son position
        /// </summary>
        public double PrevPositionDInSC { get; set; }

        public System.DateTime LastUpdate { get; set; }


        //SomeBlog.Model.Enums.SeoTools
        public int Source { get; set; }

        public virtual Keyword Keyword { get; set; }
        public virtual Content Content { get; set; }
    }
}
