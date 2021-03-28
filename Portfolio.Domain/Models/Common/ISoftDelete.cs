namespace Portfolio.Domain.Models.Common
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
