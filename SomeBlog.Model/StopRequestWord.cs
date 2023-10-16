namespace SomeBlog.Model
{
    /// <summary>
    /// /.well-known/acme-challenge
    /// /wp-admin/
    /// 
    /// gibi kotu niyetli oldugu belli olan istekler, ignore edilir ya direk banlanir
    /// </summary>
    public class StopRequestWord : Core.ModelBase
    {

        public string Word { get; set; }

        /// <summary>
        /// Bu true ise, isteği yapan ip direk banlanir, 7 gün isteklerine cvp verilmez
        /// </summary>
        public bool ShouldBan { get; set; }
    }
}
