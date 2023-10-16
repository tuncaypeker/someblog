namespace SomeBlog.AdGuard
{
    public interface IGuard
    {
        bool ClearBlackList();
        Dto.ShouldShowAdDto ShouldShowAds(string ipAddress, int blogId);
    }
}